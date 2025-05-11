using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.Extensions.ObjectPool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothesMarketPlace.Infrastructure.Helpers
{
    public class HtmlTemplatesReader : IHtmlTemplatesReader
    {
        private readonly string _filepath = @"wwwroot/email-templates/";

        public async Task<string> ReadAsync(string templateName)
        {
            string fullPath = Path.Combine(_filepath, templateName);
            using var reader = File.OpenText(fullPath);
            var fileText = await reader.ReadToEndAsync();
            return fileText;
        }
    }
}
