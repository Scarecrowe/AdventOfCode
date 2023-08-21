namespace AdventOfCode.Puzzles._2016.Day_11___Radioisotope_Thermoelectric_Generators
{
    using AdventOfCode.Core.Extensions;

    public class RadioisotopeThermoelectricGenerators
    {
        public RadioisotopeThermoelectricGenerators(string[] input, bool extraComponents = false)
        {
            this.Floors = new int[5];
            this.FloorNumbers = new() { string.Empty, "first", "second", "third", "fourth" };

            if (extraComponents)
            {
                input = LoadExtraComponents(input);
            }

            foreach (string line in input)
            {
                this.ParseLine(line);
            }
        }

        private List<string> FloorNumbers { get; set; }

        private int[] Floors { get; set; }

        public int MinimumSteps()
        {
            int steps = 0;
            int totalComponents = this.Floors.Sum();
            int elevatorComponents = Math.Min(this.Floors[1], 2);
            this.Floors[1] -= elevatorComponents;
            int floor = 1;

            while (this.Floors[4] + 1 != totalComponents)
            {
                this.MoveComponents(false, ref floor, ref steps, ref elevatorComponents, () => elevatorComponents < 2 && floor > 1);
                this.MoveComponents(true, ref floor, ref steps, ref elevatorComponents, () => floor < 4);
                this.Floors[4] += 1;
                elevatorComponents--;
            }

            return steps;
        }

        private static string[] LoadExtraComponents(string[] input)
        {
            int splitIndex = input[0].IndexOf(" and a");
            string first = input[0][..splitIndex];
            string last = input[0][splitIndex..];
            string extra = ", a elerium generator, a elerium-compatible microchip, a dilithium generator, a dilithium-compatible microchip,";
            input[0] = $"{first}{extra}{last}";

            return input;
        }

        private void ParseLine(string line)
        {
            line = line.Replace("The ").Replace(".");
            string number = line[..line.IndexOf(" ")];
            int floorNumber = this.FloorNumbers.IndexOf(number);
            line = line.Replace($"{number} floor contains ", string.Empty);

            if (line.StartsWith("nothing"))
            {
                return;
            }

            line = line[2..].Replace(", and a ", ", a ");

            string[] components = line.Contains(',')
                ? line.Split(", a ")
                : line.Split(" and a ");

            this.Floors[floorNumber] = components.Length;

            if (floorNumber == 1)
            {
                this.Floors[floorNumber]++;
            }
        }

        private void MoveComponents(
            bool up,
            ref int floor,
            ref int steps,
            ref int elevatorComponents,
            Func<bool> comparer)
        {
            while (comparer())
            {
                floor = up ? floor + 1 : floor - 1;
                int componentsRemoved = Math.Min(this.Floors[floor], 2 - elevatorComponents);

                if (componentsRemoved > 0)
                {
                    elevatorComponents += componentsRemoved;
                    this.Floors[floor] -= componentsRemoved;
                }

                steps++;
            }
        }
    }
}
