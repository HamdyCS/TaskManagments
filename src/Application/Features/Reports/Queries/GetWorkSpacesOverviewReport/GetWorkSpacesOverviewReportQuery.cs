using Application.Common.Dtos.WorkSpaceOverview;
using Application.Common.Dtos.WorkSpacesOverview;
using ErrorOr;
using MediatR;

namespace Application.Features.Reports.Queries.GetWorkSpacesOverviewReport
{
    public sealed record GetWorkSpacesOverviewReportQuery(
        WorkSpaceOverviewQueryParameters QueryParameters) : IRequest<ErrorOr<WorkSpacesOverviewReportDto>>;
}
