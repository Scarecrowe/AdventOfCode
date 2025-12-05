namespace AdventOfCode.Puzzles._2016.Day_14___One_Time_Pad
{
    public class Key
    {
        public Key(char @char, int index)
        {
            this.Char = @char;
            this.Current = 0;
            this.Index = index;
        }

        public char Char { get; }

        public int Current { get; private set; }

        public int Index { get; }

        public void Increment() => this.Current++;
    }
}
