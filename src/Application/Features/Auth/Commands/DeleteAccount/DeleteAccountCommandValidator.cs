using FluentValidation;

namespace Application.Features.Auth.Commands.DeleteAccount
{
    public class DeleteAccountCommandValidator : AbstractValidator<DeleteAccountCommand>
    {
        public DeleteAccountCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId is required");

            RuleFor(x => x.DeleteAccountDto.Token)
                .NotEmpty().WithMessage("Token is required");
        }
    }
}
