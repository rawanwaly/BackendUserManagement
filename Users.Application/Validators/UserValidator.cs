using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Domain.Core.Models;

namespace UserManagement.Application.Validators
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        { 
            RuleFor(x => x.FirstNameEN)
                .NotEmpty().WithMessage("First Name is required")
                .MaximumLength(20).WithMessage("First Name must not exceed 20 characters");

            RuleFor(x => x.LastNameEN)
                .NotEmpty().WithMessage("Last Name is required")
                .MaximumLength(20).WithMessage("Last Name must not exceed 20 characters");
            RuleFor(x => x.FirstNameAR)
                .NotEmpty().WithMessage("First Name is required")
                .MaximumLength(20).WithMessage("First Name must not exceed 20 characters");

            RuleFor(x => x.LastNameAR)
                .NotEmpty().WithMessage("Last Name is required")
                .MaximumLength(20).WithMessage("Last Name must not exceed 20 characters");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email format");

            RuleFor(x => x.MobileNumber)
                .NotEmpty().WithMessage("Mobile number is required")
                .Matches(@"^05\d{8}$").WithMessage("Mobile number must start with 05 and be 10 digits");

            RuleFor(x => x.MaritalStatus)
                .IsInEnum().WithMessage("Invalid marital status");

            RuleFor(x => x.Address)
                .MaximumLength(200).WithMessage("Address must not exceed 200 characters")
                .When(x => !string.IsNullOrEmpty(x.Address));
        }
    }
}
