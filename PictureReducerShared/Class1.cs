using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;

namespace PictureReducerShared

{
    public static class ImageResizer
    {
        [SupportedOSPlatform("windows")]  // Ensure this method is only used on Windows platforms
        public static void ResizeImage(string inputPath, string outputPath, int maxWidth, int maxHeight, long quality)
        {
            using (Image original = Image.FromFile(inputPath))
            {
                int newWidth, newHeight;

                double ratioX = (double)maxWidth / original.Width;
                double ratioY = (double)maxHeight / original.Height;
                double ratio = Math.Min(ratioX, ratioY);

                newWidth = (int)(original.Width * ratio);
                newHeight = (int)(original.Height * ratio);

                using (Bitmap resized = new Bitmap(newWidth, newHeight))
                {
                    using (Graphics g = Graphics.FromImage(resized))
                    {
                        g.CompositingQuality = CompositingQuality.HighQuality;
                        g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        g.SmoothingMode = SmoothingMode.HighQuality;

                        g.DrawImage(original, 0, 0, newWidth, newHeight);
                    }

                    ImageCodecInfo jpegCodec = ImageCodecInfo.GetImageDecoders()
                        .First(c => c.FormatID == ImageFormat.Jpeg.Guid);

                    EncoderParameters encoderParams = new EncoderParameters(1);
                    encoderParams.Param[0] = new EncoderParameter(Encoder.Quality, quality);

                    resized.Save(outputPath, jpegCodec, encoderParams);
                }
            }
        }
    }
}
