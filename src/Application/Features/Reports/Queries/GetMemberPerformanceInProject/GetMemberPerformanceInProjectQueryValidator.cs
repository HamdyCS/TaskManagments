using FluentValidation;

namespace Application.Features.Reports.Queries.GetMemberPerformanceInProject
{
    public class GetMemberPerformanceInProjectQueryValidator : AbstractValidator<GetMemberPerformanceInProjectQuery>
    {
        public GetMemberPerformanceInProjectQueryValidator()
        {
            RuleFor(x => x.ProjectId)
                .GreaterThan(0).WithMessage("Project ID must be greater than 0");

            RuleFor(x => x.MemberId)
                .NotEmpty().WithMessage("Member ID is required");
        }
    }
}
