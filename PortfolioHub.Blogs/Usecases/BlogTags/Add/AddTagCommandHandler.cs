using Ardalis.Result;
using PortfolioHub.Blogs.Domain.Entities;
using PortfolioHub.SharedKernal.Config;
using PortfolioHub.SharedKernal.Domain.Interfaces;
using ValidBuild.Sharedkernal.Domain.CQRS;

namespace PortfolioHub.Blogs.Usecases.BlogTags.Add;

internal sealed class AddTagCommandHandler
(
    IEntityRepo<BlogPostTag> blogTagRepo
) : ICommandHandler<AddTagCommand, Guid>
{
    public async Task<Result<Guid>> Handle(AddTagCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var getAllResult = await blogTagRepo.GetAllAsync(1, 100, cancellationToken);
            if (!getAllResult.IsSuccess)
                return getAllResult.PropagateFailure();

            var allExistingTags = getAllResult.Value.Select(tag => tag.Name);
            if (allExistingTags.Contains(request.Name))
            {
                var errors = new ErrorList(["Tag with the same name is already exists."]);
                return Result.Error(errors);
            }

            var blogPostTag = new BlogPostTag(request.Name);
            var result = await blogTagRepo.AddAsync(blogPostTag, cancellationToken);
            if (!result.IsSuccess) return result;

            var saveResult = await blogTagRepo.SaveChangesAsync(cancellationToken);
            if (!saveResult.IsSuccess) return saveResult;
            return Result.Success(blogPostTag.Id);
        }
        catch (Exception ex)
        {
            var errorList = new ErrorList(
              [$"Failed to add tag with name: {request.Name}",
                ex.Message,
                ex?.InnerException?.Message??string.Empty]);
            return Result.Error(errorList);
        }
    }
}
