using Microsoft.AspNetCore.Mvc;
using BookService.OpenTelemetry;

namespace BookService.Controllers
{
    public class BookServiceController
    {
        [HttpGet]
        [Route("GetBook/{name}")]
        public async Task<string> GetBookFromLibrarian(string name)
        {
            using (var spanActivity = Telemetry.ActivitySource.StartActivity("Asking librarion for book: " + name))
            {
                var client = new HttpClient();
                var responseString = await client.GetStringAsync($"https://localhost:1003/Book/{name}");
                spanActivity?.AddTag("Getting book", name);
                
                return responseString;
            }

        }
        [HttpGet]
        [Route("GetAllBooks")]
        public async Task<string> GetListOfBooksFromLibrarian(string name)
        {
            using (var span = Telemetry.ActivitySource.StartActivity("Asking librarian for the books!"))
            {
            var client = new HttpClient();
            var responseString = await client.GetStringAsync($"https://localhost:1003/Books");
            span?.AddTag("response", responseString);

             return responseString;
            }
        }

        [HttpDelete]
        [Route("DeleteBook/{name}")]
        public async Task<string> AskLibrarianToRemoveBook(string name)
        {
            using (var span = Telemetry.ActivitySource.StartActivity("Asking librarian to remove a book!"))
            {
                var client = new HttpClient();
                var responseString = await client.DeleteAsync($"https://localhost:1003/Book/{name}");
                span?.AddTag("deleting book",name);

                return responseString.RequestMessage.ToString();
            }
        }
    }
}