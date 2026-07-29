using Application.Common.Dtos;
using ErrorOr;
using MediatR;

namespace Application.Features.Reports.Queries.GetProjectTasksReportByPriority
{
    public sealed record GetProjectTasksReportByPriorityQuery(long ProjectId) : IRequest<ErrorOr<List<TasksByPriorityReportDto>>>;
}
