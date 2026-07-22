using BookHub.Application.Services;
using System.Collections.Generic;
using BookHub.Domain.Entities;
using System;
using BookHub.DataAccess.Models;

namespace BookHub.UI
{
    internal static class Program
    {
        private const string BooksFilePath = "books_data.json";
        private const string UsersFilePath = "users_data.json";

        static void Main(string[] args)
        {
            LoggerService logger = new LoggerService();
            logger.Log("INFO", "BookHub application starting up.");

            List<Book> books = DataStorage.LoadData<Book>(BooksFilePath);
            if (books.Count == 0)
            {
                books.Add(new Book(1, "The Hobbit", "Fantasy", 3, "J.R.R. Tolkien"));
                books.Add(new Book(2, "Clean Code", "Tech", 2, "Robert C. Martin"));
                DataStorage.SaveData(BooksFilePath, books);
            }

            List<User> users = DataStorage.LoadData<User>(UsersFilePath);
            AuthService authService = new AuthService(users);

            if (users.Count == 0)
            {
                authService.Register("System Admin", "admin@bookhub.ge", "AdminPass123!", UserRole.Admin);
                authService.Register("John Doe", "user@bookhub.ge", "UserPass123!", UserRole.Customer);
                DataStorage.SaveData(UsersFilePath, users);
            }

            LibraryService libraryService = new LibraryService(books);

            User? loggedInUser = null;
            while (loggedInUser == null)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("=================================");
                Console.WriteLine("    BOOKHUB SYSTEM - LOGIN      ");
                Console.WriteLine("=================================");
                Console.ResetColor();

                Console.Write("Email: ");
                string email = Console.ReadLine()?.Trim() ?? "";

                Console.Write("Password: ");
                string password = Console.ReadLine() ?? "";

                if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nError: Email and password cannot be empty.");
                    Console.ResetColor();
                    Console.WriteLine("Press Enter to try again...");
                    Console.ReadLine();
                    continue;
                }

                loggedInUser = authService.Login(email, password);

                if (loggedInUser == null)
                {
                    logger.Log("WARN", $"Failed login attempt for: {email}");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nInvalid credentials! Press Enter to try again.");
                    Console.ResetColor();
                    Console.ReadLine();
                }
            }

            logger.Log("INFO", $"User {loggedInUser.Email} ({loggedInUser.Role}) logged in.");

            bool running = true;
            while (running)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("=================================================");
                Console.WriteLine($"  BOOKHUB - Welcome, {loggedInUser.FullName} [{loggedInUser.Role}]");
                Console.WriteLine("=================================================");
                Console.ResetColor();

                Console.WriteLine("1. View All Authors");
                Console.WriteLine("2. Search Books by Genre");
                Console.WriteLine("3. Borrow a Book");
                Console.WriteLine("4. Return a Book");

                if (loggedInUser.Role == UserRole.Admin)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("5. [ADMIN] Add New Book");
                    Console.ResetColor();
                }

                Console.WriteLine("6. Exit & Save");
                Console.Write("\nSelect option: ");

                string choice = Console.ReadLine()?.Trim() ?? "";

                try
                {
                    switch (choice)
                    {
                        case "1":
                            Console.WriteLine("\n--- Authors in Library ---");
                            foreach (var author in libraryService.GetAllAuthors())
                            {
                                Console.WriteLine($" • {author}");
                            }
                            break;

                        case "2":
                            Console.Write("\nEnter Genre: ");
                            string genre = Console.ReadLine()?.Trim() ?? "";

                            if (string.IsNullOrWhiteSpace(genre))
                            {
                                Console.WriteLine("Genre search cannot be empty.");
                                break;
                            }

                            var results = libraryService.FilterByGenre(genre);
                            Console.WriteLine($"\nFound {results.Count} item(s):");
                            foreach (var b in results)
                            {
                                Console.WriteLine($" [{b.Id}] {b.Title} by {b.Author} ({b.AvailableCopies}/{b.TotalCopies} available)");
                            }
                            break;

                        case "3":
                            Console.Write("\nEnter Book ID to borrow: ");
                            if (int.TryParse(Console.ReadLine()?.Trim(), out int borrowId))
                            {
                                var bookToBorrow = libraryService.FindById(borrowId);
                                if (bookToBorrow != null)
                                {
                                    if (bookToBorrow.BorrowItem())
                                    {
                                        Console.ForegroundColor = ConsoleColor.Green;
                                        Console.WriteLine($"Successfully borrowed '{bookToBorrow.Title}'.");
                                        logger.Log("INFO", $"{loggedInUser.Email} borrowed book ID {borrowId}");
                                    }
                                    else
                                    {
                                        Console.ForegroundColor = ConsoleColor.Yellow;
                                        Console.WriteLine("No copies available.");
                                    }
                                }
                                else
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("Book ID not found.");
                                }
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Invalid ID number format.");
                            }
                            Console.ResetColor();
                            break;

                        case "4":
                            Console.Write("\nEnter Book ID to return: ");
                            if (int.TryParse(Console.ReadLine()?.Trim(), out int returnId))
                            {
                                var bookToReturn = libraryService.FindById(returnId);
                                if (bookToReturn != null)
                                {
                                    bookToReturn.ReturnItem();
                                    Console.ForegroundColor = ConsoleColor.Green;
                                    Console.WriteLine($"Successfully returned '{bookToReturn.Title}'.");
                                    logger.Log("INFO", $"{loggedInUser.Email} returned book ID {returnId}");
                                }
                                else
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("Book ID not found.");
                                }
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Invalid ID number format.");
                            }
                            Console.ResetColor();
                            break;

                        case "5":
                            if (loggedInUser.Role != UserRole.Admin)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Access Denied: Admin privileges required.");
                                Console.ResetColor();
                                break;
                            }

                            Console.Write("Enter Title: ");
                            string title = Console.ReadLine()?.Trim() ?? "";
                            Console.Write("Enter Author: ");
                            string authorName = Console.ReadLine()?.Trim() ?? "";
                            Console.Write("Enter Genre: ");
                            string bookGenre = Console.ReadLine()?.Trim() ?? "";
                            Console.Write("Enter Quantity: ");
                            int.TryParse(Console.ReadLine()?.Trim(), out int qty);

                            if (!string.IsNullOrEmpty(title) && qty > 0)
                            {
                                books.Add(new Book(books.Count + 1, title, bookGenre, qty, authorName));
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine($"Book '{title}' added successfully.");
                                logger.Log("INFO", $"Admin added book '{title}'");
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Invalid input. Book creation canceled.");
                            }
                            Console.ResetColor();
                            break;

                        case "6":
                            running = false;
                            DataStorage.SaveData(BooksFilePath, books);
                            DataStorage.SaveData(UsersFilePath, users);
                            logger.Log("INFO", "Data saved. Application exited cleanly.");

                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("\nAll changes saved. Goodbye!");
                            Console.ResetColor();
                            break;

                        default:
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Invalid selection. Try again.");
                            Console.ResetColor();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    logger.Log("ERROR", $"Execution error: {ex.Message}");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"System Error: {ex.Message}");
                    Console.ResetColor();
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
