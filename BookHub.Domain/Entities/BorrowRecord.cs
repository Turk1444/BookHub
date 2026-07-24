using System;
using System.Collections.Generic;
using System.Text;

namespace BookHub.Domain.Entities
{
    public class BorrowRecord
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public string ItemTitle { get; set; } = string.Empty;
        public string UserEmail { get; set; } = string.Empty;
        public DateTime BorrowDate { get; set; } = DateTime.Now;
        public DateTime DueDate { get; set; } = DateTime.Now.AddDays(14); 
        public DateTime? ReturnDate { get; set; }
        public decimal DailyFeeRate { get; set; } = 0.50m; 
        public bool IsReturned { get; set; } = false;

        public decimal CalculateOverdueFee()
        {
            DateTime checkDate = ReturnDate ?? DateTime.Now;
            if (checkDate > DueDate)
            {
                int daysOverdue = (int)Math.Ceiling((checkDate - DueDate).TotalDays);
                return daysOverdue * DailyFeeRate;
            }
            return 0.00m;
        }
    }
}
