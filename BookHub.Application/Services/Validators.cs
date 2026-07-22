using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using BookHub.Domain.Entities;


namespace BookHub.Application.Services
{
    public class Validators
    {
        public class UserValidator : AbstractValidator<User>
        {
            public UserValidator()
            {
                RuleFor(u => u.FullName)
                    .NotEmpty().WithMessage("Full Name is required.")
                    .MaximumLength(50).WithMessage("Full Name cannot exceed 50 characters.");

                RuleFor(u => u.Email)
                    .NotEmpty().WithMessage("Email is required.")
                    .EmailAddress().WithMessage("Must be a valid email address.");

                RuleFor(u => u.PasswordHash)
                    .NotEmpty().WithMessage("Password hash cannot be empty.");
            }
        }
    }
}
