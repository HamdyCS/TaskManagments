using Application.Common.Dtos;
using Application.Common.Dtos.WorkSpaceOverview;
using ErrorOr;
using MediatR;

namespace Application.Features.Reports.Queries.GetWorkSpacesOverviewReportPdf
{
    public sealed record GetWorkSpacesOverviewReportPdfQuery(WorkSpaceOverviewQueryParameters WorkSpaceOverviewQueryParameters) : IRequest<ErrorOr<WorkSpacesOverviewReportPdfDto>>;
}
