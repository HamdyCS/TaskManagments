using FluentValidation;

namespace Application.Features.Reports.Queries.GetMemberPerformancesInWorkSpace
{
    public class GetMemberPerformancesInWorkSpaceQueryValidator : AbstractValidator<GetMemberPerformancesInWorkSpaceQuery>
    {
        public GetMemberPerformancesInWorkSpaceQueryValidator()
        {
            RuleFor(x => x.WorkspaceId)
                .GreaterThan(0).WithMessage("Workspace ID must be greater than 0");
        }
    }
}
