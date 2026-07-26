using FluentValidation;

namespace Application.Features.TaskAttachments.Queries.GetAttachmentsByTaskId
{
    public class GetAttachmentsByTaskIdQueryValidator : AbstractValidator<GetAttachmentsByTaskIdQuery>
    {
        public GetAttachmentsByTaskIdQueryValidator()
        {
            RuleFor(x => x.WorkSpaceId)
                .GreaterThan(0).WithMessage("Workspace ID must be greater than 0");

            RuleFor(x => x.ProjectId)
                .GreaterThan(0).WithMessage("Project ID must be greater than 0");

            RuleFor(x => x.TaskId)
                .GreaterThan(0).WithMessage("Task ID must be greater than 0");
        }
    }
}
