using BookHub.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using BCrypt.Net;
using static BookHub.Domain.Entities.User;

namespace BookHub.Application.Services
{
    public class AuthService
    {
        private readonly List<User> _users;

        public AuthService(List<User> users)
        {
            _users = users;
        }

        public User? Register(string fullName, string email, string rawPassword, UserRole role)
        {
            if (_users.Any(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase)))
                return null;

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(rawPassword);

            var newUser = new User
            {
                Id = _users.Count + 1,
                FullName = fullName,
                Email = email,
                PasswordHash = hashedPassword,
                Role = role
            };

            _users.Add(newUser);
            return newUser;
        }

        public User? Login(string email, string rawPassword)
        {
            var user = _users.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
            if (user == null) return null;

            bool isValidPassword = BCrypt.Net.BCrypt.Verify(rawPassword, user.PasswordHash);
            return isValidPassword ? user : null;
        }
    }
}
