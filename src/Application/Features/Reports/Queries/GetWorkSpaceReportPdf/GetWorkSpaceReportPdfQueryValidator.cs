using FluentValidation;

namespace Application.Features.Reports.Queries.GetWorkSpaceReportPdf
{
    public class GetWorkSpaceReportPdfQueryValidator : AbstractValidator<GetWorkSpaceReportPdfQuery>
    {
        public GetWorkSpaceReportPdfQueryValidator()
        {
            RuleFor(x => x.WorkspaceId)
                .GreaterThan(0).WithMessage("Workspace ID must be greater than 0");
        }
    }
}
