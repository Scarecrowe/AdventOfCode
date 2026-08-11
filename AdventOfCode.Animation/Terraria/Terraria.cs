namespace AdventOfCode.Animation.Terraria
{
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Reflection;

    public class Terraria
    {
        public static ITerrariaRenderer LoadPuzzle(string[]? input = null)
        {
            Assembly assembly = Assembly.Load($"AdventOfCode.Puzzles.2018");

            Type? type = assembly
                .GetTypes()
                .FirstOrDefault(t =>
                    typeof(ITerrariaRenderer).IsAssignableFrom(t) &&
                    !t.IsInterface &&
                    !t.IsAbstract);

            if (type == null)
            {
                throw new InvalidOperationException(
                    "No ITerrariaRenderer implementation found.");
            }

            object[] args =
            [
                input == null ? Animation.GetInput(17, "Reservoir Research") : input
            ];

            ITerrariaRenderer renderer = (ITerrariaRenderer)Activator.CreateInstance(type, args)!;

            return renderer;
        }

        public static string GetAssetPath(string path) => $"Terraria\\Assets\\{path}";

        public static Image GetImage(string file) => Image.FromFile(GetAssetPath(file));

        public static void GenerateWallTilesets(string path)
        {
            if (!Directory.Exists($"{path}\\WallTiles"))
            {
                Directory.CreateDirectory($"{path}\\WallTiles");
            }

            foreach(string file in Directory.GetFiles(path).Where(x => Path.GetFileName(x).StartsWith("Wall_")))
            {
                Bitmap result = new(16 * 3, 16 * 2, PixelFormat.Format32bppArgb);
                Image tileset = Image.FromFile(file);

                using (Graphics graphics = Graphics.FromImage(result))
                {
                    graphics.Clear(Color.Transparent);
                    graphics.DrawImage(tileset, new Rectangle(0, 0, 16, 16), new Rectangle(44, 0, 16, 16), GraphicsUnit.Pixel);
                    graphics.DrawImage(tileset, new Rectangle(16, 0, 16, 16), new Rectangle(80, 0, 16, 16), GraphicsUnit.Pixel);
                    graphics.DrawImage(tileset, new Rectangle(32, 0, 16, 16), new Rectangle(116, 0, 16, 16), GraphicsUnit.Pixel);
                    graphics.DrawImage(tileset, new Rectangle(0, 16, 16, 16), new Rectangle(44, 44, 16, 16), GraphicsUnit.Pixel);
                    graphics.DrawImage(tileset, new Rectangle(16, 16, 16, 16), new Rectangle(80, 44, 16, 16), GraphicsUnit.Pixel);
                    graphics.DrawImage(tileset, new Rectangle(32, 16, 16, 16), new Rectangle(116, 44, 16, 16), GraphicsUnit.Pixel);
                }

                result.Save($"{path}\\WallTiles\\{Path.GetFileName(file)}", ImageFormat.Png);
            }
        }
    }
}
