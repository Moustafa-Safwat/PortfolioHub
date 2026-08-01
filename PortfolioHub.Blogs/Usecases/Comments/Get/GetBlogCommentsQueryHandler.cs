using Ardalis.Result;
using MediatR;
using PortfolioHub.Blogs.Domain.Entities;
using PortfolioHub.Blogs.Domain.Interfaces;
using PortfolioHub.Blogs.Endpoints.Comments;
using PortfolioHub.SharedKernal.Config;
using PortfolioHub.Users;
using ValidBuild.Sharedkernal.Domain.CQRS;

namespace PortfolioHub.Blogs.Usecases.Comments.Get;

internal sealed class GetBlogCommentsQueryHandler
(
    IBlogsRepo blogsRepo,
    ISender sender
) : IQueryHandler<GetBlogCommentsQuery, IReadOnlyCollection<GetCommentsResponse>>
{
    public async Task<Result<IReadOnlyCollection<GetCommentsResponse>>> Handle(
        GetBlogCommentsQuery request,
        CancellationToken cancellationToken)
    {
        var blogResult = await blogsRepo.GetByIdAsync(request.BlogId, cancellationToken);

        if (!blogResult.IsSuccess)
            return blogResult.PropagateFailure<BlogPost, IReadOnlyCollection<GetCommentsResponse>>();

        var blogPost = blogResult.Value;

        // Pagination should normally be applied only to root comments.
        var rootComments = blogPost.BlogComments
            .Where(comment =>
                !comment.IsDeleted &&
                comment.Replies is not null)
            .OrderByDescending(comment => comment.CreatedAtUtc)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        if (rootComments.Count == 0)
        {
            return Result.Success<IReadOnlyCollection<GetCommentsResponse>>(
                Array.Empty<GetCommentsResponse>());
        }

        // Include user IDs from both root comments and all nested replies.
        var userIds = rootComments
            .SelectMany(FlattenCommentTree)
            .Where(comment => !comment.IsDeleted)
            .Select(comment => comment.UserId)
            .Distinct()
            .ToArray();

        var usersResult = await sender.Send(
            new GetUsersByIdQuery(userIds),
            cancellationToken);

        if (!usersResult.IsSuccess)
        {
            return usersResult.PropagateFailure<
                IEnumerable<GetUserDto>,
                IReadOnlyCollection<GetCommentsResponse>>();
        }

        var usersById = usersResult.Value
            .GroupBy(user => user.Id)
            .ToDictionary(
                group => group.Key,
                group => group.First());

        var response = rootComments
            .Select(comment => MapComment(
                comment,
                usersById))
            .ToArray();

        return Result.Success<IReadOnlyCollection<GetCommentsResponse>>(
            response);
    }

    private static IEnumerable<BlogPostComment> FlattenCommentTree(
        BlogPostComment comment)
    {
        yield return comment;

        foreach (var reply in comment.Replies)
        {
            foreach (var nestedComment in FlattenCommentTree(reply))
            {
                yield return nestedComment;
            }
        }
    }

    private static GetCommentsResponse MapComment(
        BlogPostComment comment,
        IReadOnlyDictionary<Guid, GetUserDto> usersById)
    {
        usersById.TryGetValue(
            comment.UserId,
            out var user);

        var replies = comment.Replies
            .Where(reply => !reply.IsDeleted)
            .OrderBy(reply => reply.CreatedAtUtc)
            .Select(reply => MapComment(
                reply,
                usersById))
            .ToArray();

        return new GetCommentsResponse(
            comment.Id,
            user?.Id ?? Guid.Empty,
            user?.FirstName ?? "-",
            user?.LastName ?? "-",
            comment.Content,
            comment.CreatedAtUtc,
            comment.UpdatedAtUtc is not null,
            replies);
    }
}