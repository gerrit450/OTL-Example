
namespace Librarian.Exceptions
{
    public class BookNotFoundException : Exception
    {
        public BookNotFoundException() : base("Book not found!")
        {
        }
    }
}