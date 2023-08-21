namespace AdventOfCode.Puzzles._2021.Day_13___Transparent_Origami
{
    public class OrigamiFold
    {
        public OrigamiFold(bool isHorizontal, int value, int dots)
        {
            this.IsHorizontal = isHorizontal;
            this.Value = value;
            this.Dots = dots;
        }

        public bool IsHorizontal { get; }

        public int Value { get; }

        public int Dots { get; set; }
    }
}
