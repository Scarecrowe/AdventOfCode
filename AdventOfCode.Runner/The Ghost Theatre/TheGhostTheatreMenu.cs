namespace AdventOfCode.Runner.The_Ghost_Theatre
{
    using System;
    using System.Threading.Tasks;
    using AdventOfCode.Core;
    using AdventOfCode.Runner.Menus;
    using AdventOfCode.Runner.North_Pole_Operations;

    public class TheGhostTheatreMenu : Menu, IMenu
    {
        public TheGhostTheatreMenu()
            : base("The Ghost Theatre")
        {
        }

        public async Task<IMenu> Execute()
        {
            while (true)
            {
                this.Reset();
                PuzzleConsole.WriteLine("Assembling your solution...");
                PuzzleConsole.WriteLine();
                PuzzleConsole.Flush();

                int year = PromptInt("Year", 2015);
                int day = PromptInt("Day", 1);
                int iterations = PromptInt("Iterations", 1);

                await PuzzleRunner.RunAsync(year, day, iterations);

                PuzzleConsole.WriteLine();
                PuzzleConsole.Write("Run another puzzle? (y/n): ");
                PuzzleConsole.Flush();
                var input = Console.ReadLine();

                if (!IsYes(input))
                {
                    break;
                }

                PuzzleConsole.Clear();
            }

            return new NorthPoleOperationsMenu();
        }
    }
}
