using Application.Common.Emails;
using Application.Common.Interfaces.Channels;
using Infrastructure.common.channels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Common.Channels
{
    public class OtpEmailQueue : BackgroundQueue<OtpEmailContent>, IOtpEmailQueue
    {
    }
}
