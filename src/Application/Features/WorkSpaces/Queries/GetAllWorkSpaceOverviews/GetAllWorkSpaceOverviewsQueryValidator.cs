using FluentValidation;

namespace Application.Features.WorkSpaces.Queries.GetWorkSpaceOverviews
{
    public class GetAllWorkSpaceOverviewsQueryValidator : AbstractValidator<GetAllWorkSpaceOverviewsQuery>
    {
        public GetAllWorkSpaceOverviewsQueryValidator()
        {

            RuleFor(x => x.PaginationRequestDto.PageNumber)
                .GreaterThan(0).WithMessage("PageNumber must be greater than 0");

            RuleFor(x => x.PaginationRequestDto.PageSize)
                .GreaterThan(0).WithMessage("PageSize must be greater than 0");
        }
    }
}