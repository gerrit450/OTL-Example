using Librarian.Exceptions;
using Librarian.Model;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Metrics;

namespace Librarian.Controllers
{
    public class LibrarianController
    {
        private readonly Counter<long> _booksRemovedCounter = DiagnosticsConfig.LibraryMeter.CreateCounter<long>("removed");
        private readonly Counter<long> _booksRetrievedCounter = DiagnosticsConfig.LibraryMeter.CreateCounter<long>("viewed");

        [HttpGet]
        [Route("Books")]
        public List<Book> GetListOfBooks()
        {
            using (var span = DiagnosticsConfig.ActivitySource.StartActivity("Get library books"))
            {
                _booksRetrievedCounter.Add(Books.ListOfBooks.Count);
                return Books.ListOfBooks;
            }
        }

        [HttpGet]
        [Route("Book/{name}")]
        public Book GetBook(string name)
        {
            using (var span = DiagnosticsConfig.ActivitySource.StartActivity("view library book"))
            {
                // look for a book. If not found, an exception will be thrown
                Book myBook;
                try
                {
                    myBook = Books.ListOfBooks.Single(book => book.bookName == name);
                    _booksRetrievedCounter.Add(1);
                    span?.SetTag("book", myBook.bookName);
                }
                catch (Exception)
                {
                    span?.SetTag("book", name);
                    throw new BookNotFoundException();
                }

                return myBook;
            }
        }

        [HttpDelete]
        [Route("Book/{name}")]
        public string RemoveBook(string name)
        {
            Book bookToDelete;
            using (var span1 = DiagnosticsConfig.ActivitySource.StartActivity("Deleting book part 1"))
            {
                span1?.AddTag("book", name);
                try
                {
                    bookToDelete = Books.ListOfBooks.Single(book => book.bookName == name);
                }
                catch (Exception)
                {
                    throw new BookNotFoundException();
                }
            }

            using (var span2 = DiagnosticsConfig.ActivitySource.StartActivity("Deleting book part 2"))
            {
                span2?.AddTag("book", name); 
                
                Books.ListOfBooks.Remove(bookToDelete);
                _booksRemovedCounter.Add(1);
               
                return name + " has been removed from library!";
            }
        }

    }
}
