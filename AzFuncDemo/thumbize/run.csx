#r "System.Drawing"

using ImageResizer;
using System.Drawing;
using System.Drawing.Imaging;

public static void Run(Stream raw, string imageName, Stream thumbnail, TraceWriter log)
{
    log.Info($"C# Blob trigger function Processed blob\n Name:{imageName} \n Size: {raw.Length} Bytes");

    var settings = new ImageResizer.ResizeSettings
    {
        MaxWidth = 256,
        Format = "png"
    };

    // Take our raw stream (pointing to raw-images blob)
    // Resize using our settings
    // Output to thumbnail stream (pointing to thumbs blob)
    ImageResizer.ImageBuilder.Current.Build(raw, thumbnail, settings);
}