using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Dtos
{
    public class PaginationResultDto<T> where T : class
    {
        public IEnumerable<T> Data { get; set; }
        public int TotalCount { get; set; }


        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public int? NextPage { get; set; }
        public int? PreviousPage { get; set; }



        public int TotalPages { get; set; }
        public bool HasNextPage { get; set; }
        public bool HasPreviousPage { get; set; }
    }
}
