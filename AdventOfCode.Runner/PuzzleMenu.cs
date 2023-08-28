namespace AdventOfCode.Runner
{
    public class PuzzleMenu
    {
        public PuzzleMenu()
        {
            this.MainMenu();
        }

        private static void PrintTitle()
        {
            Console.WriteLine();
            Console.WriteLine(" Advent of Code Puzzle Runner");
            Console.WriteLine();
        }

        private void MainMenu()
        {
            Console.Clear();
            PuzzleRunner.PrintTree(13);
            PrintTitle();
            Console.WriteLine(" Choose an option:");
            Console.WriteLine();
            Console.WriteLine(" 1) Run a day");
            Console.WriteLine(" 2) Run a year");
            Console.WriteLine(" 3) Run all");
            Console.WriteLine(" 4) Exit");
            Console.WriteLine();
            Console.Write(" Select an option: ");

            switch (this.ValidateMenu(Console.ReadLine() ?? string.Empty))
            {
                case Menu.Day:
                    this.DayMenu();
                    break;
                case Menu.Year:
                    this.YearMenu();
                    break;
                case Menu.All:
                    this.AllMenu();
                    break;
                case Menu.Exit:
                    break;
            }
        }

        private void DayMenu()
        {
            Console.Clear();
            PuzzleRunner.PrintTree(13);
            Console.WriteLine();

            Console.Write(" Select a year: ");
            int year = this.ValidateYear(Console.ReadLine() ?? string.Empty);

            Console.Write(" Select a day: ");
            int day = this.ValidateDay(Console.ReadLine() ?? string.Empty);

            PuzzleRunner.RunPuzzle(year, day);

            Console.WriteLine();
            Console.Write(" Press any key to continue");
            Console.ReadLine();
            this.MainMenu();
        }

        private void YearMenu()
        {
            Console.Clear();
            PuzzleRunner.PrintTree(13);
            Console.WriteLine();
            Console.Write(" Select a year: ");

            int year = this.ValidateYear(Console.ReadLine() ?? string.Empty);

            for (int day = 1; day <= 25; day++)
            {
                PuzzleRunner.RunPuzzle(year, day);
            }

            Console.WriteLine();
            Console.Write(" Press any key to continue");
            Console.ReadLine();
            this.MainMenu();
        }

        private void AllMenu()
        {
            Console.Clear();
            PuzzleRunner.PrintTree(13);
            Console.WriteLine();
            Console.Write(" Select a year: ");

            for (int year = 2015; year <= 2022; year++)
            {
                for (int day = 1; day <= 25; day++)
                {
                    PuzzleRunner.RunPuzzle(year, day);
                }
            }

            Console.WriteLine();
            Console.Write(" Press any key to continue");
            Console.ReadLine();
            this.MainMenu();
        }

        private Menu ValidateMenu(string input)
        {
            bool isNumber = int.TryParse(input.Trim(), out int value);

            if (!isNumber
                || (value < 1 || value > 4))
            {
                this.InvalidSelection();
            }

            return (Menu)value;
        }

        private int ValidateYear(string input)
        {
            bool isNumber = int.TryParse(input.Trim(), out int year);

            if (!isNumber || (year < 2015 || year > 2022))
            {
                this.InvalidSelection();
            }

            return year;
        }

        private int ValidateDay(string input)
        {
            bool isNumber = int.TryParse(input.Trim(), out int day);

            if (!isNumber || (day < 1 || day > 25))
            {
                this.InvalidSelection();
            }

            return day;
        }

        private void InvalidSelection()
        {
            Console.WriteLine();
            Console.Write(" Invalid selection, press any key to continue");
            Console.ReadLine();
            Console.Clear();
            PuzzleRunner.PrintTree(13);
            this.MainMenu();
        }
    }
}
