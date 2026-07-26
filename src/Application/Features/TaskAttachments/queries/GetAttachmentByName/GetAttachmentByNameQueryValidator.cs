using FluentValidation;

namespace Application.Features.TaskAttachments.Queries.GetAttachmentByName
{
    public class GetAttachmentByNameQueryValidator : AbstractValidator<GetAttachmentByNameQuery>
    {
        public GetAttachmentByNameQueryValidator()
        {
            RuleFor(x => x.WorkSpaceId)
                .GreaterThan(0).WithMessage("Workspace ID must be greater than 0");

            RuleFor(x => x.ProjectId)
                .GreaterThan(0).WithMessage("Project ID must be greater than 0");

            RuleFor(x => x.TaskId)
                .GreaterThan(0).WithMessage("Task ID must be greater than 0");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(255).WithMessage("Name must not exceed 255 characters");
        }
    }
}
