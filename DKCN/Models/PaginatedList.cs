using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DKCN.Models
{
    public class PaginatedList<T> : List<T>
    {
        public int PageIndex { get; set; }
        public int TotalPage { get; set; }
        public int StartPage { get; set; }
        public int EndPage { get; set; }

        public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            TotalPage = (int)Math.Ceiling(count / (double)pageSize);

            StartPage = PageIndex - 5;
            EndPage = PageIndex + 4;
            if(StartPage <= 0)
            {
                EndPage = EndPage - (StartPage - 1);
                StartPage = 1;
            }
            if(EndPage > TotalPage)
            {
                EndPage = TotalPage;

                if(EndPage > 10)
                {
                    StartPage = EndPage - 9;
                }
            }
            this.AddRange(items);
        }

        public static PaginatedList<T> Create(List<T> source, int pageIndex, int pageSize)
        {
            var count = source.Count();
            var items = source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            
            return new PaginatedList<T>(items, count, pageIndex, pageSize);
        }

        public bool HasPreviousPage => PageIndex > 1;
        public bool HasNextPage => PageIndex < TotalPage ;


    }
}