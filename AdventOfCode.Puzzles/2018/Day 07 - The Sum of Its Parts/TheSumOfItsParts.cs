namespace AdventOfCode.Puzzles._2018.Day_07___The_Sum_of_Its_Parts
{
    using AdventOfCode.Core.Extensions;

    public class TheSumOfItsParts
    {
        public TheSumOfItsParts(string[] input)
        {
            this.Instructions = input.Select(x => new Instruction(x)).ToList();
            this.Steps = this.DistinctSteps().Select(step => new ElfStep(step, this.Instructions.Where(x => x.After == step).Select(x => x.Before).OrderBy(x => x).ToList())).ToList();
        }

        public List<Instruction> Instructions { get; }

        public List<ElfStep> Steps { get; }

        public string AssembleyOrder()
        {
            List<char> result = new();
            List<ElfStep> steps = this.Steps.Where(x => x.Parents.Count == 0).ToList();
            ElfStep? current = steps.First();

            while (current != null)
            {
                if (!result.Contains(current.Letter))
                {
                    result.Add(current.Letter);
                }

                steps.Remove(current);

                foreach (ElfStep step in this.Steps)
                {
                    if (step.Parents.Contains(current.Letter))
                    {
                        step.Parents.Remove(current.Letter);
                        steps.Add(step);
                    }
                }

                current = steps.OrderBy(x => x.Letter).FirstOrDefault(x => x.Parents.Count == 0);
            }

            return result.Join();
        }

        public int AssemblyTime(int elfCount)
        {
            List<Elf> elves = new();

            List<ElfStep> steps = this.Steps.Where(x => x.Parents.Count == 0).ToList();
            ElfStep current = steps.First();
            int seconds = 0;

            while (true)
            {
                foreach (ElfStep step in this.Steps.Where(x => x.Parents.Count == 0).ToList())
                {
                    if (elves.Count < elfCount)
                    {
                        elves.Add(new Elf(step));
                        this.Steps.Remove(step);
                    }
                }

                List<Elf> finished = new();

                foreach (Elf elf in elves)
                {
                    elf.Seconds--;
                    if (elf.Seconds == 0)
                    {
                        finished.Add(elf);

                        foreach (ElfStep step in this.Steps)
                        {
                            step.Parents.Remove(elf.Step.Letter);
                        }
                    }
                }

                foreach (Elf elf in finished)
                {
                    elves.Remove(elf);
                }

                seconds++;

                if (elves.Count == 0 && this.Steps.Count == 0)
                {
                    return seconds;
                }
            }
        }

        private HashSet<char> DistinctSteps()
        {
            HashSet<char> steps = new();

            foreach (Instruction instruction in this.Instructions)
            {
                if (!steps.Contains(instruction.After))
                {
                    steps.Add(instruction.After);
                }

                if (!steps.Contains(instruction.Before))
                {
                    steps.Add(instruction.Before);
                }
            }

            return steps;
        }
    }
}
