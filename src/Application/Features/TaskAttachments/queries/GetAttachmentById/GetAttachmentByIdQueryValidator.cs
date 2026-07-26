using FluentValidation;

namespace Application.Features.TaskAttachments.Queries.GetAttachmentById
{
    public class GetAttachmentByIdQueryValidator : AbstractValidator<GetAttachmentByIdQuery>
    {
        public GetAttachmentByIdQueryValidator()
        {
            RuleFor(x => x.WorkSpaceId)
                .GreaterThan(0).WithMessage("Workspace ID must be greater than 0");

            RuleFor(x => x.ProjectId)
                .GreaterThan(0).WithMessage("Project ID must be greater than 0");

            RuleFor(x => x.TaskId)
                .GreaterThan(0).WithMessage("Task ID must be greater than 0");

            RuleFor(x => x.AttachmentId)
                .GreaterThan(0).WithMessage("Attachment ID must be greater than 0");
        }
    }
}
