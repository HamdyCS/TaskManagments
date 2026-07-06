using Application.Common.Emails;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Interfaces.Channels
{
    public interface IOtpEmailQueue : IBackgroundQueue<OtpEmailContent>;
   
}
