using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.TaskAttachments.queries.DownloadAttachmentById
{
    public record DownloadAttachmentResultDto(
        Stream FileStream,
        string FileName,
        string ContentType
    );
}
