using Application.Common.Interfaces.Services;
using Application.Features.TaskAttachments;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Extensions
{
    public static class TaskAttachmentExtension
    {
        public static TaskAttachmentDto ToTaskAttachmentDto(this TaskAttachment taskAttachment, IFileUrlService fileUrlService)
        {
           var dto = new TaskAttachmentDto(
                taskAttachment.Id,
                taskAttachment.Name,
                fileUrlService.GetUrl(taskAttachment.Path),
                taskAttachment.CreatedAt
            );
            return dto;
        }

        public static List<TaskAttachmentDto> ToTaskAttachmentDtoList(this IEnumerable<TaskAttachment> taskAttachments, IFileUrlService fileUrlService)
        {
            var dtos = new List<TaskAttachmentDto>();
            foreach (var taskAttachment in taskAttachments)
            {
                dtos.Add(taskAttachment.ToTaskAttachmentDto(fileUrlService));
            }
            return dtos;
        }
    }
}
