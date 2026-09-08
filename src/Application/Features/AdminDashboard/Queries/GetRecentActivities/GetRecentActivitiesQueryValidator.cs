using Application.Common.Dtos;
using FluentValidation;

namespace Application.Features.AdminDashboard.Queries.GetRecentActivities
{
    public class GetRecentActivitiesQueryValidator : AbstractValidator<GetRecentActivitiesQuery>
    {
        public GetRecentActivitiesQueryValidator()
        {
            RuleFor(x => x.PaginationRequestDto.PageNumber)
                .GreaterThan(0).WithMessage("Page number must be greater than 0");

            RuleFor(x => x.PaginationRequestDto.PageSize)
                .InclusiveBetween(1, 100).WithMessage("Page size must be between 1 and 100");
        }
    }
}
