namespace AdventOfCode.Puzzles._2018.Day_01___Chronal_Calibration
{
    using AdventOfCode.Core.Extensions;

    public class ChronalCalibration
    {
        public ChronalCalibration(string[] input) => this.Frequencies = Parse(input);

        public int Frequency { get; private set; }

        public List<int> Frequencies { get; }

        public ChronalCalibration Run()
        {
            this.Frequency = this.Frequencies.Sum();

            return this;
        }

        public ChronalCalibration Run(bool multiple = false)
        {
            if (!multiple)
            {
                this.Frequency = this.Frequencies.Sum();

                return this;
            }

            return this.Multiple();
        }

        private static List<int> Parse(string[] input) => input.ToIntList();

        private ChronalCalibration Multiple()
        {
            HashSet<int> frequencies = new();

            while (true)
            {
                foreach (int frequency in this.Frequencies)
                {
                    this.Frequency += frequency;

                    if (frequencies.Contains(this.Frequency))
                    {
                        return this;
                    }
                    else
                    {
                        frequencies.Add(this.Frequency);
                    }
                }
            }
        }
    }
}
