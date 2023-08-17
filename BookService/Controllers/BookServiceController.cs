using Microsoft.AspNetCore.Mvc;

namespace BookService.Controllers
{
    public class BookServiceController
    {
        [HttpGet]
        [Route("GetBook/{name}")]
        public async Task<string> GetBookFromLibrarian(string name)
        {
            var activity = Telemetry.OpenTelemetry.CreateActivitySource("Getting books from book service");
            using (var spanActivity = Telemetry.OpenTelemetry.StartSpanActivity(activity))
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
            var activity = Telemetry.OpenTelemetry.CreateActivitySource("Getting all the books");
            using (var span = Telemetry.OpenTelemetry.StartSpanActivity(activity))
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
            var activity = Telemetry.OpenTelemetry.CreateActivitySource("Deleting a book from service");
            using (var span = Telemetry.OpenTelemetry.StartSpanActivity(activity))
            {
                var client = new HttpClient();
                var responseString = await client.DeleteAsync($"https://localhost:1003/Book/{name}");

                span?.AddTag("deleting book",name);

                return responseString.RequestMessage.ToString();
            }
        }
    }
}