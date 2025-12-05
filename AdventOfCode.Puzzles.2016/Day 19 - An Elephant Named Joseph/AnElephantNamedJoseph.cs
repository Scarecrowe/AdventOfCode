namespace AdventOfCode.Puzzles._2016.Day_19___An_Elephant_Named_Joseph
{
    using AdventOfCode.Core.Extensions;

    public class AnElephantNamedJoseph
    {
        public AnElephantNamedJoseph(string[] input)
        {
            this.ElfCount = input[0].ToInt();
            this.Elves = new();

            for (int i = 1; i <= this.ElfCount; i++)
            {
                this.Elves.Add(new(i));
            }
        }

        public int ElfCount { get; private set; }

        private List<Elf> Elves { get; }

        public int Clockwise()
        {
            Elf? current = this.Elves[0];

            do
            {
                int index = current.Number;
                Elf? next = null;

                if (index >= this.Elves.Count)
                {
                    index = 0;
                }

                while (next == null || next.Presents == 0)
                {
                    next = this.Elves[index];

                    index++;

                    if (index >= this.Elves.Count)
                    {
                        index = 0;
                    }
                }

                current.AddPresents(next.Presents);
                next.Reset();

                this.ElfCount--;

                index = next.Number;
                current = null;

                if (index >= this.Elves.Count)
                {
                    index = 0;
                }

                while (current == null || current.Presents == 0)
                {
                    current = this.Elves[index];
                    index++;

                    if (index >= this.Elves.Count)
                    {
                        index = 0;
                    }
                }
            }
            while (this.ElfCount > 1);

            for (int i = 0; i < this.Elves.Count; i++)
            {
                if (this.Elves[i].Presents > 0)
                {
                    return this.Elves[i].Number;
                }
            }

            return 0;
        }

        public int Opposite()
        {
            int pow = (int)Math.Pow(3, (int)Math.Floor(Math.Log(this.ElfCount) / Math.Log(3)));

            if (this.ElfCount == pow)
            {
                return this.ElfCount;
            }

            if (this.ElfCount - pow <= pow)
            {
                return this.ElfCount - pow;
            }

            return (2 * this.ElfCount) - (3 * pow);
        }
    }
}
