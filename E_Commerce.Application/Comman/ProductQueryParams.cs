using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Comman
{
    //not any new spec i must add in service and controller.......
    public class ProductQueryParams
    {
        public int? BrandId { get; set; }
         public   int? TypeId { get; set; }
      public  string? SearchValue { get; set; }
        public ProductSortingOptions Sort{ get; set; }  //Enum because i have options, add 1 of them
        public int PageIndex { get; set; } = 1; //لو مبعتش عدد صفح عايزها ترجع رجع اول صقحه بس
        private int pageSize = 5;
        private int DefultPageSize = 5;
        private int MaxPageSize = 10;

        public int PageSize {
            get => pageSize;
            set=> pageSize=value > MaxPageSize?MaxPageSize :(value)<1? DefultPageSize:value;
        }//لازم اتاكد انه هيبعت رقم منطقي 
    }
}
