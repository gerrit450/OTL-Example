using Microsoft.AspNetCore.Mvc;

namespace BookService.Controllers
{
    public class BookServiceController
    {
        [HttpGet]
        [Route("GetBook/{name}")]
        public async Task<string> GetBookFromLibrarian(string name)
        {
            var span = Telemetry.Telemetry.ActivitySource.StartActivity("Asking librarion for book: " + name);

            var client = new HttpClient();
            var responseString = await client.GetStringAsync($"https://localhost:1003/Book/{name}");
            span?.Stop();

            return responseString;
        }
        [HttpGet]
        [Route("GetAllBooks")]
        public async Task<string> GetListOfBooksFromLibrarian(string name)
        {
            var span = Telemetry.Telemetry.ActivitySource.StartActivity("Asking librarian for the books...!");
            var client = new HttpClient();
            var responseString = await client.GetStringAsync($"https://localhost:1003/Books");
            span?.Stop();
            return responseString;
        }

        [HttpDelete]
        [Route("DeleteBook/{name}")]
        public async Task<string> AskLibrarianToRemoveBook(string name)
        {
            var client = new HttpClient();
            var responseString = await client.DeleteAsync($"https://localhost:1003/Book/{name}");

            return responseString.RequestMessage.ToString();
        }
    }
}