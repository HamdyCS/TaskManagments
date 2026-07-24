using FluentValidation;

namespace Application.Features.Tasks.Queries.GetMyTaskById
{
    public class GetMyTaskByIdQueryValidator : AbstractValidator<GetMyTaskByIdQuery>
    {
        public GetMyTaskByIdQueryValidator()
        {
            RuleFor(x => x.WorkSpaceId)
                .GreaterThan(0).WithMessage("Workspace ID must be greater than 0");

            RuleFor(x => x.ProjectId)
                .GreaterThan(0).WithMessage("Project ID must be greater than 0");

            RuleFor(x => x.TaskId)
                .GreaterThan(0).WithMessage("Task ID must be greater than 0");

            RuleFor(x => x.AssignedToId)
                .NotEmpty().WithMessage("AssignedTo ID is required");
        }
    }
}
