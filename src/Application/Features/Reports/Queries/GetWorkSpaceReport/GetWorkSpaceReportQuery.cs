using Application.Common.Dtos;
using ErrorOr;
using MediatR;

namespace Application.Features.Reports.Queries.GetWorkSpaceReport
{
    public sealed record GetWorkSpaceReportQuery(long WorkspaceId) : IRequest<ErrorOr<WorkSpaceReportDto>>;
}
