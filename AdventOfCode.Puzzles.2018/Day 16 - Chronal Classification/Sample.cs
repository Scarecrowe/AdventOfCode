namespace AdventOfCode.Puzzles._2018.Day_16___Chronal_Classification
{
    public class Sample
    {
        public Sample(int[] before, int[] after, int[] instruction)
        {
            this.Before = before;
            this.After = after;
            this.Instruction = instruction;
        }

        public int[] Before { get; }

        public int[] After { get; }

        public int[] Instruction { get; }
    }
}
