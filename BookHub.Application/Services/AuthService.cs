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

        public User? Login(string email, string password)
        {
            var user = _users.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
            if (user == null) return null;

            // BCrypt Verification
            bool isValidPassword = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
            return isValidPassword ? user : null;
        }

        public User Register(string fullName, string email, string plainPassword, UserRole role)
        {
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(plainPassword);

            var user = new User
            {
                Id = _users.Count + 1,
                FullName = fullName,
                Email = email,
                PasswordHash = hashedPassword,
                Role = role
            };

            _users.Add(user);
            return user;
        }
    }
}
