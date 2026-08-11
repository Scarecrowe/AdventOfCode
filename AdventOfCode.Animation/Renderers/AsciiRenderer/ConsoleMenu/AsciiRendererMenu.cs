namespace AdventOfCode.Animation.Renderers.AsciiRenderer.ConsoleMenu
{
    using System.Diagnostics;
    using System.Threading.Tasks;
    using AdventOfCode.Core;
    using AdventOfCode.Core.ConsoleMenu;
    using AdventOfCode.Core.ConsoleMenu.Configuration;

    public class AsciiRendererMenu : ConsoleMenu, IConsoleMenu
    {
        public AsciiRendererMenu(IConsoleMenu parent, int year, int day)
            : base(parent, string.Empty)
        {
            this.Year = year;
            this.Day = day;
            this.Animation = Puzzle.GetAnimation<IAsciiAnimation>(this.Year, this.Day);
            this.Configuration = this.Animation?.AsciiConfiguration();
            this.PushHistory($"{this.Year} Day {this.Day} - {this.Animation?.DayTitle}");
        }

        public IAsciiAnimation? Animation { get; }

        public int Year { get; }

        public int Day { get; }

        public IAsciiRendererConfiguration? Configuration { get; }

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
                    case AsciiRendererMenuType.RenderSilver:
                    case AsciiRendererMenuType.RenderGold:
                        SolutionType solutionType = (SolutionType)option.Key;
                        this.PushHistory($"Render {solutionType}");
                        this.RenderPuzzle(solutionType);
                        this.PopHistory();
                        continue;
                    case AsciiRendererMenuType.View:
                        menu = await new AsciiRendererViewMenu(this, this.Configuration, this.Year, this.Day).Execute();

                        if (menu.MainMenu)
                        {
                            this.Parent.GotoMainMenu();
                            break;
                        }

                        this.PopHistory();
                        continue;
                    case AsciiRendererMenuType.Explorer:
                        Process.Start(new ProcessStartInfo
                        {
                            FileName = "explorer.exe",
                            Arguments = AdventOfCode.Animation.Animation.GetRenderPath(this.Year, this.Day, this.Animation?.DayTitle ?? string.Empty),
                            UseShellExecute = true
                        });
                        continue;
                    case AsciiRendererMenuType.Configuration:
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
            this.Items.Add(AsciiRendererMenuType.RenderSilver, "Render Silver", "Render the silver solution");
            this.Items.Add(AsciiRendererMenuType.RenderGold, "Render Gold", "Render the gold solution");
            this.Items.Add(AsciiRendererMenuType.View, "View", "View previous renders");
            this.Items.Add(AsciiRendererMenuType.Explorer, "Explorer", "Open output folder");
            this.Items.Add(AsciiRendererMenuType.Configuration, "Configuration", "Change the way this puzzle is animated");
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

            this.SetSubTitle($"Rendering {this.Configuration.OutputFormat} puzzle");
            this.Reset();

            AsciiRenderer renderer = new(this.Configuration, (SolutionType)type, this.Year, this.Day);
            renderer.SetTimeout(TimeSpan.FromMinutes(5));

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
            catch(NotImplementedException)
            {
                PuzzleConsole.WriteLine($"Rendering disabled");
                PuzzleConsole.WriteLine($"To prevent Christmas Tree defoliation");
                PuzzleConsole.WriteLine();
                PuzzleConsole.WriteLine("Press any key to continue");
                PuzzleConsole.Flush();
                Console.ReadKey();
                return;
            }
            catch
            {
                renderer.Finish();
                throw;
            }

            PuzzleConsole.WriteLine();
            PuzzleConsole.WriteLine();
            PuzzleConsole.Flush();

            renderer.Render();

            string outputPath = $"{AdventOfCode.Animation.Animation.GetRenderPath(this.Year, this.Day, this.Animation?.DayTitle ?? string.Empty)}\\ascii-{(SolutionType)type}.{this.Configuration.OutputFormat}".ToLower();

            PuzzleConsole.WriteLine($"Animation created: {outputPath}");
            PuzzleConsole.Flush();

            if (PromptBool("Would you like to view this anitmation? (y,n): "))
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = outputPath,
                    UseShellExecute = true
                });
            }
        }
    }
}
