using BookHub.Application.Services;
using BookHub.Domain.Entities;
using System;
using BookHub.DataAccess.Models;
using System.Collections.Generic;
using BookHub.DataAccess;
using EmailService = BookHub.Application.Services.EmailService;



//customer:
//
// Email: user@bookhub.ge
// Password: User123!
//
// Admin:
//
// Email: admin@bookhub.ge
// Password : Admin123!
namespace BookHub.UI
{
    class Program
    {
        private const string ItemsFilePath = "items_data.json";
        private const string UsersFilePath = "users_data.json";
        private const string RecordsFilePath = "records_data.json";

        static void Main(string[] args)
        {
            LoggerService logger = new LoggerService();
            EmailService emailService = new EmailService();


            List<LibraryItem> items = DataStorage.LoadData<LibraryItem>(ItemsFilePath);
            if (items.Count == 0)
            {
                items.AddRange(new List<LibraryItem>
                {
                    //Books
                    new Book(1, "The Hobbit", "Fantasy", 3, "J.R.R. Tolkien"),
                    new Book(2, "Clean Code", "Tech", 2, "Robert C. Martin"),
                    new Book(3, "Design Patterns", "Tech", 4, "Erich Gamma"),
                    new Book(4, "1984", "Dystopian", 5, "George Orwell"),
                    new Book(5, "To Kill a Mockingbird", "Classic", 2, "Harper Lee"),
                    new Book(6, "The Pragmatic Programmer", "Tech", 3, "Andrew Hunt"),
                    new Book(7, "Dune", "Sci-Fi", 4, "Frank Herbert"),
                    new Book(8, "Foundation", "Sci-Fi", 3, "Isaac Asimov"),
                    new Book(9, "Sapiens", "History", 5, "Yuval Noah Harari"),
                    new Book(10, "Atomic Habits", "Self-Help", 6, "James Clear"),
                    
                    // Magazines
                    new Magazine(11, "National Geographic", "Science", 5, 202407),
                    new Magazine(12, "TIME Magazine", "News", 3, 1042),
                    new Magazine(13, "Wired", "Tech", 4, 301),
                    new Magazine(14, "Forbes", "Business", 2, 88),
                    new Magazine(15, "The Economist", "Finance", 3, 9500),
                    new Magazine(16, "Scientific American", "Science", 4, 412),
                    new Magazine(17, "Harvard Business Review", "Business", 2, 112),

                    // EBooks
                    new EBook(18, "C# 12 In a Nutshell", "Tech", 12.5, "PDF"),
                    new EBook(19, "Pro ASP.NET Core", "Tech", 15.2, "EPUB"),
                    new EBook(20, "Domain-Driven Design", "Tech", 8.4, "PDF"),
                    new EBook(21, "The Martian", "Sci-Fi", 3.2, "EPUB"),
                    new EBook(22, "Steve Jobs Biography", "Biography", 10.1, "PDF"),
                    new EBook(23, "Clean Architecture", "Tech", 7.5, "PDF"),
                    new EBook(24, "Microservices Patterns", "Tech", 9.8, "EPUB"),
                    new EBook(25, "Thinking in Systems", "Education", 4.5, "PDF")
                });
                DataStorage.SaveData(ItemsFilePath, items);
            }

            List<User> users = DataStorage.LoadData<User>(UsersFilePath);
            List<BorrowRecord> records = DataStorage.LoadData<BorrowRecord>(RecordsFilePath);

           
            if (records.Count == 0)
            {
                records.Add(new BorrowRecord
                {
                    Id = 1,
                    ItemId = 1,
                    ItemTitle = "The Hobbit",
                    UserEmail = "user@bookhub.ge",
                    BorrowDate = DateTime.Now.AddDays(-20),
                    DueDate = DateTime.Now.AddDays(-6), 
                    IsReturned = false
                });
                DataStorage.SaveData(RecordsFilePath, records);
            }

            AuthService authService = new AuthService(users);
            if (users.Count == 0)
            {
                authService.Register("System Admin", "admin@bookhub.ge", "Admin123!", UserRole.Admin);
                authService.Register("John Doe", "user@bookhub.ge", "User123!", UserRole.Customer);
                authService.Register("Saba", "saba@gmail.com", "123", UserRole.Customer);
                DataStorage.SaveData(UsersFilePath, users);
            }

            LibraryService libraryService = new LibraryService(items, records);

            User? loggedInUser = null;
            while (loggedInUser == null)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("=========================================");
                Console.WriteLine("     BOOKHUB MANAGEMENT SYSTEM           ");
                Console.WriteLine("=========================================");
                Console.ResetColor();

                Console.Write("Email: ");
                string email = Console.ReadLine()?.Trim() ?? "";
                Console.Write("Password: ");
                string password = Console.ReadLine() ?? "";

                loggedInUser = authService.Login(email, password);
                if (loggedInUser == null)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nInvalid credentials! Press Enter to try again.");
                    Console.ResetColor();
                    Console.ReadLine();
                }
            }

            bool running = true;
            while (running)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("╔═══════════════════════════════════════════════╗");
                Console.WriteLine("║          BOOKHUB LIBRARY MANAGEMENT           ║");
                Console.WriteLine("╚═══════════════════════════════════════════════╝");
                Console.ResetColor();

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"[USER] Welcome, {loggedInUser.FullName}");
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine($"[ROLE] {loggedInUser.Role}");
                Console.ResetColor();
                Console.WriteLine();

                // Menu options with colors
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("1. ");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Browse All Items (10 Items Catalog)");

                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("2. ");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("LINQ Search (Author / Genre)");

                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("3. ");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("LINQ Query: Top 5 Borrowed Items");

                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("4. ");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Check My Overdue Items & Fees");

                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("5. ");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Borrow an Item");

                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("6. ");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Return a Borrowed Item");

                if (loggedInUser.Role == UserRole.Admin)
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("7. ");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("[ADMIN] LINQ Overdue List & Send SMTP Reminders");
                    Console.ResetColor();
                }

                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("8. ");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Exit & Save");

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("\n[OPTION] Select option: ");
                Console.ResetColor();
                string choice = Console.ReadLine()?.Trim() ?? "";

                switch (choice)
                {
                    case "1":
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine("\n--- CATALOG ---");
                        Console.ResetColor();
                        foreach (var item in libraryService.GetAllItems())
                        {
                            string detail = item is Book b ? $"By {b.Author}" : (item is Magazine m ? $"Issue #{m.IssueNumber}" : "[E-BOOK] Digital");
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write($"[{item.Id}]");
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Write($" [{item.GetType().Name}] ");
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.Write($"{item.Title} ");
                            Console.ForegroundColor = ConsoleColor.Magenta;
                            Console.Write($"- {item.Genre} ");
                            Console.ForegroundColor = ConsoleColor.Gray;
                            Console.Write($"({detail}) ");
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine($"- Available: {item.AvailableCopies}");
                        }
                        Console.ResetColor();
                        break;

                    case "2":
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.Write("\nEnter Genre or Author keyword: ");
                        Console.ResetColor();
                        string keyword = Console.ReadLine()?.Trim() ?? "";
                        var searchResults = libraryService.SearchByAuthorOrGenre(keyword);
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"\n[LINQ] Found {searchResults.Count} matching item(s):");
                        Console.ResetColor();
                        foreach (var res in searchResults)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write("  > ");
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Write($"{res.Title} ");
                            Console.ForegroundColor = ConsoleColor.Magenta;
                            Console.WriteLine($"({res.Genre})");
                        }
                        Console.ResetColor();
                        break;

                    case "3":
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("\n[LINQ] Top Most Borrowed Items:");
                        Console.ResetColor();
                        foreach (var top in libraryService.GetTopBooks(5))
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write("  * ");
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Write($"{top.Title} - ");
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine($"Borrowed {top.BorrowCount} time(s)");
                        }
                        Console.ResetColor();
                        break;

                    case "4":
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine("\n--- YOUR OVERDUE RECORDS & FEES ---");
                        Console.ResetColor();
                        var myOverdues = records.Where(r => r.UserEmail == loggedInUser.Email && !r.IsReturned).ToList();
                        if (!myOverdues.Any())
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("[OK] You have no overdue items!");
                            Console.ResetColor();
                        }
                        else
                        {
                            foreach (var rec in myOverdues)
                            {
                                decimal fee = rec.CalculateOverdueFee();
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.Write("[ALERT] Item: ");
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.Write($"{rec.ItemTitle} | ");
                                Console.ForegroundColor = ConsoleColor.Magenta;
                                Console.Write($"Due: {rec.DueDate:yyyy-MM-dd} | ");
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine($"Fee: ${fee:F2}");
                            }
                        }
                        Console.ResetColor();
                        break;

                    case "5":
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine("\n--- BORROW AN ITEM ---");
                        Console.ResetColor();
                        Console.WriteLine("First, view available items:");
                        foreach (var item in libraryService.GetAllItems().Where(i => i.AvailableCopies > 0))
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write($"[{item.Id}]");
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.Write($" {item.Title} ");
                            Console.ForegroundColor = ConsoleColor.Gray;
                            Console.WriteLine($"({item.AvailableCopies} available)");
                        }
                        Console.ResetColor();

                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.Write("\nEnter Item ID to borrow: ");
                        Console.ResetColor();

                        if (int.TryParse(Console.ReadLine()?.Trim(), out int borrowId))
                        {
                            var itemToBorrow = libraryService.FindById(borrowId);
                            if (itemToBorrow != null)
                            {
                                if (libraryService.BorrowItem(borrowId, loggedInUser.Email, itemToBorrow.Title))
                                {
                                    Console.ForegroundColor = ConsoleColor.Green;
                                    Console.WriteLine($"[SUCCESS] You borrowed '{itemToBorrow.Title}' successfully!");
                                    Console.WriteLine($"[INFO] Due Date: {DateTime.Now.AddDays(14):yyyy-MM-dd}");
                                    logger.Log("INFO", $"{loggedInUser.Email} borrowed item ID {borrowId} - {itemToBorrow.Title}");
                                }
                                else
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("[ERROR] No copies available or invalid item.");
                                }
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("[ERROR] Item not found.");
                            }
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("[ERROR] Invalid ID format.");
                        }
                        Console.ResetColor();
                        break;

                    case "6":
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine("\n--- RETURN A BORROWED ITEM ---");
                        Console.ResetColor();
                        var myBorrowedItems = libraryService.GetUserBorrowedItems(loggedInUser.Email);

                        if (!myBorrowedItems.Any())
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("[INFO] You have no borrowed items.");
                            Console.ResetColor();
                        }
                        else
                        {
                            Console.WriteLine("Your borrowed items:");
                            foreach (var borrowed in myBorrowedItems)
                            {
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.Write($"[{borrowed.ItemId}] ");
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.Write($"{borrowed.ItemTitle} - ");
                                Console.ForegroundColor = ConsoleColor.Magenta;
                                Console.WriteLine($"Due: {borrowed.DueDate:yyyy-MM-dd}");
                            }
                            Console.ResetColor();

                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.Write("\nEnter Item ID to return: ");
                            Console.ResetColor();

                            if (int.TryParse(Console.ReadLine()?.Trim(), out int returnId))
                            {
                                if (libraryService.ReturnItem(returnId, loggedInUser.Email))
                                {
                                    var returnedItem = libraryService.FindById(returnId);
                                    Console.ForegroundColor = ConsoleColor.Green;
                                    Console.WriteLine($"[SUCCESS] You returned '{returnedItem?.Title}' successfully!");
                                    logger.Log("INFO", $"{loggedInUser.Email} returned item ID {returnId}");
                                }
                                else
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("[ERROR] Could not return item. Not found or already returned.");
                                }
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("[ERROR] Invalid ID format.");
                            }
                            Console.ResetColor();
                        }
                        break;

                    case "7":
                        if (loggedInUser.Role != UserRole.Admin) 
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("[ERROR] Access Denied: Admin privileges required.");
                            Console.ResetColor();
                            break;
                        }

                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\n[ADMIN] Overdue Items via LINQ & SMTP Notifications:");
                        Console.ResetColor();
                        var overdues = libraryService.GetOverdueRecords();
                        if (overdues.Count == 0)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("[OK] No overdue items found!");
                            Console.ResetColor();
                        }
                        else
                        {
                            foreach (var o in overdues)
                            {
                                decimal fee = o.CalculateOverdueFee();
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.Write($"[OVERDUE] ");
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.Write($"{o.UserEmail} - ");
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.Write($"Item: '{o.ItemTitle}' | ");
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine($"Fee: ${fee:F2}");
                                Console.ForegroundColor = ConsoleColor.Cyan;
                                Console.WriteLine("   [SENDING] Email reminder...");
                                Console.ResetColor();
                                emailService.SendOverdueReminder(o.UserEmail, "Valued Member", o.ItemTitle, fee);
                            }
                        }
                        break;

                    case "8":
                        running = false;
                        DataStorage.SaveData(ItemsFilePath, items);
                        DataStorage.SaveData(UsersFilePath, users);
                        DataStorage.SaveData(RecordsFilePath, records);
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("\n[OK] Data saved safely.");
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("[EXIT] Goodbye!");
                        Console.ResetColor();
                        break;

                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\n[ERROR] Invalid option. Please try again.");
                        Console.ResetColor();
                        break;
                }

                if (running)
                {
                    Console.WriteLine("\nPress Enter to return to menu...");
                    Console.ReadLine();
                }
            }
        }
    }
}