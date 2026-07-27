using FluentValidation;

namespace Application.Features.TaskComments.Queries.GetCommentById
{
    public class GetCommentByIdQueryValidator : AbstractValidator<GetCommentByIdQuery>
    {
        public GetCommentByIdQueryValidator()
        {
            RuleFor(x => x.WorkSpaceId)
                .GreaterThan(0).WithMessage("Workspace ID must be greater than 0");

            RuleFor(x => x.ProjectId)
                .GreaterThan(0).WithMessage("Project ID must be greater than 0");

            RuleFor(x => x.TaskId)
                .GreaterThan(0).WithMessage("Task ID must be greater than 0");

            RuleFor(x => x.CommentId)
                .GreaterThan(0).WithMessage("Comment ID must be greater than 0");
        }
    }
}
