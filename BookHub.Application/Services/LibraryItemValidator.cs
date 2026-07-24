using BookHub.Domain.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookHub.Application.Services
{
    public class LibraryItemValidator : AbstractValidator<LibraryItem>
    {
        public LibraryItemValidator()
        {
            RuleFor(item => item.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(100).WithMessage("Title cannot exceed 100 characters.");

            RuleFor(item => item.Genre)
                .NotEmpty().WithMessage("Genre is required.");

            RuleFor(item => item.TotalCopies)
                .GreaterThan(0).WithMessage("Total copies must be at least 1.");
        }
    }
}
