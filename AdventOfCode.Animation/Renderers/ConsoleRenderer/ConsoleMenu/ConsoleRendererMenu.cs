namespace AdventOfCode.Animation.Renderers.ConsoleRenderer.ConsoleMenu
{
    using System.Threading.Tasks;
    using AdventOfCode.Animation.Renderers.AsciiRenderer.ConsoleMenu;
    using AdventOfCode.Core;
    using AdventOfCode.Core.ConsoleMenu;
    using AdventOfCode.Core.ConsoleMenu.Configuration;

    public class ConsoleRendererMenu : ConsoleMenu, IConsoleMenu
    {
        public ConsoleRendererMenu(IConsoleMenu parent, int year, int day)
            : base(parent, string.Empty)
        {
            this.Year = year;
            this.Day = day;
            this.Animation = Puzzle.GetAnimation<IConsoleAnimation>(this.Year, this.Day);
            this.Configuration = this.Animation?.ConsoleConfiguration();
            this.PushHistory($"{this.Year} Day {this.Day} - {this.Animation?.DayTitle}");
        }

        public IConsoleAnimation? Animation { get; }

        public int Year { get; }

        public int Day { get; }

        public IConsoleRendererConfiguration? Configuration { get; }

        public async Task<IConsoleMenu> Execute()
        {
            while (true)
            {
                this.CreateMenuItems();

                if (this.Animation == null
                    || this.Items == null)
                {
                    break;
                }

                this.Reset();
                IConsoleMenuItem? option = await this.WriteMenu();

                if (option == null)
                {
                    break;
                }

                IConsoleMenu? menu;

                switch (option.Key)
                {
                    case ConsoleRendererMenuType.RenderSilver:
                    case ConsoleRendererMenuType.RenderGold:
                        SolutionType solutionType = (SolutionType)option.Key;
                        this.PushHistory($"Render {solutionType}");
                        this.RenderPuzzle(solutionType);
                        this.PopHistory();
                        continue;
                    case ConsoleRendererMenuType.Configuration:
                        if (this.Configuration == null)
                        {
                            break;
                        }

                        menu = await new ConfigurationMenu(this, "Configuration", this.Configuration).Execute();

                        if (menu.MainMenu)
                        {
                            this.Parent.GotoMainMenu();
                            return this.Parent;
                        }

                        continue;
                    case GenericMenu.Back:
                        break;
                    case GenericMenu.MainMenu:
                        this.Parent?.GotoMainMenu();
                        break;
                    default:
                        break;
                }

                break;
            }

            return this.Parent!;
        }

        private void CreateMenuItems()
        {
            this.Items.Clear();
            this.Items.Add(ConsoleRendererMenuType.RenderSilver, "Render Silver", "Render the silver solution");
            this.Items.Add(ConsoleRendererMenuType.RenderGold, "Render Gold", "Render the gold solution");
            this.Items.Add(ConsoleRendererMenuType.Configuration, "Configuration", "Change the way this puzzle is animated");
            this.AddGenericMenuItems("Select a different puzzle");
        }

        private void RenderPuzzle(SolutionType type)
        {
            IAsciiAnimation? puzzle = Puzzle.GetAnimation<IAsciiAnimation>(this.Year, this.Day);

            if (this.Configuration == null
                || puzzle == null)
            {
                return;
            }

            ////this.SetSubTitle($"Rendering {this.Configuration.OutputFormat} puzzle");
            this.Reset();

            ConsoleRenderer renderer = new(this.Configuration, (SolutionType)type, this.Year, this.Day);

            try
            {
                if (type == SolutionType.Silver)
                {
                    puzzle.SilverFrame(renderer);
                }
                else
                {
                    puzzle.GoldFrame(renderer);
                }

                renderer.Finish();
            }
            catch (NotImplementedException)
            {
                PuzzleConsole.WriteLine($"Rendering disabled");
                PuzzleConsole.WriteLine($"To prevent Christmas Tree defoliation");
                PuzzleConsole.WriteLine();
                PuzzleConsole.WriteLine("Press any key to continue");
                PuzzleConsole.Flush();
                Console.ReadKey();
                return;
            }

            PuzzleConsole.WriteLine();
            PuzzleConsole.WriteLine();
            PuzzleConsole.Flush();

            renderer.Render();
            
            PuzzleConsole.Flush();

            if (PromptBool("Would you like to view this anitmation? (y,n): "))
            {
            }
        }
    }
}
