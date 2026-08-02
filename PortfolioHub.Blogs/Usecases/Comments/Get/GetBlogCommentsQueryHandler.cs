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

        var blogComments = blogPost.BlogComments.Where(c => c.ParentCommentId is null);

        if (!blogComments.Any())
            return Result.SuccessWithMessage("No comments found for this artical");

        var userIds = blogComments
            .Select(comment => comment.UserId)
            .Concat(blogComments.SelectMany(comment => comment.Replies)
                                .Select(reply => reply.UserId)
            ).Distinct();

        var getUserQuery = new GetUsersByIdQuery(userIds);
        var userQueryResult = await sender.Send(getUserQuery);
        if (!userQueryResult.IsSuccess)
            return userQueryResult.PropagateFailure<IEnumerable<GetUserDto>, IReadOnlyCollection<GetCommentsResponse>>(); ;

        var usersData = userQueryResult.Value;

        var response = blogComments
            .Select(c => MapComment(c, usersData))
            .ToList()
            .AsReadOnly();

        return Result.Success<IReadOnlyCollection<GetCommentsResponse>>(response);
    }

    private GetCommentsResponse MapComment(BlogPostComment blogComments, IEnumerable<GetUserDto> usersData)
    {
        return new GetCommentsResponse
        (
          blogComments.Id,
          blogComments.UserId,
          usersData.FirstOrDefault(u => u.Id == blogComments.UserId)?.FirstName ?? "NA",
          usersData.FirstOrDefault(u => u.Id == blogComments.UserId)?.LastName ?? "NA",
          blogComments.Content,
          blogComments.CreatedAtUtc,
          blogComments.UpdatedAtUtc is not null,
          blogComments.Replies.Select(reply => MapComment(reply, usersData)).ToList()
        );
    }
}