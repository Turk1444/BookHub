using BookHub.Application.Services;
using BookHub.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookHub.Tests.Tests
{
    public class AuthServiceTests
    {
        [Fact]
        public void Register_ShouldCreateNewUser_WhenEmailIsUnique()
        {
            var users = new List<User>();
            var authService = new AuthService(users);

            var result = authService.Register("Jane Doe", "jane@test.com", "Password123!", UserRole.Customer);

            Assert.NotNull(result);
            Assert.Equal("jane@test.com", result.Email);
            Assert.Single(users);
        }

        [Fact]
        public void Login_ShouldReturnNull_WhenCredentialsAreInvalid()
        {
            var users = new List<User>();
            var authService = new AuthService(users);
            authService.Register("Jane Doe", "jane@test.com", "Password123!", UserRole.Customer);

            var result = authService.Login("jane@test.com", "WrongPassword");

            Assert.Null(result);
        }
    }
}
