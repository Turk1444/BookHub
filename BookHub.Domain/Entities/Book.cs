using System;
using System.Collections.Generic;
using System.Text;

namespace BookHub.Domain.Entities
{
    public class Book : LibraryItem
    {
        public string Author { get; set; } = string.Empty;

        public Book() { }
        public Book(int id, string title, string genre, int totalCopies, string author)
        {
            Id = id; Title = title; Genre = genre;
            TotalCopies = totalCopies; AvailableCopies = totalCopies;
            Author = author;
        }
    }
}
