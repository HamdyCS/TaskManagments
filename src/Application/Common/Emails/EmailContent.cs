using System;
using System.Collections.Generic;
using System.Text;

namespace Application.common.Emails
{
    public class EmailContent
    {
        public required string Subject { get; set; }

        public required string Messsage { get; set; }

        public required string TextBody { get; set; }

        public required string To { get; set; }
    }

}
