using BookManagement.Application.DTOs.Books;
using FluentValidation;

namespace BookManagement.Application.Validators;

public class CreateBookDtoValidator : AbstractValidator<CreateBookDto>
{
    public CreateBookDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Kitob nomi kiritilishi shart")
            .MaximumLength(200).WithMessage("Kitob nomi 100 belgidan oshmasligi kerak");

        RuleFor(x => x.Author)
            .NotEmpty().WithMessage("Muallif ismi kiritilishi shart")
            .MaximumLength(200).WithMessage("Muallif ismi 50 belgidan oshmasligi kerak");

        RuleFor(x => x.ISBN)
            .NotEmpty().WithMessage("ISBN kiritilishi shart")
            .Matches(@"^[0-9]{13}$").WithMessage("ISBN 13 raqamdan iborat bo'lishi kerak");

        RuleFor(x => x.Category)
            .NotEmpty().WithMessage("Kategoriya kiritilishi shart")
            .MaximumLength(100).WithMessage("Kategoriya 50 belgidan oshmasligi kerak");

        RuleFor(x => x.PublicationYear)
            .GreaterThan(1000).WithMessage("Nashr yili noto'g'ri")
            .LessThanOrEqualTo(DateTime.Now.Year).WithMessage("Nashr yili kelajakda bo'lishi mumkin emas");

        RuleFor(x => x.TotalCopies)
            .GreaterThan(0).WithMessage("Nusxalar soni kamida 1 ta bo'lishi kerak");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Tavsif 1000 belgidan oshmasligi kerak")
            .When(x => !string.IsNullOrEmpty(x.Description));
    }
}
