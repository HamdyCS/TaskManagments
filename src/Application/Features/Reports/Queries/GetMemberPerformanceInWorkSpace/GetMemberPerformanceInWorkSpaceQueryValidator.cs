using FluentValidation;

namespace Application.Features.Reports.Queries.GetMemberPerformanceInWorkSpace
{
    public class GetMemberPerformanceInWorkSpaceQueryValidator : AbstractValidator<GetMemberPerformanceInWorkSpaceQuery>
    {
        public GetMemberPerformanceInWorkSpaceQueryValidator()
        {
            RuleFor(x => x.WorkspaceId)
                .GreaterThan(0).WithMessage("Workspace ID must be greater than 0");

            RuleFor(x => x.MemberId)
                .NotEmpty().WithMessage("Member ID is required");
        }
    }
}
