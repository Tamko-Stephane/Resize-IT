using System.Drawing.Imaging;

namespace SocialNetwork_Img_Resizer.Helpers
{
    public static class Utility
    {
        public static ImageFormat GetImageFormat(string format)
        {
            switch (format)
            {
                case "png":
                case "PNG":
                    return ImageFormat.Png;
                case "bmp":
                case "BMP":
                    return ImageFormat.Bmp;
                case "jpeg":
                case "JPEG":
                    return ImageFormat.Jpeg;
                case "tiff":
                case "TIFF":
                    return ImageFormat.Tiff;
                case "gif":
                case "GIF":
                    return ImageFormat.Gif;
                default:
                    return ImageFormat.Png;
            }
        }
    }
}