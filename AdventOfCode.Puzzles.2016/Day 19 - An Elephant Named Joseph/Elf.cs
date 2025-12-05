namespace AdventOfCode.Puzzles._2016.Day_19___An_Elephant_Named_Joseph
{
    public class Elf
    {
        public Elf(int number)
        {
            this.Number = number;
            this.Presents = 1;
        }

        public int Number { get; private set; }

        public int Presents { get; private set; }

        public void AddPresents(int value) => this.Presents += value;

        public void Reset() => this.Presents = 0;

        public new string ToString() => $"Number: {this.Number}, Presents: {this.Presents}";
    }
}
