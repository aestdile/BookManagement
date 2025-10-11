using BookManagement.Application.DTOs.Auth;
using FluentValidation;

namespace BookManagement.Application.Validators;

public class LoginDtoValidator : AbstractValidator<LoginDto>
{
    public LoginDtoValidator()
    {
        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Telefon raqam kiritilishi shart")
            .Matches(@"^\+998[0-9]{9}$")
            .WithMessage("Telefon raqam +998XXXXXXXXX formatida bo'lishi kerak");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Parol kiritilishi shart");
    }
}