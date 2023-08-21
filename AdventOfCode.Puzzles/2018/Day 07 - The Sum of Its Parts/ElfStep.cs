namespace AdventOfCode.Puzzles._2018.Day_07___The_Sum_of_Its_Parts
{
    public class ElfStep
    {
        public ElfStep(char letter, List<char> parents)
        {
            this.Letter = letter;
            this.Parents = parents;
        }

        public char Letter { get; }

        public List<char> Parents { get; }
    }
}
