using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.WorkSpaces.commands.GetAllWorkSpaces
{
    public class GetAllWorkSpacesQueryValidator : AbstractValidator<GetAllWorkSpacesQuery>
    {
        public GetAllWorkSpacesQueryValidator()
        {
           
        }
    }
}
