using Application.Common.Emails;
using Application.Common.Interfaces.Channels;
using Infrastructure.common.channels;

namespace Infrastructure.Common.Channels
{
    public class DeleteAccountEmailQueue : BackgroundQueue<DeleteAccountEmailContent>, IDeleteAccountEmailQueue
    {
    }
}
