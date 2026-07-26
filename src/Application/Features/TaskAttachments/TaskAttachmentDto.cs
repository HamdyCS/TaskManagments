namespace Application.Features.TaskAttachments
{
    public record TaskAttachmentDto(
        long Id,
        string Name,
        string Url,
        DateTime CreatedAt);
}
