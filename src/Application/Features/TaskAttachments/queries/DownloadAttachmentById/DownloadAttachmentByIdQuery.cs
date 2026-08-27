using Application.Features.TaskAttachments.queries.DownloadAttachmentById;
using ErrorOr;
using MediatR;

namespace Application.Features.TaskAttachments.Queries.DownloadAttachmentById
{
    public sealed record DownloadAttachmentByIdQuery(
        long WorkSpaceId,
        long ProjectId,
        long TaskId,
        long AttachmentId) : IRequest<ErrorOr<DownloadAttachmentResultDto>>;
}
