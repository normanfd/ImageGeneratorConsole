using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace ImageGeneratorConsole
{
    #pragma warning disable S1118 // Utility classes should not have public constructors
    internal class Program
    #pragma warning restore S1118 // Utility classes should not have public constructors
    {
        private const string V = "D:\\ktp.jpg";

        static void Main()
        {
            string path = V;
            Image image = Image.FromFile(path);

            string dataUri = Encode(image);
            Image decodedImage = Decode(dataUri);

            new Bitmap(decodedImage).Save("D:\\ktpdecode." + decodedImage.RawFormat);
        }
        public static string Encode(Image image)
        {
            MemoryStream memoryStream = new MemoryStream();
            MemoryStream m = memoryStream;
            image.Save(m, image.RawFormat);
            byte[] imageBytes = m.ToArray();
            string mimetype = GetMimeType(image);
            return "data:" + mimetype + ";base64," + Convert.ToBase64String(imageBytes);
        }
        public static Image Decode(string dataUri)
        {
            byte[] bytes = Convert.FromBase64String(dataUri.Split(new[] { ',' }, 2)[1]);
            Image image;
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                image = Image.FromStream(ms);
            }
            return image;
        }

        public static string GetMimeType(Image i)
        {
            foreach (ImageCodecInfo codec in ImageCodecInfo.GetImageDecoders())
            {
                if (codec.FormatID == i.RawFormat.Guid)
                    return codec.MimeType;
            }
            return "image/unknown";
        }
    }
}
