using FluentValidation;

namespace Application.Features.Auth.Commands.SendDeleteAccountEmail
{
    public class SendDeleteAccountEmailCommandValidator : AbstractValidator<SendDeleteAccountEmailCommand>
    {
        public SendDeleteAccountEmailCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId is required");
        }
    }
}
