namespace AdventOfCode.Puzzles._2019.Day_08___Space_Image_Format
{
    using System.Text;
    using AdventOfCode.Animation.Renderers;
    using AdventOfCode.Core;

    public class SpaceImageFormat
    {
        public SpaceImageFormat(string image, int width, int height)
        {
            this.Message = new char[0, 0];
            this.Image = image;
            this.Layers = new();
            this.Width = width;
            this.Height = height;
            this.ParseLayers();
        }

        public SpaceImageFormat(string image, int width, int height, IFrameRenderer renderer)
            : this(image, width, height)
        {
            this.Renderer = renderer;
        }

        public IFrameRenderer? Renderer { get; }

        private string Image { get; }

        private List<List<string>> Layers { get; }

        private char[,] Message { get; set; }

        private int Width { get; }

        private int Height { get; }

        public void Render()
        {
            char[,] current = new char[this.Height, this.Width];

            for (int y = 0; y < this.Height; y++)
            {
                for (int x = 0; x < this.Width; x++)
                {
                    current[y, x] = '2';
                }
            }

            for (int layer = 0; layer < this.Layers.Count; layer++)
            {
                for (int y = 0; y < this.Height; y++)
                {
                    for (int x = 0; x < this.Width; x++)
                    {
                        if (current[y, x] != '2')
                        {
                            continue;
                        }

                        char pixel = this.Layers[layer][y][x];

                        if (pixel != '2')
                        {
                            current[y, x] = pixel;
                        }
                    }
                }

                this.Renderer?.RenderFrame(new Frame(this.BuildFrame(current, layer)));
            }
        }

        public int FewestDigits()
        {
            List<string> min = this.Layers.Aggregate((a, b) => a.SelectMany(c => c).Count(c => c == '0') < b.SelectMany(c => c).Count(c => c == '0') ? a : b);

            return min.Sum(c => c.Count(d => d == '1')) * min.Sum(c => c.Count(d => d == '2'));
        }

        public SpaceImageFormat Decode()
        {
            this.Message = new char[this.Height, this.Width];

            foreach(Vector<int> point in Vector<int>.AxisEnumerator(this.Width, this.Height))
            {
                for (int layer = 0; layer < this.Layers.Count; layer++)
                {
                    PixelColour colour = (PixelColour)this.Layers[layer][point.Y][point.X];

                    if (colour == PixelColour.Transparent)
                    {
                        continue;
                    }

                    this.Message[point.Y, point.X] = (char)colour;
                    break;
                }
            }

            return this;
        }

        public string Print()
        {
            StringBuilder result = new();
            result.AppendLine().AppendLine();

            if (this.Message == null)
            {
                return string.Empty;
            }

            for (int y = 0; y < this.Height; y++)
            {
                for (int x = 0; x < this.Width; x++)
                {
                    result.Append($"{(this.Message[y, x] == '0' ? ' ' : '#')}");
                }

                result.AppendLine();
            }

            result.AppendLine();

            return result.ToString();
        }

        private void ParseLayers()
        {
            int index = 0;

            while (index < this.Image.Length)
            {
                this.Layers.Add(new());

                for (int i = 0; i < this.Height; i++)
                {
                    this.Layers.Last().Add(this.Image.Substring(index, this.Width));
                    index += this.Width;
                }
            }
        }

        private string[] BuildFrame(char[,] image, int layer)
        {
            List<string> result = [];

            result.Add($"SPACE IMAGE FORMAT // LAYER {layer + 1:000}/{this.Layers.Count:000}");
            result.Add(new string('-', this.Width));

            for (int y = 0; y < this.Height; y++)
            {
                StringBuilder sb = new();

                for (int x = 0; x < this.Width; x++)
                {
                    sb.Append(image[y, x] switch
                    {
                        '0' => ' ',
                        '1' => '#',
                        '2' => '·',
                        _ => '?'
                    });
                }

                result.Add(sb.ToString());
            }

            return [.. result];
        }
    }
}
