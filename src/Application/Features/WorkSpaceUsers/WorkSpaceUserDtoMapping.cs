using Mapster;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.WorkSpaceUsers
{
    public class WorkSpaceUserDtoMapping : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
           config.NewConfig<WorkSpaceUser, WorkSpaceUserDto>()
                .Map(dest=>dest.FullName,src=>src.User.FullName)
                .Map(dest => dest.Email, src => src.User.Email);
        }
    }
}
