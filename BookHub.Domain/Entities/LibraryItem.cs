using System;
using System.Collections.Generic;
using System.Text;

namespace BookHub.Domain.Entities
{
    public class LibraryItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Genre { get; set; }
        public int TotalCopies { get; private set; }
        public int AvailableCopies { get; private set; }

        protected LibraryItem(int id, string title, string genre, int totalCopies)
        {
            Id = id;
            Title = title;
            Genre = genre;
            TotalCopies = totalCopies;
            AvailableCopies = totalCopies;
        }

        public bool BorrowItem()
        {
            if (AvailableCopies <= 0) return false;
            AvailableCopies--;
            return true;
        }

        public void ReturnItem()
        {
            if (AvailableCopies < TotalCopies)
            {
                AvailableCopies++;
            }
        }   
    }    
}
