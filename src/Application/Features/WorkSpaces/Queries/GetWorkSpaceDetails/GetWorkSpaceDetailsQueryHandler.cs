using Application.Common.Dtos.WorkSpace;
using Application.Common.Errors;
using Application.Common.Interfaces.Repositories;
using Mapster;

namespace Application.Features.WorkSpaces.Queries.GetWorkSpaceDetails
{
    public class GetWorkSpaceDetailsQueryHandler(
        IUnitOfWork unitOfWork,
        ILogger<GetWorkSpaceDetailsQueryHandler> logger
    ) : IRequestHandler<GetWorkSpaceDetailsQuery, ErrorOr<WorkSpaceDetailsDto>>
    {
        public async Task<ErrorOr<WorkSpaceDetailsDto>> Handle(
            GetWorkSpaceDetailsQuery request,
            CancellationToken cancellationToken)
        {
            var workSpaceId = request.WorkSpaceId;
            logger.LogInformation("Starting get workspace details for workspace {WorkSpaceId}", workSpaceId);


            logger.LogInformation("Getting workspace details for workspace {WorkSpaceId}", workSpaceId);
            var workSpace = await unitOfWork.WorkSpaceRepository.GetWorkSpaceDetailsAsync(workSpaceId);

            if (workSpace is null)
            {
                logger.LogWarning("Workspace {WorkSpaceId} not found", workSpaceId);
                return WorkSpaceErrors.WorkSpaceNotFoundById(workSpaceId);
            }

            logger.LogInformation("Getting workspace details for workspace {WorkSpaceId} successfully", workSpaceId);
            return workSpace.Adapt<WorkSpaceDetailsDto>();
        }
    }
}