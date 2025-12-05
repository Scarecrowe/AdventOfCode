namespace AdventOfCode.Puzzles._2017.Day_15___Dueling_Generators
{
    using AdventOfCode.Core.Extensions;

    public class DuelingGenerators
    {
        public DuelingGenerators(string[] input)
        {
            this.GeneratorA = input[0].Replace("Generator A starts with ").ToInt();
            this.GeneratorB = input[1].Replace("Generator B starts with ").ToInt();
        }

        private static int FactorA { get; } = 16807;

        private static int FactorB { get; } = 48271;

        private static int Divisor { get; } = 2147483647;

        private long GeneratorA { get; set; }

        private long GeneratorB { get; set; }

        public long Simple()
        {
            long result = 0;

            for (int i = 0; i < 40000000; i++)
            {
                this.GeneratorA = (this.GeneratorA * FactorA) % Divisor;
                this.GeneratorB = (this.GeneratorB * FactorB) % Divisor;

                if ((this.GeneratorA & 0xFFFF) == (this.GeneratorB & 0xFFFF))
                {
                    result++;
                }
            }

            return result;
        }

        public long Advanced()
        {
            int result = 0;

            Queue<long> queueA = new();
            Queue<long> queueB = new();

            int i = 0;

            while (i < 5000000)
            {
                this.GeneratorA = (this.GeneratorA * FactorA) % Divisor;
                this.GeneratorB = (this.GeneratorB * FactorB) % Divisor;

                if (this.GeneratorA % 4 == 0)
                {
                    queueA.Enqueue(this.GeneratorA);
                }

                if (this.GeneratorB % 8 == 0)
                {
                    queueB.Enqueue(this.GeneratorB);
                }

                if (queueA.Count > 0 && queueB.Count > 0)
                {
                    long genAVal = queueA.Dequeue();
                    long genBVal = queueB.Dequeue();

                    if ((genAVal & 0xFFFF) == (genBVal & 0xFFFF))
                    {
                        result++;
                    }

                    i++;
                }
            }

            return result;
        }
    }
}
