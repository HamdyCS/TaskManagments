using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Dashboard.Queries.GetDashboard
{
    public class GetDashboardQueryValidator : AbstractValidator<GetDashboardQuery>
    {
        public GetDashboardQueryValidator()
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