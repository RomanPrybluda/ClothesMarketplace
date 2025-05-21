namespace Domain
{
    public class ImageEncoderSettings
    {
        public ImageFormatConfig[] Formats { get; set; } = Array.Empty<ImageFormatConfig>();
    }

    public class ImageFormatConfig
    {
        public string Name { get; set; } = string.Empty;

        public string[] Extensions { get; set; } = Array.Empty<string>();

        public ImageEncoderFormatSettings Settings { get; set; } = new ImageEncoderFormatSettings();
    }

    public class ImageEncoderFormatSettings
    {
        public int Quality { get; set; }

        public int CompressionLevel { get; set; }
    }
}