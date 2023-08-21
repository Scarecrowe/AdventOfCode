namespace AdventOfCode.Puzzles._2019.Day_23___Category_Six
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2019.IntCode;

    public class CategorySix
    {
        public CategorySix(string program)
        {
            this.Nics = new(50);

            for (int i = 0; i < 50; i++)
            {
                this.Nics.Add(new(program));
                this.Nics.Last().Input.Enqueue(i);
                this.Nics.Last().Run();
            }
        }

        private List<IntcodeCpu> Nics { get; set; }

        private Vector<int>? Nat { get; set; }

        public long Run(bool runWithNat = false)
        {
            long result = 0;
            long lastZero = -1;

            while (result == 0)
            {
                bool idle = true;

                foreach (IntcodeCpu cpu in this.Nics)
                {
                    if (cpu.Input.Count == 0)
                    {
                        cpu.Input.Enqueue(-1);
                    }

                    cpu.Run();

                    while (cpu.Output.Any())
                    {
                        idle = false;
                        int address = (int)cpu.Output.Dequeue();
                        long x = cpu.Output.Dequeue();
                        long y = cpu.Output.Dequeue();

                        if (address == 255)
                        {
                            if (!runWithNat)
                            {
                                return y;
                            }

                            this.Nat = new(x, y);

                            continue;
                        }

                        if (address == 0)
                        {
                            if (y == lastZero)
                            {
                                return y;
                            }

                            lastZero = y;
                        }

                        this.Nics[address].Input.Enqueue(x);
                        this.Nics[address].Input.Enqueue(y);
                    }
                }

                if (idle)
                {
                    if (this.Nat?.Y == lastZero)
                    {
                        return this.Nat.Y;
                    }

                    lastZero = this.Nat?.Y ?? 0;

                    this.Nics[0].Input.Enqueue(this.Nat?.X ?? 0);
                    this.Nics[0].Input.Enqueue(this.Nat?.Y ?? 0);
                }
            }

            return result;
        }
    }
}
