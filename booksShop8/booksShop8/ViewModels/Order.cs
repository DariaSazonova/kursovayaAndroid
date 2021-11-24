using System;
using System.Collections.Generic;
using System.Text;

namespace booksShop8.ViewModels
{
    public class Order
    {
       public int ordernum { get; set; }
        public int? ClientId { get; set; }
        public int? EmployeeId { get; set; }
        public DateTime? DateFirst { get; set; }
        public DateTime? DateEnd { get; set; }
        public string status { get; set; }

    }
}
