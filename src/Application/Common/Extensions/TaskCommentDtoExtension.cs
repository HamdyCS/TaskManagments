using Application.Common.Dtos;
using Application.Common.Exceptions;
using Application.Features.TaskComments;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Extensions
{
    public static class TaskCommentDtoExtension
    {
        public static PaginationResultDto<TaskCommentDto> ToPaginationResultDto(this IEnumerable<TaskCommentDto> allComments, int pageNumber, int pageSize)
        {
            var totalCount = allComments.Count();
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
            var skip = (pageNumber - 1) * pageSize;
            var data = allComments.Skip(skip).Take(pageSize).ToList();

            return new PaginationResultDto<TaskCommentDto>
            {
                Data = data,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize,
                NextPage = pageNumber < totalPages ? pageNumber + 1 : null,
                PreviousPage = pageNumber > 1 ? pageNumber - 1 : null,
                TotalPages = totalPages,
                HasNextPage = pageNumber < totalPages,
                HasPreviousPage = pageNumber > 1
            };
        }
    }
}
