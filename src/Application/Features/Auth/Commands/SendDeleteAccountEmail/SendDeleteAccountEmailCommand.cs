namespace Application.Features.Auth.Commands.SendDeleteAccountEmail
{
    public sealed record SendDeleteAccountEmailCommand(string UserId) : IRequest<ErrorOr<bool>>;
}
