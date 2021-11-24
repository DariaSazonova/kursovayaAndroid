using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;

using System.Net;
using System.Text.Json;

namespace booksShop8.ViewModels
{
    class Service
    {
        string UrlBook = "http://192.168.1.42/myserver3/book/"; // обращайте внимание на конечный слеш
        string UrlSatus = "http://192.168.1.42/myserver3/bookstatus/";
        string UrlAuthor = "http://192.168.1.42/myserver3/author/";
        string UrlGenre = "http://192.168.1.42/myserver3/genrer/";
        string UrlUser = "http://192.168.1.42/myserver3/user/";
        string UrlClient = "http://192.168.1.42/myserver3/client/";
        string UrlOrder = "http://192.168.1.42/myserver3/order/";
        string UrlOrderD = "http://192.168.1.42/myserver3/ordersdetail/";
        string UrlOrderController = "http://192.168.1.42/myserver3/ordercontroller1/";
        // настройки для десериализации для нечувствительности к регистру символов

        // настройка клиента
        private HttpClient GetClient()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            return client;
        }

        // получаем все книги 
        public async Task<string>   Get()
        {
            HttpClient client = GetClient();
            string result = await client.GetStringAsync(UrlBook);
            return result;
        }
        // получаем книгу 
        public async Task<string> GetBook(int id)
        {
            HttpClient client = GetClient();
            string result = await client.GetStringAsync(UrlBook + id.ToString());
            return result;
        }
        // получаем автора
        public async Task<string> GetAuthor(int id)
        {
            HttpClient client = GetClient();
            string result = await client.GetStringAsync(UrlAuthor+id.ToString());
            return result;
        }
        // получаем жанр
        public async Task<string> GetGenre(int id)
        {
            HttpClient client = GetClient();
            string result = await client.GetStringAsync(UrlGenre + id.ToString());
            return result;
        }
        //получаем статус 
        public async Task<string> GetBookStatus(int id)
        {
            HttpClient client = GetClient();
            string result = await client.GetStringAsync(UrlSatus+id.ToString());
            return result;
        }
        //получаем пользователя
        public async Task<string> Getuser (string id)
        {
            HttpClient client = GetClient();
            string result = await client.GetStringAsync(UrlUser + id.ToString());
            return result;
        }
        // получаем клиента 
        public async Task<string> Getclient(string id)
        {
            HttpClient client = GetClient();
            string result = await client.GetStringAsync(UrlClient+ id.ToString());
            return result;
        }
   

        // получаем номер заказа
        public async Task<string> GetOrderNum(int id)
        {
            HttpClient client = GetClient();
            string result = await client.GetStringAsync(UrlOrderController + id.ToString());
            return result;
        }
        // добавляем новый заказ
        public async Task<Order> AddOrder(Order Order)
        {
            HttpClient client = GetClient();
            var response = await client.PostAsync(UrlOrder,
                new StringContent(
                    JsonSerializer.Serialize(Order),
                    Encoding.UTF8, "application/json"));

            if (response.StatusCode != HttpStatusCode.OK)
                return null;

            return JsonSerializer.Deserialize<Order>(
                await response.Content.ReadAsStringAsync());
        }
        // добавляем новый заказ в OrderDetails
        public async Task<OrderDetails> AddOrderDetails(OrderDetails OrderDetails)
        {
            HttpClient client = GetClient();
            var response = await client.PostAsync(UrlOrderD,
                new StringContent(
                    JsonSerializer.Serialize(OrderDetails),
                    Encoding.UTF8, "application/json"));

            if (response.StatusCode != HttpStatusCode.OK)
                return null;

            return JsonSerializer.Deserialize<OrderDetails>(
                await response.Content.ReadAsStringAsync());
        }

    }
}
