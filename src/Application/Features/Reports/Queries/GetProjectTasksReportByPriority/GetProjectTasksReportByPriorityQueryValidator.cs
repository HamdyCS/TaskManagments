using FluentValidation;

namespace Application.Features.Reports.Queries.GetProjectTasksReportByPriority
{
    public class GetProjectTasksReportByPriorityQueryValidator : AbstractValidator<GetProjectTasksReportByPriorityQuery>
    {
        public GetProjectTasksReportByPriorityQueryValidator()
        {
            RuleFor(x => x.ProjectId)
                .GreaterThan(0).WithMessage("Project ID must be greater than 0");
        }
    }
}
