using Application.Common.Dtos;
using ErrorOr;
using MediatR;

namespace Application.Features.Reports.Queries.GetProjectTasksReportByStatus
{
    public sealed record GetProjectTasksReportByStatusQuery(long ProjectId) : IRequest<ErrorOr<List<TasksByStatusReportDto>>>;
}
