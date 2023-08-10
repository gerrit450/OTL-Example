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
        public List<Book> GetListOfBooks(string name)
        {
            return listOfBooks;
        }

        [HttpGet]
        [Route("Book/{name}")]
        public Book GetBook(string name)
        {
            Book myBook;
            try
            {
                myBook = listOfBooks.Single(book => book.bookName == name); // look for a book. If not found, an exception will be thrown
            }
            catch (Exception)
            {
                using (var exceptionSpan = Telemetry.OpenTelemetry.StartSpanActivity("Book not found!"))
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
            try
            {
                listOfBooks.Remove(listOfBooks.Single(book => book.bookName == name));
            }
            catch (Exception)
            {
                using (var exceptionSpan = Telemetry.OpenTelemetry.StartSpanActivity("Book not found!"))
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
