using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.WorkSpaceUserDashboard.Queries.GetWorkSpaceUserDashboard
{
    public class GetWorkSpaceUserDashboardQueryValidator : AbstractValidator<GetWorkSpaceUserDashboardQuery>
    {
        public GetWorkSpaceUserDashboardQueryValidator()
        {
            RuleFor(p => p.UserId)
                .NotEmpty().WithMessage("UserId is required.");

            RuleFor(p => p.WorkSpaceId)
                .NotEmpty().WithMessage("WorkSpaceId is required.");

            RuleFor(p => p.SpecificallyForThisUser)
                .NotNull().WithMessage("SpecificallyForThisUser is required.");
        }
    }
}