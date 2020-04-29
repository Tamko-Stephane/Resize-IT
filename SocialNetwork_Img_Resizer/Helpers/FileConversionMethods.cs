using System.Drawing;
using System.IO;
using System.Threading.Tasks;

namespace SocialNetwork_Img_Resizer.Helpers
{
    public static class FileConversionMethods
    {
        public async static Task<byte[]> ConvertFileToByteArray(Stream stream)
        {
            if (stream != null)
            {
                byte[] dataArray = null;
                //convert stream to array
                var task = Task.Run(
                    () =>
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            stream.CopyTo(ms);      //for .NET 4
                            dataArray = ms.ToArray();
                        }
                    }
                    );

                await task;
                return dataArray;
            }

            return null;
        }

        public async static Task<Image> ConvertByteArrayToImage(byte[] bytes)
        {
            if (bytes != null)
            {
                Image image = null;
                //convert stream to array
                var task = Task.Run(
                    () =>
                    {
                        using (MemoryStream ms = new MemoryStream(bytes))
                        {

                            image = Image.FromStream(ms);
                        }
                    }
                    );

                await task;
                return image;
            }

            return null;
        }
    }
}