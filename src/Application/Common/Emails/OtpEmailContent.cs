using Application.common.Emails;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Emails
{
    public class OtpEmailContent : EmailContent
    {
        public required string OtpCode { get; set; }

        public required string OtpType { get; set; }

        public required string FullName { get; set; }

        public required int Valid_Minutes { get; set; }
    }
}
