using System;
using System.Collections.Generic;
using System.Text;

namespace booksShop8.ViewModels
{
    public class Basket
    {
        public static Dictionary<int, int> basketDiction = new Dictionary<int, int>();
        public static profile profil = new profile();
        public static List<Books> viewBooks = new List<Books>();
        public static Dictionary<int, string> authorDictionary = new Dictionary<int, string>();
    }
}
