using Application.common.Emails;

namespace Application.Common.Emails
{
    public class ResetPasswordEmailContent : EmailContent
    {
        public required string FullName { get; set; }
        public required string Url { get; set; }
    }
}
