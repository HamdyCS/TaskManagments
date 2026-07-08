using Application.common.Emails;

namespace Application.Common.Emails
{
    public class ChangeEmailContent : EmailContent
    {
        public required string FullName { get; set; }
        public required string Url { get; set; }
    }
}
