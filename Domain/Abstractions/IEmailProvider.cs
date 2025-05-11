namespace Domain.Abstractions
{
    public interface IEmailProvider
    {
        void SendEmail(string receiverEmail, string receiverName, string subject, string message);
    }
}