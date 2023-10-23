namespace AdventOfCode.Animation.Extensions
{
    using System.Drawing;
    using System.Drawing.Imaging;

    public static class BitmapExtensions
    {
        public static byte[] ToByteArray(this Bitmap bitmap, ImageFormat format)
        {
            using MemoryStream ms = new();
            bitmap.Save(ms, format);
            ms.Seek(0, SeekOrigin.Begin);

            return ms.ToArray();
        }
    }
}
