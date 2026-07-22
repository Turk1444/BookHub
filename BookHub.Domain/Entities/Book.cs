using System;
using System.Collections.Generic;
using System.Text;

namespace BookHub.Domain.Entities
{
    public class Book : LibraryItem
    {
        public string Author { get; set; }

        public Book(int id, string title, string genre, int totalCopies, string author)
            : base(id, title, genre, totalCopies)
        {
            Author = author;
        }
    }
}
