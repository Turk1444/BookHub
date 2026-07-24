using System;
using System.Collections.Generic;
using System.Text;

namespace BookHub.Domain.Entities
{
    public class Magazine : LibraryItem
    {
        public int IssueNumber { get; set; }

        public Magazine() { }
        public Magazine(int id, string title, string genre, int totalCopies, int issueNumber)
        {
            Id = id; Title = title; Genre = genre;
            TotalCopies = totalCopies; AvailableCopies = totalCopies;
            IssueNumber = issueNumber;
        }
    }
}
