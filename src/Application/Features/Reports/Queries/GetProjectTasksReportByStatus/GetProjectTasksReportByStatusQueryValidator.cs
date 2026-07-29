using FluentValidation;

namespace Application.Features.Reports.Queries.GetProjectTasksReportByStatus
{
    public class GetProjectTasksReportByStatusQueryValidator : AbstractValidator<GetProjectTasksReportByStatusQuery>
    {
        public GetProjectTasksReportByStatusQueryValidator()
        {
            RuleFor(x => x.ProjectId)
                .GreaterThan(0).WithMessage("Project ID must be greater than 0");
        }
    }
}
