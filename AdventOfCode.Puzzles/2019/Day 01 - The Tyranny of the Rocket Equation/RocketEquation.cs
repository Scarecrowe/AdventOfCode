namespace AdventOfCode.Puzzles._2019.Day_01___The_Tyranny_of_the_Rocket_Equation
{
    using AdventOfCode.Core.Extensions;

    public class RocketEquation
    {
        public static int FuelRequirements(string[] input) => input.ToInt().Sum(mass => CalculateFuel(mass));

        public static int FullFuelRequirements(string[] input)
        {
            int fuel = 0;

            foreach (string mass in input)
            {
                int moduleMass = CalculateFuel(int.Parse(mass));

                fuel += moduleMass;

                while (true)
                {
                    moduleMass = CalculateFuel(moduleMass);

                    if (moduleMass <= 0)
                    {
                        break;
                    }

                    fuel += moduleMass;
                }
            }

            return fuel;
        }

        private static int CalculateFuel(int mass) => (int)Math.Floor(mass / 3M) - 2;
    }
}
