using ContactCore.DTOs;
using FluentValidation;

namespace ContactCore.Validators;

public class ContactValidator : AbstractValidator<ContactDTO>
{
    public ContactValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(100);

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Phone)
            .NotEmpty()
            .Matches(@"^\d{8,9}$")
            .WithMessage("Phone number must be 8 or 9 digits");

        RuleFor(x => x.DDD)
            .NotEmpty()
            .Matches(@"^\d{2}$")
            .WithMessage("DDD must be exactly 2 digits");
    }
}
