using Domain.Entities;
using Mapster;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

namespace Application.Features.Users.Commands.RegisterNewUser
{
    public class RegisterUserDtoMapping : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            //ignoreid
            config.NewConfig<RegisterUserDto, User>()
                .Ignore(u => u.Id)
                .Ignore(u => u.EmailConfirmed).Map(x => x.UserName, r =>
                new MailAddress(r.Email).User);
        }
    }
}
