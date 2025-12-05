namespace AdventOfCode.Puzzles._2023.Day_22___Sand_Slabs
{
    public class Brick
    {
        public int X1 { get; set; }
        public int Y1 { get; set; }
        public int Z1 { get; set; }
        public int X2 { get; set; }
        public int Y2 { get; set; }
        public int Z2 { get; set; }

        public List<Brick> Above { get; } = new();
        public List<Brick> Supporters { get; } = new();

        public IEnumerable<(int x, int y)> Coords
        {
            get
            {
                for (int x = X1; x <= X2; x++)
                    for (int y = Y1; y <= Y2; y++)
                        yield return (x, y);
            }
        }

        public int Height => Z2 - Z1 + 1;
    }
}
