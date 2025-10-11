using BookManagement.Application.DTOs.Auth;
using FluentValidation;

namespace BookManagement.Application.Validators;

public class RegisterDtoValidator : AbstractValidator<RegisterDto>
{
    public RegisterDtoValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("Ism kiritilishi shart")
            .MaximumLength(100).WithMessage("Ism 100 belgidan oshmasligi kerak");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Familiya kiritilishi shart")
            .MaximumLength(100).WithMessage("Familiya 100 belgidan oshmasligi kerak");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Telefon raqam kiritilishi shart")
            .Matches(@"^\+998[0-9]{9}$")
            .WithMessage("Telefon raqam +998XXXXXXXXX formatida bo'lishi kerak");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Parol kiritilishi shart")
            .MinimumLength(6).WithMessage("Parol kamida 6 belgidan iborat bo'lishi kerak")
            .Matches(@"[A-Z]").WithMessage("Parolda kamida bitta katta harf bo'lishi kerak")
            .Matches(@"[a-z]").WithMessage("Parolda kamida bitta kichik harf bo'lishi kerak")
            .Matches(@"[0-9]").WithMessage("Parolda kamida bitta raqam bo'lishi kerak");

        RuleFor(x => x.ConfirmPassword)
            .Equal(x => x.Password).WithMessage("Parollar mos kelmadi");
    }
}
