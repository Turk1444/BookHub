using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BookHub.Domain.Entities;


namespace BookHub.Application.Services
{
    public class LibraryService
    {
        private readonly List<LibraryItem> _items;
        private readonly List<BorrowRecord> _records;

        public LibraryService(List<LibraryItem> items, List<BorrowRecord> records)
        {
            _items = items;
            _records = records;
        }

        // REQUIRED LINQ QUERY 1: Search by Author or Genre
        public List<LibraryItem> SearchByAuthorOrGenre(string keyword)
        {
            return _items.Where(item =>
                item.Genre.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                (item is Book b && b.Author.Contains(keyword, StringComparison.OrdinalIgnoreCase))
            ).ToList();
        }

        // REQUIRED LINQ QUERY 2: Overdue Items List
        public List<BorrowRecord> GetOverdueRecords()
        {
            return _records
                .Where(r => !r.IsReturned && DateTime.Now > r.DueDate)
                .ToList();
        }

        // REQUIRED LINQ QUERY 3: Top Borrowed Items
        public List<LibraryItem> GetTopBooks(int count = 5)
        {
            return _items
                .OrderByDescending(i => i.BorrowCount)
                .Take(count)
                .ToList();
        }

        public LibraryItem? FindById(int id)
        {
            return _items.FirstOrDefault(i => i.Id == id);
        }

        public List<LibraryItem> GetAllItems() => _items;

        public IEnumerable<object> FilterByGenre(string genre)
        {
            throw new NotImplementedException();
        }

        // Borrow an item
        public bool BorrowItem(int itemId, string userEmail, string itemTitle)
        {
            var item = FindById(itemId);
            if (item == null || item.AvailableCopies <= 0)
                return false;

            item.BorrowItem();

            var record = new BorrowRecord
            {
                Id = _records.Count + 1,
                ItemId = itemId,
                ItemTitle = itemTitle,
                UserEmail = userEmail,
                BorrowDate = DateTime.Now,
                DueDate = DateTime.Now.AddDays(14),
                IsReturned = false
            };

            _records.Add(record);
            return true;
        }

        // Return a borrowed item
        public bool ReturnItem(int itemId, string userEmail)
        {
            var record = _records.FirstOrDefault(r => 
                r.ItemId == itemId && 
                r.UserEmail == userEmail && 
                !r.IsReturned);

            if (record == null)
                return false;

            record.IsReturned = true;
            record.ReturnDate = DateTime.Now;

            var item = FindById(itemId);
            if (item != null)
            {
                item.ReturnItem();
            }

            return true;
        }

        // Get user's borrowed items
        public List<BorrowRecord> GetUserBorrowedItems(string userEmail)
        {
            return _records
                .Where(r => r.UserEmail == userEmail && !r.IsReturned)
                .ToList();
        }
    }
}

