using Domain.Entities;
using Mapster;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Users
{
    public class UserDtoMapping : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            //user to userDto
            config.NewConfig<User, UserDto>();

            //userDto to user
            config.NewConfig<UserDto, User>()
                .Ignore(dest => dest.Id)
                .Ignore(dest => dest.Email);
        }
    }
}
