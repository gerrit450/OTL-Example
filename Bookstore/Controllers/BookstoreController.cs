using Microsoft.AspNetCore.Mvc;
using Telemetry;

namespace BookStore.Controller
{

    public class BookStoreController
    {
        [HttpGet]
        [Route("Books")]
        public async Task<string> GetAllBooks()
        {
            //var activity = Telemetry.OpenTelemetry.CreateActivitySource("Getting books from book service");
            //using (var spanActivity = Telemetry.OpenTelemetry.StartSpanActivity(activity))
            //{
                //spanActivity?.SetStartTime(DateTime.Now);
                //spanActivity?.SetTag("Books found:", "3");
                //spanActivity?.SetTag("Time:", DateTime.Now.ToString());

                var client = new HttpClient();
                var responseString = await client.GetStringAsync($"https://localhost:1002/GetAllBooks/");

                return responseString;
            //}
        }

        [HttpGet]
        [Route("Book/{name}")]
        public async Task<string> GetBook(string name) // get a book!
        {
            var activity = Telemetry.OpenTelemetry.CreateActivitySource(name);
            using (var spanActivity = Telemetry.OpenTelemetry.StartSpanActivity(activity))
            {
                var client = new HttpClient();
                var responseString = await client.GetStringAsync($"https://localhost:1002/GetBook/{name}");
                spanActivity?.SetStartTime(DateTime.Now);
                spanActivity?.AddTag("book found:", responseString);

                return responseString;
            }
        }

        [HttpDelete]
        [Route("Book/{name}")]
        public async Task<string> RemoveBook(string name) // remove our book from the library
        {
            var activity = Telemetry.OpenTelemetry.CreateActivitySource("test");
            using (var spanActivity = Telemetry.OpenTelemetry.StartSpanActivity(activity))
            {
                var client = new HttpClient();
                var responseString = await client.DeleteAsync($"https://localhost:1002/DeleteBook/{name}");
                spanActivity?.SetStartTime(DateTime.Now);

                return responseString.RequestMessage.ToString();
            }
        }
    }
}