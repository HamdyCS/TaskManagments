using FluentValidation;

namespace Application.Features.WorkSpaces.Queries.GetUserWorkSpaceRole
{
    public class GetUserWorkSpaceRoleQueryValidator : AbstractValidator<GetUserWorkSpaceRoleQuery>
    {
        public GetUserWorkSpaceRoleQueryValidator()
        {
            RuleFor(x => x.WorkSpaceId)
                .GreaterThan(0).WithMessage("Workspace ID must be greater than 0");

            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("User ID is required");
        }
    }
}
