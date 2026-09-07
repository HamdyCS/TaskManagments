using Application.common.Emails;

namespace Application.Common.Emails
{
    public class DeleteAccountEmailContent : EmailContent
    {
        public required string FullName { get; set; }
        public required string Url { get; set; }
    }
}
