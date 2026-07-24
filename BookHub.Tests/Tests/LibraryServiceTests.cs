using BookHub.Application.Services;
using BookHub.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookHub.Tests.Tests
{
    public class LibraryServiceTests
    {
        [Fact]
        public void SearchByAuthorOrGenre_ShouldReturnMatches_UsingLINQ()
        {
            var items = new List<LibraryItem>
            {
                new Book(1, "Test Book", "Sci-Fi", 5, "Test Author"),
                new Magazine(2, "Test Mag", "Science", 5, 100)
            };
            var service = new LibraryService(items, new List<BorrowRecord>());

            var results = service.SearchByAuthorOrGenre("Sci-Fi");

            Assert.Single(results);
            Assert.Equal("Test Book", results[0].Title);
        }
    }
}
