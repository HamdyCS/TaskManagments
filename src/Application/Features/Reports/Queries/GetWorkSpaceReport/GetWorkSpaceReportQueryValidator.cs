using FluentValidation;

namespace Application.Features.Reports.Queries.GetWorkSpaceReport
{
    public class GetWorkSpaceReportQueryValidator : AbstractValidator<GetWorkSpaceReportQuery>
    {
        public GetWorkSpaceReportQueryValidator()
        {
            RuleFor(x => x.WorkspaceId)
                .GreaterThan(0).WithMessage("Workspace ID must be greater than 0");
        }
    }
}
