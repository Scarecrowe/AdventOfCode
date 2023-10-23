namespace AdventOfCode.Animation.Extensions
{
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Imaging;

    public static class ImageExtensions
    {
        public static Image SetOpacity(this Image image, float opacity)
        {
            ColorMatrix colorMatrix = new()
            {
                Matrix33 = opacity
            };
            ImageAttributes imageAttributes = new();
            imageAttributes.SetColorMatrix(
                colorMatrix,
                ColorMatrixFlag.Default,
                ColorAdjustType.Bitmap);
            Bitmap result = new(image.Width, image.Height);

            using (Graphics graphcis = Graphics.FromImage(result))
            {
                graphcis.SmoothingMode = SmoothingMode.AntiAlias;
                graphcis.DrawImage(
                    image,
                    new Rectangle(0, 0, image.Width, image.Height),
                    0,
                    0,
                    image.Width,
                    image.Height,
                    GraphicsUnit.Pixel,
                    imageAttributes);
            }

            return result;
        }

        public static Image FlipHorizontally(this Image image)
        {
            Image result = (Image)image.Clone();

            result.RotateFlip(RotateFlipType.RotateNoneFlipX);

            return result;
        }
    }
}
