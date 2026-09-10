using FluentValidation;

namespace Application.Features.WorkSpaces.Queries.GetWorkSpaceDetails
{
    public class GetWorkSpaceDetailsQueryValidator : AbstractValidator<GetWorkSpaceDetailsQuery>
    {
        public GetWorkSpaceDetailsQueryValidator()
        {
            RuleFor(x => x.WorkSpaceId)
                .NotEmpty().WithMessage("WorkSpaceId is required");
        }
    }
}