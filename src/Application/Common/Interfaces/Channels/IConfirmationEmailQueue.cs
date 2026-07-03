
using Application.Common.Emails;

namespace Application.Common.Interfaces.Channels
{
    public interface IConfirmationEmailQueue : IBackgroundQueue<ConfirmationEmailContent>;

}
