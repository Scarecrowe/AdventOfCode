namespace AdventOfCode.Runner.Menus
{
    using AdventOfCode.Core;

    public abstract class Menu
    {
        protected static readonly string[] InvalidMessages =
        [
            "That’s not on the Naughty List.",
            "Even the elves haven’t heard of that one.",
            "That option isn’t on the list.",
            "Not on the board, I’m afraid.",
            "That’s… not a thing.",
            "Krampus is confused by your selection.",
            "The Archivist can’t find that entry.",
            "Nice try. Not quite.",
            "That didn’t make the list.",
            "Nope. That’s not one of ours."
        ];

        protected static readonly string[] PressAnyKeyMessages =
        [
            "Press any key to keep the sleigh moving...",
            "Press any key to wake the elves...",
            "Press any key to continue the expedition...",
            "Press any key... Santa is waiting."
        ];

        public Menu()
        {
            this.Title = string.Empty;
        }

        public Menu(string title)
        {
            this.Title = title;
        }

        public string Title { get; private set; }

        public void Reset()
        {
            Console.Clear();
            PuzzleRunner.PrintTree(13);
            PuzzleConsole.WriteLine();

            if (!string.IsNullOrEmpty(this.Title))
            {
                this.PrintTitle();
                PuzzleConsole.WriteLine();
            }

            PuzzleConsole.Flush();
        }

        public void WaitForUser()
        {
            PuzzleConsole.WriteLine();
            PuzzleConsole.WriteLine(RandomPressAnyKeyMessage());
            PuzzleConsole.Flush();
            Console.ReadLine();
        }

        public void PrintTitle()
        {
            PuzzleConsole.WriteLine($"=== {this.Title} ===");
            PuzzleConsole.Flush();
        }

        public void PrintSubTitle(string title)
        {
            PuzzleConsole.WriteLine($"--- {title} ---");
            PuzzleConsole.Flush();
        }

        public void WriteLine(string line)
        {
            PuzzleConsole.WriteLine(line);
        }

        protected static TMenu ValidateMenu<TMenu>(string input)
            where TMenu : struct, Enum
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                InvalidSelection();
                return default;
            }

            bool isNumber = int.TryParse(input.Trim(), out int value);

            if (!isNumber || !Enum.IsDefined(typeof(TMenu), value))
            {
                InvalidSelection();
                return default;
            }

            return (TMenu)Enum.ToObject(typeof(TMenu), value);
        }

        protected static int ValidateYear(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return 2015;
            }

            bool isNumber = int.TryParse(input.Trim(), out int year);

            if (!isNumber || (year < 2015 || year > 2025))
            {
                InvalidSelection();

                return -1;
            }

            return year;
        }

        protected static int ValidateDay(string input, int count = 25)
        {
            if (string.IsNullOrEmpty(input))
            {
                return 1;
            }

            bool isNumber = int.TryParse(input.Trim(), out int day);

            if (!isNumber || (day < 1 || day > count))
            {
                InvalidSelection();

                return -1;
            }

            return day;
        }

        protected static void InvalidSelection()
        {
            var message = InvalidMessages[Random.Shared.Next(InvalidMessages.Length)];

            PuzzleConsole.WriteLine();
            PuzzleConsole.WriteLine(message);
            PuzzleConsole.WriteLine();

            PuzzleConsole.Write("Press Enter to try again...");
            PuzzleConsole.Flush();

            Console.ReadLine();
            Console.Clear();
        }

        protected static bool PromptYes(string text)
        {
            PuzzleConsole.WriteLine();
            PuzzleConsole.Write(text);
            PuzzleConsole.Flush();

            string? input = Console.ReadLine();

            return IsYes(input);
        }

        protected static string RandomPressAnyKeyMessage() => PressAnyKeyMessages[Random.Shared.Next(PressAnyKeyMessages.Length)];

        protected static int PromptInt(string label, int defaultValue)
        {
            PuzzleConsole.Write($"{label} (default {defaultValue}): ");
            PuzzleConsole.Flush();
            var input = Console.ReadLine();

            if(input == "exit")
            {
                new ExitMenu().Execute();
            }

            if (string.IsNullOrWhiteSpace(input))
            {
                return defaultValue;
            }

            if (int.TryParse(input, out var value))
            {
                return value;
            }

            PuzzleConsole.WriteLine($"Invalid input. Using {defaultValue}.");
            PuzzleConsole.Flush();
            return defaultValue;
        }

        protected static bool IsYes(string? input)
        {
            return input?.Trim().Equals("y", StringComparison.OrdinalIgnoreCase) == true;
        }
    }
}
