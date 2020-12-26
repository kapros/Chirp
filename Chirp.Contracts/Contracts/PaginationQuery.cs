using System;
using System.Collections.Generic;
using System.Text;

namespace Chirp.Contracts
{
    public class PaginationQuery
    {
        public PaginationQuery() : this(1, 100) { }

        public PaginationQuery(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber < 1 ? 1 : pageNumber;
            PageSize = pageSize > 100 ? 100 : pageSize;
        }

        public int PageNumber { get; set; }

        public int PageSize { get; set; }
    }
}
