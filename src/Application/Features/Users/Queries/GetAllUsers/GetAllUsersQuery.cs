using Application.Common.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Users.Queries.GetAllUsers
{
    public sealed record GetAllUsersQuery(PaginationRequestDto PaginationRequestDto) : IRequest<ErrorOr<PaginationResultDto<UserDto>>>;
    
}
