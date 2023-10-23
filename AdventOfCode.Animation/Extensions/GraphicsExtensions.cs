namespace AdventOfCode.Animation.Extensions
{
    using System.Drawing;
    using System.Drawing.Imaging;

    public static class GraphicsExtensions
    {
        public static void DrawImageWithOpacity(
            this Graphics graphics,
            float opacity,
            Image image,
            Rectangle dest)
        {
            float[][] matrixItems =
            {
                new float[] { 1, 0, 0, 0, 0 },
                new float[] { 0, 1, 0, 0, 0 },
                new float[] { 0, 0, 1, 0, 0 },
                new float[] { 0, 0, 0, opacity, 0 },
                new float[] { 0, 0, 0, 0, 1 }
            };

            ColorMatrix colorMatrix = new(matrixItems);

            ImageAttributes imageAtt = new();
            imageAtt.SetColorMatrix(
               colorMatrix,
               ColorMatrixFlag.Default,
               ColorAdjustType.Bitmap);

            graphics.DrawImage(image, dest, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, imageAtt);
        }
    }
}
