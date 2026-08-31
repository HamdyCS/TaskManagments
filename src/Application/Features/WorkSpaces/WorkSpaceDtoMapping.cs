using Mapster;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.WorkSpaces
{
    internal class WorkSpaceDtoMapping : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<WorkSpace,WorkSpaceDto>()
                .Map(dest => dest.CreatedByName, src => src.CreatedBy.FullName);
        }
    }
}
