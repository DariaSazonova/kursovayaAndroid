using Xamarin.Forms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Android.Graphics;

namespace booksShop8.ViewModels
{
    public class Books
    {
        public int bookId { get; set; }
        public string bookName { get; set; }
        public string bookDescription { get; set; }
        public decimal bookCost { get; set; }
        public string author { get; set; }
        public string genre { get; set; }
        public string status { get; set; }
        public Nullable<int> bookMark { get; set; }
        public Nullable<int> bookMarkCount { get; set; }
        public Nullable<int> Quantities { get; set; }
        public byte[] bookImg { get; set; }
        public override bool Equals(object obj)
        {
            Books book = obj as Books;
            return this.bookId == book.bookId;
        }
        
    }
}
