using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Comman
{
    public sealed class PaginatedResult<TEntity>
    {
        //when he create obj can setting ,cann't reset it
        public PaginatedResult(int pageIndex, int count, int pageSize, IReadOnlyList<TEntity> data)
        {
            PageIndex = pageIndex;
            Count = count;
            PageSize = pageSize;
            Data = data;
        }

        public int  PageIndex { get;}
        public int Count { get;}
        public int PageSize { get; }
        public IReadOnlyList<TEntity> Data { get; }
    }
}
