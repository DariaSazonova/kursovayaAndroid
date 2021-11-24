using System;
using System.Collections.Generic;
using System.Text;

namespace booksShop8.ViewModels
{
    public class OrderDetails
    {
        public int OrderId { get; set; }
        public int BookId { get; set; }
        public int? Quantities { get; set; }
        public decimal? Cost { get; set; }
    }
}
