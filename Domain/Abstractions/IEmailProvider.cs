namespace Domain.Abstractions
{
    public interface IEmailProvider
    {
        string SendTextEmail(string receiverEmail, string receiverName, string subject, string message);
        string SendHtmlEmail(string receiverEmail, string receiverName, string subject, string htmlContent);
    }
}