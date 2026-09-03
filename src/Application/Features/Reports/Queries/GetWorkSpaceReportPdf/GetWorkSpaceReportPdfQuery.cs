using Application.Common.Dtos;
using ErrorOr;
using MediatR;

namespace Application.Features.Reports.Queries.GetWorkSpaceReportPdf
{
    public sealed record GetWorkSpaceReportPdfQuery(long WorkspaceId) : IRequest<ErrorOr<WorkSpaceReportPdfDto>>;
}
