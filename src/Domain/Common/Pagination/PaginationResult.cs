using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Common.Pagination
{
    public class PaginationResult<T> where T : class
    {
        public IEnumerable<T> Data { get;}
        public int TotalCount { get;  }


        public int PageNumber { get; }
        public int PageSize { get;  }

        public int? NextPage { get; }
        public int? PreviousPage { get;}


        
        public int TotalPages => (int)Math.Ceiling((decimal)TotalCount / PageSize);
        public bool HasNextPage => PageNumber < TotalPages;
        public bool HasPreviousPage => PageNumber > 1;



        public PaginationResult(IEnumerable<T> data, int totalCount, int pageNumber, int pageSize)
        {
            this.Data = data;
            TotalCount = totalCount;
            PageNumber = pageNumber;
            PageSize = pageSize;

            this.NextPage = HasNextPage ? PageSize + 1 : null;
            this.PreviousPage = HasPreviousPage ? PageNumber - 1 : null;
        }
    }
}
