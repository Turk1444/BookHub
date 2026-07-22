using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using BookHub.Domain.Entities;

namespace BookHub.Domain.Entities
{
    public class LibraryItemTests
    {
        [Fact]
        public void BorrowItem_ShouldDecrementAvailableCopies_WhenCopiesExist()
        {
            var book = new Book(1, "Test Book", "Fiction", 3, "Test Author");
            bool result = book.BorrowItem();
            Assert.True(result);
            Assert.Equal(2, book.AvailableCopies);
        }

        [Fact]
        public void BorrowItem_ShouldFail_WhenNoCopiesAvailable()
        {
            var book = new Book(1, "Test Book", "Fiction", 0, "Test Author");
            bool result = book.BorrowItem();
            Assert.False(result);
            Assert.Equal(0, book.AvailableCopies);
        }
    }
}
