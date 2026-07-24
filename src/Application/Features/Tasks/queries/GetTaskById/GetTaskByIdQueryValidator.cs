using FluentValidation;

namespace Application.Features.Tasks.Queries.GetTaskById
{
    public class GetTaskByIdQueryValidator : AbstractValidator<GetTaskByIdQuery>
    {
        public GetTaskByIdQueryValidator()
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
