using Application.Common.Dtos;

namespace Application.Features.Users.Queries.GetAllRegularUsers
{
    public sealed record GetAllRegularUsersQuery(PaginationRequestDto PaginationRequestDto) : IRequest<ErrorOr<PaginationResultDto<UserDto>>>;
}
