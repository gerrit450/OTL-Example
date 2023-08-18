using Bookstore;
using BookStore.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace BookStore.Controller
{
    public class BookStoreController
    {
        [HttpGet]
        [Route("Books")]
        public async Task<List<BookServiceBook>> GetAllBooks()
        {
            using (var span = DiagnosticsConfig.ActivitySource.StartActivity("get books from bookservice"))
            {
                span?.SetTag("Time", DateTime.Now.ToString());

                var client = new HttpClient();
                var response = await client.GetStringAsync($"https://localhost:1002/GetAllBooks/");
                var listOfBooks = JsonSerializer.Deserialize<List<BookServiceBook>>(response) ?? new List<BookServiceBook>();

                span?.SetTag("bookservice.books.count", listOfBooks.Count);

                return listOfBooks;
            }
        }

        [HttpGet]
        [Route("Book/{name}")]
        public async Task<BookServiceBook> GetBook(string name) // get a book
        {
            using (var span = DiagnosticsConfig.ActivitySource.StartActivity("get book from bookservice"))
            {
                var client = new HttpClient();
                var response = await client.GetStringAsync($"https://localhost:1002/GetBook/{name}");
                var book = JsonSerializer.Deserialize<BookServiceBook>(response) ?? new BookServiceBook();

                span?.SetTag("bookservice.book.name", book.Name);
                span?.SetTag("bookservice.book.author", book.Author);

                return book;
            }
        }

        [HttpDelete]
        [Route("Book/{name}")]
        public async Task RemoveBook(string name) // remove our book from the library
        {
            using (var span = DiagnosticsConfig.ActivitySource.StartActivity("remove book from bookservice"))
            {
                var client = new HttpClient();

                span?.SetStartTime(DateTime.Now);

                var response = await client.DeleteAsync($"https://localhost:1002/DeleteBook/{name}");

                span?.AddTag("bookservice.response", response.ToString());

            }
        }
    }
}