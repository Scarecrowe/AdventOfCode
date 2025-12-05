namespace AdventOfCode.Puzzles._2021.Day_13___Transparent_Origami
{
    using System.Text;
    using AdventOfCode.Core;
    using AdventOfCode.Core.Extensions;

    public class TransparentOrigami
    {
        public TransparentOrigami(string[] input)
        {
            this.Dots = new();
            this.Folds = new();
            this.Map = new (0, 0);

            this.ParseInput(input);
            this.BuildGrid();
        }

        public HashSet<OrigamiFold> Folds { get; private set; }

        private HashSet<Vector<int>> Dots { get; set; }

        private VectorArray<int, int> Map { get; set; }

        public TransparentOrigami Fold()
        {
            foreach (OrigamiFold fold in this.Folds)
            {
                this.Map = fold.IsHorizontal ? this.FoldHorizontally(fold) : this.FoldVertically(fold);
            }

            return this;
        }

        public TransparentOrigami PrintFolds()
        {
            for (int i = 0; i < this.Folds.Count; i++)
            {
                OrigamiFold fold = this.Folds.ElementAt(i);

                PuzzleConsole.WriteLine($"Fold {i + 1}: -> {fold.Dots}");
            }

            PuzzleConsole.WriteLine();

            return this;
        }

        public string Print()
        {
            if (this.Map == null)
            {
                return string.Empty;
            }

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

                string[] tokens;

                if (!isFolds)
                {
                    tokens = line.Split(",");

                    this.Dots.Add(new(tokens[0].ToInt(), tokens[1].ToInt()));

                    continue;
                }

                tokens = line.Replace("fold along ").Split("=");

                this.Folds.Add(new(tokens[0] == "x", tokens[1].ToInt(), 0));
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

        private VectorArray<int, int> FoldVertically(OrigamiFold fold)
        {
            if (this.Map == null)
            {
                return new (0, 0);
            }

            VectorArray<int, int> map = new (this.Map.Width, fold.Value);

            for (int y = 0; y < fold.Value; y++)
            {
                for (int x = 0; x < this.Map.Width; x++)
                {
                    if (this.Map[y, x] == 1 || this.Map[fold.Value + Math.Min(fold.Value - y, this.Map.Width), x] == 1)
                    {
                        map[y, x] = 1;
                        fold.Dots++;
                    }
                }
            }

            return map;
        }

        private VectorArray<int, int> FoldHorizontally(OrigamiFold fold)
        {
            if (this.Map == null)
            {
                return new(0, 0);
            }

            VectorArray<int, int> map = new(fold.Value, this.Map.Height);

            for (int y = 0; y < this.Map.Height; y++)
            {
                for (int x = 0; x < fold.Value; x++)
                {
                    if (this.Map[y, x] == 1 || this.Map[y, fold.Value + Math.Min(fold.Value - x, this.Map.Height)] == 1)
                    {
                        map[y, x] = 1;
                        fold.Dots++;
                    }
                }
            }

            return map;
        }
    }
}
