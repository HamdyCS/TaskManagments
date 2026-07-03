using Application.common.Emails;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Emails
{
    public class ConfirmationEmailContent : EmailContent
    {
        public required string FullName { get; set; }
        public required string Url { get; set; }
    }

}
