using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.WorkSpaceUsers.queries.GetAllWorkSpaceUsers
{
    public class GetAllWorkSpaceUsersQueryValidator : AbstractValidator<GetAllWorkSpaceUsersQuery>
    {
        public GetAllWorkSpaceUsersQueryValidator()
        {
            RuleFor(x => x.WorkSpaceId)
                .NotNull().WithMessage("WorkSpaceId is required")
                .Must(x => x > 0).WithMessage("WorkSpaceId must be greater than 0");
        }
    }
}
