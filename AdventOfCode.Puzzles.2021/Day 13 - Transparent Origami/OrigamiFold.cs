namespace AdventOfCode.Puzzles._2021.Day_13___Transparent_Origami
{
    public class OrigamiFold
    {
        public OrigamiFold(char axis, int value)
        {
            this.Axis = axis;
            this.Value = value;
            this.Dots = 0;
        }

        public char Axis { get; }

        public int Value { get; }

        public int Dots { get; set; }
    }
}