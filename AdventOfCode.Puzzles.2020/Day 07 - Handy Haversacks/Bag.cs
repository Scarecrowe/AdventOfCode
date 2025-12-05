namespace AdventOfCode.Puzzles._2020.Day_07___Handy_Haversacks
{
    public class Bag
    {
        public Bag(string name, bool outer, int count = 0)
        {
            this.Name = name;
            this.Count = count;
            this.Children = new();
            this.Outer = outer;
        }

        public string Name { get; }

        public int Count { get; }

        public List<Bag> Children { get; }

        public bool Outer { get; }

        public override string ToString() => this.Name;
    }
}
