namespace AdventOfCode.Puzzles._2021.Day_13___Transparent_Origami
{
    using System.Text;
    using AdventOfCode.Animation.Renderers;
    using AdventOfCode.Core;
    using AdventOfCode.Core.Extensions;

    public class TransparentOrigami
    {
        public TransparentOrigami(string[] input)
        {
            this.Dots = [];
            this.Folds = [];
            this.Map = new(0, 0);

            this.ParseInput(input);
            this.BuildGrid();
        }

        public TransparentOrigami(string[] input, IFrameRenderer renderer)
            : this(input)
        {
            this.Renderer = renderer;
        }

        public IFrameRenderer? Renderer { get; }

        public List<OrigamiFold> Folds { get; private set; }

        private HashSet<Vector<int>> Dots { get; set; }

        private VectorArray<int, int> Map { get; set; }

        public TransparentOrigami Fold()
        {
            foreach (OrigamiFold fold in this.Folds)
            {
                this.Map = fold.Axis == 'y'
                    ? this.FoldUp(fold)
                    : this.FoldLeft(fold);
            }

            return this;
        }

        public TransparentOrigami RenderSilver(int holdFrames = 24)
            => this.RenderFolds(maxFolds: 1, holdFrames);

        public TransparentOrigami RenderGold(int holdFrames = 48)
            => this.RenderFolds(maxFolds: null, holdFrames);

        public string Print()
        {
            StringBuilder result = new();
            result.AppendLine().AppendLine();

            for (int y = 0; y < this.Map.Height; y++)
            {
                for (int x = 0; x < this.Map.Width; x++)
                {
                    result.Append(this.Map[y, x] == 1 ? "#" : " ");
                }

                result.AppendLine();
            }

            result.AppendLine();

            return result.ToString();
        }

        private TransparentOrigami RenderFolds(int? maxFolds, int holdFrames)
        {
            if (this.Renderer == null)
            {
                return this;
            }

            int renderWidth = this.Map.Width;
            int renderHeight = this.Map.Height + 2;

            int totalFolds = maxFolds ?? this.Folds.Count;
            int foldNumber = 0;

            foreach (OrigamiFold fold in this.Folds.Take(totalFolds))
            {
                foldNumber++;

                this.Renderer.RenderFrame(new Frame(this.PadFrame(
                    this.BuildFrame(
                        $"TRANSPARENT ORIGAMI // FOLD {foldNumber:00} // {fold.Axis}={fold.Value}",
                        fold),
                    renderWidth,
                    renderHeight)));

                this.Map = fold.Axis == 'y'
                    ? this.FoldUp(fold)
                    : this.FoldLeft(fold);

                this.Renderer.RenderFrame(new Frame(this.PadFrame(
                    this.BuildFrame(
                        $"TRANSPARENT ORIGAMI // DOTS {this.CountDots():0000}",
                        null),
                    renderWidth,
                    renderHeight)));
            }

            string title = totalFolds == 1
                ? $"FIRST FOLD COMPLETE // DOTS {this.CountDots():0000}"
                : "CODE // PZFJHRFZ";

            for (int i = 0; i < holdFrames; i++)
            {
                this.Renderer.RenderFrame(new Frame(this.PadFrame(
                    this.BuildFrame(title, null),
                    renderWidth,
                    renderHeight)));
            }

            return this;
        }

        private string[] BuildFrame(string title, OrigamiFold? activeFold)
        {
            List<string> result = [];

            result.Add(title);
            result.Add(string.Empty);

            for (int y = 0; y < this.Map.Height; y++)
            {
                StringBuilder row = new();

                for (int x = 0; x < this.Map.Width; x++)
                {
                    if (activeFold != null &&
                        activeFold.Axis == 'y' &&
                        y == activeFold.Value)
                    {
                        row.Append('-');
                    }
                    else if (activeFold != null &&
                             activeFold.Axis == 'x' &&
                             x == activeFold.Value)
                    {
                        row.Append('|');
                    }
                    else
                    {
                        row.Append(this.Map[y, x] == 1 ? '#' : '.');
                    }
                }

                result.Add(row.ToString());
            }

            return [.. result];
        }

        private string[] PadFrame(string[] frame, int width, int height)
        {
            List<string> result = [];

            foreach (string row in frame)
            {
                result.Add(row.PadRight(width, ' '));
            }

            while (result.Count < height)
            {
                result.Add(new string(' ', width));
            }

            return [.. result];
        }

        public int CountDots()
        {
            int dots = 0;

            for (int y = 0; y < this.Map.Height; y++)
            {
                for (int x = 0; x < this.Map.Width; x++)
                {
                    if (this.Map[y, x] == 1)
                    {
                        dots++;
                    }
                }
            }

            return dots;
        }

        public TransparentOrigami FoldOnce()
        {
            OrigamiFold fold = this.Folds.First();

            this.Map = fold.Axis == 'y'
                ? this.FoldUp(fold)
                : this.FoldLeft(fold);

            return this;
        }

        private void ParseInput(string[] input)
        {
            bool isFolds = false;

            foreach (string line in input)
            {
                if (string.IsNullOrEmpty(line))
                {
                    isFolds = true;
                    continue;
                }

                if (!isFolds)
                {
                    string[] tokens = line.Split(",");

                    this.Dots.Add(new(tokens[0].ToInt(), tokens[1].ToInt()));

                    continue;
                }

                string[] foldTokens = line
                    .Replace("fold along ", string.Empty)
                    .Split("=");

                this.Folds.Add(new(foldTokens[0][0], foldTokens[1].ToInt()));
            }
        }

        private void BuildGrid()
        {
            int width = this.Dots.Max(c => c.X) + 1;
            int height = this.Dots.Max(c => c.Y) + 1;

            this.Map = new(width, height);

            foreach (Vector<int> point in this.Dots)
            {
                this.Map[point] = 1;
            }
        }

        private VectorArray<int, int> FoldUp(OrigamiFold fold)
        {
            VectorArray<int, int> map = new(this.Map.Width, fold.Value);

            for (int y = 0; y < this.Map.Height; y++)
            {
                if (y == fold.Value)
                {
                    continue;
                }

                int targetY = y < fold.Value
                    ? y
                    : fold.Value - (y - fold.Value);

                if (targetY < 0 || targetY >= map.Height)
                {
                    continue;
                }

                for (int x = 0; x < this.Map.Width; x++)
                {
                    if (this.Map[y, x] == 1)
                    {
                        map[targetY, x] = 1;
                    }
                }
            }

            fold.Dots = this.CountDots(map);

            return map;
        }

        private VectorArray<int, int> FoldLeft(OrigamiFold fold)
        {
            VectorArray<int, int> map = new(fold.Value, this.Map.Height);

            for (int y = 0; y < this.Map.Height; y++)
            {
                for (int x = 0; x < this.Map.Width; x++)
                {
                    if (x == fold.Value)
                    {
                        continue;
                    }

                    int targetX = x < fold.Value
                        ? x
                        : fold.Value - (x - fold.Value);

                    if (targetX < 0 || targetX >= map.Width)
                    {
                        continue;
                    }

                    if (this.Map[y, x] == 1)
                    {
                        map[y, targetX] = 1;
                    }
                }
            }

            fold.Dots = this.CountDots(map);

            return map;
        }

        private int CountDots(VectorArray<int, int> map)
        {
            int dots = 0;

            for (int y = 0; y < map.Height; y++)
            {
                for (int x = 0; x < map.Width; x++)
                {
                    if (map[y, x] == 1)
                    {
                        dots++;
                    }
                }
            }

            return dots;
        }
    }
}