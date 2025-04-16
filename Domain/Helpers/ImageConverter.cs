using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Webp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Helpers
{
    public static class ImageConverter
    {
        public static Byte[] ConvertToWebpImageFormat(byte[] imageBytes)
        {
            using var img = Image.Load(imageBytes);
            using var memoryStream = new MemoryStream();
            var encoder = new WebpEncoder() 
            { 
                Quality = 60, 
                FileFormat = WebpFileFormatType.Lossy,
                Method = (WebpEncodingMethod)6,
                UseAlphaCompression = true,
                FilterStrength = 50
            };
            img.SaveAsWebp(memoryStream, encoder);
            return memoryStream.ToArray();
        }
    }
}
