using FluentValidation;

namespace Application.Features.Projects.Queries.GetAllProjects
{
    public class GetAllProjectsQueryValidator : AbstractValidator<GetAllProjectsQuery>
    {
        public GetAllProjectsQueryValidator()
        {
            RuleFor(x => x.WorkSpaceId)
                .GreaterThan(0).WithMessage("Workspace ID must be greater than 0");

            RuleFor(x => x.PaginationRequest.PageNumber)
                .GreaterThan(0).WithMessage("Page number must be greater than 0");

            RuleFor(x => x.PaginationRequest.PageSize)
                .GreaterThan(0).WithMessage("Page size must be greater than 0")
                .LessThanOrEqualTo(100).WithMessage("Page size must not exceed 100");
        }
    }
}
