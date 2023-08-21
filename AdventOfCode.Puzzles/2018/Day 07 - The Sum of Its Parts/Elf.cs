namespace AdventOfCode.Puzzles._2018.Day_07___The_Sum_of_Its_Parts
{
    public class Elf
    {
        public Elf(ElfStep step)
        {
            this.Seconds = (step.Letter - 64) + 60;
            this.Step = step;
        }

        public int Seconds { get; set; }

        public ElfStep Step { get; }
    }
}
