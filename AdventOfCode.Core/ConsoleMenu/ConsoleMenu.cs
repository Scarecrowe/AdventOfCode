namespace AdventOfCode.Core.ConsoleMenu
{
    using AdventOfCode.Core;

    public abstract class ConsoleMenu
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

        public ConsoleMenu()
        {
            this.Parent = default!;
            this.Title = string.Empty;
            this.SubTitle = string.Empty;
            this.Items = new ConsoleMenuItems();
            this.History = [];
        }

        public ConsoleMenu(IConsoleMenu parent)
        {
            this.Parent = parent;
            this.Title = string.Empty;
            this.SubTitle = string.Empty;
            this.Items = new ConsoleMenuItems();
            this.History = [];
        }

        public ConsoleMenu(string title)
        {
            this.Parent = default!;
            this.Title = title;
            this.SubTitle = string.Empty;
            this.Items = new ConsoleMenuItems();
            this.History = [];
            this.PushHistory(title);
        }

        public ConsoleMenu(IConsoleMenu parent, string title)
        {
            this.Parent = parent;
            this.Title = title;
            this.SubTitle = string.Empty;
            this.Items = new ConsoleMenuItems();
            this.History = [.. parent.History];
        }

        public List<string> History { get; }

        public IConsoleMenuItems Items { get; }

        public IConsoleMenu Parent { get; }

        public string Title { get; private set; }

        public string SubTitle { get; private set; }

        public bool MainMenu { get; private set; }

        public void SetTitle(string title)
        {
            this.Title = title;
        }

        public void SetSubTitle(string title)
        {
            this.SubTitle = title;
        }

        public void WriteTree(int count = 13)
        {
            string[] tokens = new string[9];
            tokens[0] = @"         ";
            tokens[1] = @"    *    ";
            tokens[2] = @"   /.\   ";
            tokens[3] = @"  /..'\  ";
            tokens[4] = @"  /'.'\  ";
            tokens[5] = @" /.''.'\ ";
            tokens[6] = @" /.'.'.\ ";
            tokens[7] = @"/'.''.'.\";
            tokens[8] = @"^^^[_]^^^";

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < count; j++)
                {
                    this.Write(tokens[i]);
                }

                this.WriteLine();
            }

            this.WriteLine();
            this.Flush();
        }

        public void PushHistory(string value)
        {
            this.History.Add(value);
        }

        public void PopHistory()
        {
            this.History.RemoveAt(this.History.Count - 1);
        }

        public void GotoMainMenu()
        {
            this.MainMenu = true;
        }

        public void Reset()
        {
            Console.Clear();
            this.WriteTree(13);
            this.WriteTitle();

            if (!string.IsNullOrEmpty(this.SubTitle))
            {
                this.WriteSubTitle();
            }

            this.Flush();
        }

        public void WaitForUser()
        {
            this.WriteLine();
            this.WriteLine(RandomPressAnyKeyMessage());
            this.Flush();
            Console.ReadLine();
        }

        public void WriteTitle()
        {
            if (this.History.Any())
            {
                this.WriteLine($"=== {string.Join(" / ", this.History)} ===");
            }
            else
            {
                this.WriteLine($"=== {this.Title} ===");
            }
            
            this.WriteLine();
            this.Flush();
        }

        public void WriteSubTitle()
        {
            this.WriteLine($"--- {this.SubTitle} ---");
            this.WriteLine();
            this.Flush();
        }

        public void AddBackMenuItem(string label)
        {
            this.Items.Add(GenericMenu.Back, "Back", label);
        }

        public void AddMainMenuItem()
        {
            this.Items.Add(GenericMenu.MainMenu, "Main Menu", "Return to North Pole Operations");
        }

        public void AddExitMenuItem()
        {
            this.Items.Add(GenericMenu.Exit, "Exit", "Santa's Calling It a Day");
        }

        public void AddGenericMenuItems(string backLabel)
        {
            this.AddBackMenuItem(backLabel);
            this.AddMainMenuItem();
            this.AddExitMenuItem();
        }

        public async Task<IConsoleMenuItem> WriteMenu()
        {
            foreach (ConsoleMenuItem item in this.Items)
            {
                if (string.IsNullOrEmpty(item.Description))
                {
                    this.WriteLine($"{item.Index:D2}. {item.Name,-30}");
                }
                else
                {
                    this.WriteLine($"{item.Index:D2}. {item.Name,-30} - {item.Description}");
                }
            }

            this.WriteLine();

            int index = PromptInt("Select an option: ", 1);

            switch (index)
            {
                case GenericMenu.Back:
                    return this.Items.FirstOrDefault(x => x.GetKey<int>() == GenericMenu.Back)!;
                case GenericMenu.MainMenu:
                    return this.Items.FirstOrDefault(x => x.GetKey<int>() == GenericMenu.MainMenu)!;
                case GenericMenu.Exit:
                    await this.Exit();
                    break;
            }

            return this.Items.FirstOrDefault(x => x.Index == index)!;
        }

        public async Task Exit()
        {
            await new ExitMenu().Execute();
        }

        public void Write(string chr)
        {
            PuzzleConsole.Write(chr);
        }

        public void WriteLine()
        {
            PuzzleConsole.WriteLine();
        }

        public void WriteLine(string line)
        {
            PuzzleConsole.WriteLine(line);
        }

        public void Flush()
        {
            PuzzleConsole.Flush();
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

        protected static bool PromptBool(string text)
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
            string? input = Console.ReadLine();

            if (input == "back")
            {
                return GenericMenu.Back;
            }

            if (input == "main"
                || input == "main menu")
            {
                return GenericMenu.MainMenu;
            }

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

        protected static string PromptString(string label, string defaultValue)
        {
            PuzzleConsole.Write($"{label} (default {defaultValue}): ");
            PuzzleConsole.Flush();
            string? input = Console.ReadLine();

            if (input == "exit")
            {
                new ExitMenu().Execute();
            }

            if (string.IsNullOrWhiteSpace(input))
            {
                return defaultValue;
            }

            return input;
        }

        protected static bool IsYes(string? input)
        {
            return input?.Trim().Equals("y", StringComparison.OrdinalIgnoreCase) == true;
        }
    }
}
