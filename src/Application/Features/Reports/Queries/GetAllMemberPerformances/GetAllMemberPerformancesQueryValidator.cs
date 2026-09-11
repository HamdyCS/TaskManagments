using FluentValidation;

namespace Application.Features.Reports.Queries.GetAllMemberPerformances
{
    public class GetAllMemberPerformancesQueryValidator : AbstractValidator<GetAllMemberPerformancesQuery>
    {
        public GetAllMemberPerformancesQueryValidator()
        {
            RuleFor(x => x.PaginationRequestDto.PageNumber)
                .GreaterThan(0).WithMessage("Page number must be greater than 0");

            RuleFor(x => x.PaginationRequestDto.PageSize)
                .GreaterThan(0).WithMessage("Page size must be greater than 0");
        }
    }
}
