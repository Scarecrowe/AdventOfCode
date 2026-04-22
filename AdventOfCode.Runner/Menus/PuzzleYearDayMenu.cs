namespace AdventOfCode.Runner.Menus
{
    using AdventOfCode.Core;

    public class PuzzleYearDayMenu : Menu, IMenu
    {
        public PuzzleYearDayMenu(string title)
            : base(title)
        {
            this.Year = -1;
            this.Day = -1;
        }

        public int Year { get; private set; }

        public int Day { get; private set; }

        public int Days { get; private set; }

        public Task<IMenu> Execute()
        {
            this.Year = -1;
            this.Day = -1;

            this.PromptYear();
            this.PromptDay();

            return Task.FromResult((IMenu)this);
        }

        public void ResetYear()
        {
            this.Year = -1;
        }

        private void PromptYear()
        {
            string? input = $"{PromptInt("Year", 2015)}";

            while (this.Year == -1)
            {
                this.Year = ValidateYear(input);
            }
        }

        private void PromptDay()
        {
            this.Reset();
            this.WriteLine("Assembling your solution...");
            PuzzleConsole.WriteLine();
            PuzzleConsole.Flush();

            List<string> days = PuzzleRunner.GetPuzzleDays(this.Year);

            foreach(string day in days)
            {
                this.WriteLine(day);
            }

            this.Days = days.Count;
            int count = this.Days;

            PuzzleConsole.WriteLine($"{++count,2}. Choose Another Puzzle");
            PuzzleConsole.WriteLine($"{++count,2}. Return to North Pole Operations");
            PuzzleConsole.WriteLine($"{++count,2}. Santa's Calling It a Day (Exit)");
            PuzzleConsole.WriteLine();

            while (this.Day == -1)
            {
                string? input = $"{PromptInt("Select an option: ", 1)}";

                this.Day = ValidateDay(input, count);
            }
        }
    }
}
