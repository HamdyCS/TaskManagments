using Application.Common.Dtos;

namespace Application.Features.Users.Queries.GetAllAdminUsers
{
    public sealed record GetAllAdminUsersQuery(PaginationRequestDto PaginationRequestDto) : IRequest<ErrorOr<PaginationResultDto<UserDto>>>;
}
