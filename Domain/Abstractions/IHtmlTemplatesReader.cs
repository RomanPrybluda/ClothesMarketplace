
namespace ClothesMarketPlace.Infrastructure.Helpers
{
    public interface IHtmlTemplatesReader
    {
        Task<string> ReadAsync(string templateName);
    }
}