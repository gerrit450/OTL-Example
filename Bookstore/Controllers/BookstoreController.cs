using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
using System.Xml.Linq;

namespace BookStore.Controller
{

    public class BookStoreController
    {
        [HttpGet]
        [Route("Books")]
        public async Task<string> GetAllBooks()
        {
            var telemetryActivity = Telemetry.Telemetry.ActivitySource.StartActivity("Getting all the books!");
            telemetryActivity?.SetStartTime(DateTime.Now);

            var client = new HttpClient();
            var responseString = await client.GetStringAsync($"https://localhost:1002/GetAllBooks/");

            telemetryActivity?.SetTag("Books found:", "3");
            telemetryActivity?.SetTag("Time:", DateTime.Now.ToString());

            telemetryActivity?.Stop();

            return responseString;
        }

        [HttpGet]
        [Route("Book/{name}")]
        public async Task<string> GetBook(string name) // get a book!
        {
            var telemetryActivity = Telemetry.Telemetry.ActivitySource.StartActivity("Getting a book!");
            telemetryActivity?.SetStartTime(DateTime.Now);

            var client = new HttpClient();
            var responseString = await client.GetStringAsync($"https://localhost:1002/GetBook/{name}");

            telemetryActivity?.SetEndTime(DateTime.Now);
            telemetryActivity?.AddTag("book found:", responseString);

            telemetryActivity?.Stop();

            return responseString;
        }

        [HttpDelete]
        [Route("Book/{name}")]
        public async Task<string> RemoveBook(string name) // remove our book from the library
        {
            var telemetryActivity = Telemetry.Telemetry.ActivitySource.StartActivity("Getting a book!");
            telemetryActivity?.SetStartTime(DateTime.Now);

            var client = new HttpClient();
            var responseString = await client.DeleteAsync($"https://localhost:1002/DeleteBook/{name}");

            telemetryActivity?.Stop();

            return responseString.RequestMessage.ToString();
        }
    }
}