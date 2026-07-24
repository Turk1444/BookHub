using System;
using System.Collections.Generic;
using System.Text;

namespace BookHub.Domain.Entities
{
    public class LibraryItem
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
        public int TotalCopies { get; set; }
        public int AvailableCopies { get; set; }
        public int BorrowCount { get; set; } = 0;

        public virtual bool BorrowItem()
        {
            if (AvailableCopies > 0)
            {
                AvailableCopies--;
                BorrowCount++;
                return true;
            }
            return false;
        }

        public virtual void ReturnItem()
        {
            if (AvailableCopies < TotalCopies)
                AvailableCopies++;
        }
    }    
}
