using Librarian.Exceptions;
using Librarian.Model;
using Microsoft.AspNetCore.Mvc;
using Telemetry;

namespace Librarian.Controllers
{
    public class LibrarianController
    {
        private static List<Book> listOfBooks = new List<Book>()
        {
            new Book("book1","author1"), new Book("book2","author2"), new Book("book3","author3")
        };

        [HttpGet]
        [Route("Books")]
        public List<Book> GetListOfBooks()
        {
            var activity = Telemetry.OpenTelemetry.CreateActivitySource("Getting books from Library");
            using (var span = Telemetry.OpenTelemetry.StartSpanActivity(activity))
            {
                span?.AddTag("Result","Getting books");
                span?.SetStartTime(DateTime.Now);
                span?.SetEndTime(DateTime.Now);
            }
            return listOfBooks;
        }

        [HttpGet]
        [Route("Book/{name}")]
        public Book GetBook(string name)
        {
            var GettingBookActivity = Telemetry.OpenTelemetry.CreateActivitySource("Getting all the books");
            using (var span = Telemetry.OpenTelemetry.StartSpanActivity(GettingBookActivity))
            {
                span?.SetStartTime(DateTime.Now);
                span?.AddTag("Event","Getting a book");
            }

            Book myBook;
            try
            {
                myBook = listOfBooks.Single(book => book.bookName == name); // look for a book. If not found, an exception will be thrown
            }
            catch (Exception)
            {
                var activity = Telemetry.OpenTelemetry.CreateActivitySource("Could not find book!");
                using (var exceptionSpan = Telemetry.OpenTelemetry.StartSpanActivity(activity))
                {
                exceptionSpan?.AddTag("Time:",DateTime.Now.ToString());
                exceptionSpan?.AddTag("Status","Book not found in list!");
                exceptionSpan?.AddTag("Tried looking for book", name);
                }

                throw new BookNotFoundException();
            }
            
            return myBook;
        }
        [HttpDelete]
        [Route("Book/{name}")]
        public string RemoveBook(string name)
        {
            var GettingBookActivity = Telemetry.OpenTelemetry.CreateActivitySource("Getting all the books");
            using (var span = Telemetry.OpenTelemetry.StartSpanActivity(GettingBookActivity))
            {
                span?.SetStartTime(DateTime.Now);
            }
            
            try
            {
                listOfBooks.Remove(listOfBooks.Single(book => book.bookName == name));
            }
            catch (Exception)
            {
                var activity = Telemetry.OpenTelemetry.CreateActivitySource("Could not find book!");
                using (var exceptionSpan = Telemetry.OpenTelemetry.StartSpanActivity(activity))
                {
                exceptionSpan?.AddTag("Time:", DateTime.Now.ToString());
                exceptionSpan?.AddTag("Status:", "Book not found in list!");
                exceptionSpan?.AddTag("Tried looking for book:", name);
                }

                throw new BookNotFoundException();
            }

            return name + " has been removed from library!";
        }
    }
}
