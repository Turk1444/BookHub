using BookHub.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookHub.Tests.Tests
{
    public class LibraryItemTests
    {
        [Fact]
        public void BorrowItem_WhenCopiesAvailable_ShouldDecreaseAvailableCopiesAndIncreaseBorrowCount()
        {
            
            var book = new Book(1, "Clean Code", "Tech", 2, "Robert C. Martin");

            bool result = book.BorrowItem();

            Assert.True(result);
            Assert.Equal(1, book.AvailableCopies);
            Assert.Equal(1, book.BorrowCount);
        }

        [Fact]
        public void BorrowItem_WhenNoCopiesAvailable_ShouldReturnFalse()
        {
            var book = new Book(1, "Clean Code", "Tech", 0, "Robert C. Martin");

            bool result = book.BorrowItem();

            Assert.False(result);
            Assert.Equal(0, book.AvailableCopies);
            Assert.Equal(0, book.BorrowCount);
        }

        [Fact]
        public void ReturnItem_ShouldIncreaseAvailableCopies()
        {
            var book = new Book(1, "Clean Code", "Tech", 2, "Robert C. Martin");
            book.BorrowItem(); 

            book.ReturnItem();

            Assert.Equal(2, book.AvailableCopies);
        }
    }
}
