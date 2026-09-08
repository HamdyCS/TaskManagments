using FluentValidation;

namespace Application.Features.AdminDashboard.Queries.GetAdminDashboard
{
    public class GetAdminDashboardQueryValidator : AbstractValidator<GetAdminDashboardQuery>
    {
        public GetAdminDashboardQueryValidator()
        {
            RuleFor(p => p.UserId)
                .NotEmpty().WithMessage("UserId is required.");
        }
    }
}
