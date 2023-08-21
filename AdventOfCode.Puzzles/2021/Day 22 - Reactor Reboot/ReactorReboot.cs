namespace AdventOfCode.Puzzles._2021.Day_22___Reactor_Reboot
{
    public class ReactorReboot
    {
        public ReactorReboot(string[] input)
        {
            this.Instructions = input.Select(x => new Instruction(x)).ToList();
            this.On = new();
            this.Off = new();
        }

        private List<Instruction> Instructions { get; set; }

        private List<Cube> On { get; set; }

        private List<Cube> Off { get; set; }

        public long Reboot(bool ignore = true)
        {
            foreach (Instruction instruction in this.Instructions)
            {
                this.Reboot(instruction, ignore);
            }

            return this.On.Sum(x => x.Volume()) - this.Off.Sum(x => x.Volume());
        }

        private void Reboot(Instruction instruction, bool ignore)
        {
            if (ignore && instruction.Ignore)
            {
                return;
            }

            List<Cube> overlaps = new();
            Cube cube = new(instruction);

            foreach (Cube on in this.On)
            {
                if (cube.Intersects(on))
                {
                    overlaps.Add(cube.Overlaps(on));
                }
            }

            foreach (Cube off in this.Off)
            {
                if (cube.Intersects(off))
                {
                    this.On.Add(cube.Overlaps(off));
                }
            }

            this.Off.AddRange(overlaps);

            if (instruction.IsOn)
            {
                this.On.Add(cube);
            }
        }
    }
}
