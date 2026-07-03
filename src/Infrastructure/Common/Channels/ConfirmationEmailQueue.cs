using Application.Common.Emails;
using Application.Common.Interfaces.Channels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.common.channels
{
    public class ConfirmationEmailQueue : BackgroundQueue<ConfirmationEmailContent>, IConfirmationEmailQueue;
   
}
