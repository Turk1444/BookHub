using System;
using System.Collections.Generic;
using System.Text;

namespace BookHub.Domain.Entities
{
    public class EBook : LibraryItem
    {
        public double FileSizeMb { get; set; }
        public string Format { get; set; } = "PDF";

        public EBook() { }
        public EBook(int id, string title, string genre, double fileSizeMb, string format = "PDF")
        {
            Id = id; Title = title; Genre = genre;
            TotalCopies = 999; AvailableCopies = 999; 
            FileSizeMb = fileSizeMb; Format = format;
        }
    }
}
