namespace AdventOfCode.Puzzles._2021.Day_12___Passage_Pathing
{
    public class Link
    {
        public Link(string a, string b)
        {
            this.A = a;
            this.B = b;
        }

        public string A { get; }

        public string B { get; }

        public string Not(string value) => this.A == value ? this.B : this.A;
    }
}
