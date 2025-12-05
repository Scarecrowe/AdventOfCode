namespace AdventOfCode.Puzzles._2022.Day_08___Treetop_Tree_House
{
    using AdventOfCode.Core;
    using AdventOfCode.Core.Extensions;

    public class TreetopTreeHouse
    {
        public TreetopTreeHouse(string[] input)
        {
            this.Map = Parse(input);
            this.Visible = this.Map.EdgeEnumerator().Select(e => e.Point).ToHashSet();
        }

        private IVectorCollection<int, int> Map { get; set; }

        private HashSet<Vector<int>> Visible { get; set; }

        public int VisibleTrees()
            => this.Visible.Count
            + this.ViewNorth()
            + this.ViewSouth()
            + this.ViewWest()
            + this.ViewEast();

        public int BestTreeHouseScore()
            => this.Map.AxisEnumerator()
            .Select(cell => cell.Point)
            .Max(point => this.MoveNorth(point) * this.MoveSouth(point) * this.MoveWest(point) * this.MoveEast(point));

        private static VectorArray<int, int> Parse(string[] input) => new(input, (c) => c.ToInt());

        private int View(
            Func<int, bool> iWhile,
            Func<int, bool> jWhile,
            Func<int, int> jIncrementor,
            int jStartIndex,
            Func<int, int, Vector<int>> tallestSelector,
            Func<int, int, Vector<int>> selector,
            Func<int, int, Vector<int>> contains)
        {
            int result = 0;

            for (int i = 1; iWhile(i); i++)
            {
                int tallest = this.Map[tallestSelector(i, 0)];

                for (int j = jStartIndex; jWhile(j); j = jIncrementor(j))
                {
                    if (this.Map[selector(i, j)] <= tallest)
                    {
                        continue;
                    }

                    tallest = this.Map[selector(i, j)];

                    if (this.Visible.Contains(contains(i, j)))
                    {
                        continue;
                    }

                    result++;
                    this.Visible.Add(contains(i, j));
                }
            }

            return result;
        }

        private int ViewNorth() => this.View(
            (i) => i < this.Map.Width - 1,
            (j) => j < this.Map.Height - 1,
            (j) => j + 1,
            1,
            (i, j) => new(i, 0),
            (i, j) => new(i, j),
            (i, j) => new(i, j));

        private int ViewWest() => this.View(
            (i) => i < this.Map.Height - 1,
            (j) => j < this.Map.Width - 1,
            (j) => j + 1,
            1,
            (i, j) => new(0, i),
            (i, j) => new(j, i),
            (i, j) => new(j, i));

        private int ViewSouth() => this.View(
            (i) => i < this.Map.Width - 1,
            (j) => j >= 1,
            (j) => j - 1,
            this.Map.Height - 2,
            (i, j) => new(i, this.Map.Height - 1),
            (i, j) => new(i, j),
            (i, j) => new(i, j));

        private int ViewEast() => this.View(
            (i) => i < this.Map.Height - 1,
            (j) => j >= 1,
            (j) => j - 1,
            this.Map.Width - 2,
            (i, j) => new(this.Map.Width - 1, i),
            (i, j) => new(j, i),
            (i, j) => new(j, i));

        private int Move(Vector<int> point, int startIndex, Func<int, bool> @while, Func<int, int> incrementor, Func<int, Vector<int>, Vector<int>> selector)
        {
            int result = 0;

            for (int i = startIndex; @while(i); i = incrementor(i))
            {
                if (this.Map[selector(i, point)] >= this.Map[point])
                {
                    result = incrementor(result);

                    return result;
                }

                if (this.Map[i, point.X] < this.Map[point])
                {
                    result = incrementor(result);
                }
            }

            return result;
        }

        private int MoveNorth(Vector<int> point)
            => this.Move(point, point.Y - 1, (i) => i >= 0, (i) => i - 1, (i, p) => new(p.X, i));

        private int MoveSouth(Vector<int> point)
            => this.Move(point, point.Y + 1, (i) => i < this.Map.Height, (i) => i + 1, (i, p) => new(p.X, i));

        private int MoveWest(Vector<int> point)
            => this.Move(point, point.X - 1, (i) => i >= 0, (i) => i - 1, (i, p) => new(i, p.Y));

        private int MoveEast(Vector<int> point)
            => this.Move(point, point.X + 1, (i) => i < this.Map.Width, (i) => i + 1, (i, p) => new(i, p.Y));
    }
}
