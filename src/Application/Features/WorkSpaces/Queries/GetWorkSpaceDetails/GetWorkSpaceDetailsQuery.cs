using Application.Common.Dtos.WorkSpace;

namespace Application.Features.WorkSpaces.Queries.GetWorkSpaceDetails
{
    public sealed record GetWorkSpaceDetailsQuery(long WorkSpaceId) : IRequest<ErrorOr<WorkSpaceDetailsDto>>;
}