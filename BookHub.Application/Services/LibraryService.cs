using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BookHub.Domain.Entities;


namespace BookHub.Application.Services
{
    public class LibraryService
    {
        private readonly List<Book> _books;

        public LibraryService(List<Book> books)
        {
            _books = books;
        }
        public List<Book> FilterByGenre(string genre) =>
            _books.Where(b => b.Genre.Equals(genre, StringComparison.OrdinalIgnoreCase))
                  .OrderBy(b => b.Title)
                  .ToList();

        public Book? FindById(int id) =>
            _books.FirstOrDefault(b => b.Id == id);

        public bool IsAvailable(string title) =>
            _books.Any(b => b.Title.Equals(title, StringComparison.OrdinalIgnoreCase) && b.AvailableCopies > 0);

        public List<string> GetAllAuthors() =>
            _books.Select(b => b.Author).Distinct().ToList();

        public int GetTotalInventoryCount() =>
            _books.Sum(b => b.TotalCopies);

        public List<Book> GetAllBooks() => _books;
    }
}
