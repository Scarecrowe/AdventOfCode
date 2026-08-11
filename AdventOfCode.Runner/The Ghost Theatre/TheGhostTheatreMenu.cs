namespace AdventOfCode.Runner.The_Ghost_Theatre
{
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using AdventOfCode.Animation;
    using AdventOfCode.Animation.Renderers.AsciiRenderer;
    using AdventOfCode.Animation.Renderers.AsciiRenderer.ConsoleMenu;
    using AdventOfCode.Animation.Renderers.ConsoleRenderer.ConsoleMenu;
    using AdventOfCode.Core;
    using AdventOfCode.Core.ConsoleMenu;
    using AdventOfCode.Runner.North_Pole_Operations;

    public class TheGhostTheatreMenu : ConsoleMenu, IConsoleMenu
    {
        public TheGhostTheatreMenu()
            : base("The Ghost Theatre")
        {
        }

        private AnimationType? SelectedAnimationType { get; set; }

        public async Task<IConsoleMenu> Execute()
        {
            while (true)
            {
                IConsoleMenuItem? item = null;

                if (!this.SelectedAnimationType.HasValue)
                {
                    this.AddMainMenuItems();
                    this.SetSubTitle("What type of animation would you like to build?");
                    this.Reset();

                    item = await this.WriteMenu();

                    switch (item.Key)
                    {
                        case GenericMenu.Back:
                            return new NorthPoleOperationsMenu();
                    }

                    this.SelectedAnimationType = (AnimationType)(item?.Index ?? 0);
                    this.PushHistory($"{this.SelectedAnimationType}");
                }

                this.SetSubTitle($"Finding all {this.SelectedAnimationType} animation puzzles");
                this.Reset();

                this.AddAnimationMenuItems(this.SelectedAnimationType.Value);

                this.SetSubTitle($"Which puzzle would you like to render in {this.SelectedAnimationType}?");
                this.Reset();

                if  (this.SelectedAnimationType == AnimationType.Ascii)
                {
                    this.Items.Add(TheGhostTheatreMenuType.RenderAllMp4, "Render All MP4 Animations", "Generate MP4 files for all silver and gold solutions");
                    this.Items.Add(TheGhostTheatreMenuType.RenderAllGif, "Render All GIF Animations", "Generate GIF files for all silver and gold solutions");
                }

                this.AddGenericMenuItems("Select a different animation type");

                item = await this.WriteMenu();
    
                if (item.GetKey<TheGhostTheatreMenuType>() == TheGhostTheatreMenuType.RenderAllMp4)
                {
                    List<Type> puzzles = Animation.GetByType(this.SelectedAnimationType.Value);
                    List<string> report = [];

                    foreach (Type puzzle in puzzles)
                    {
                        int day = Puzzle.GetDayFromType(puzzle);
                        int year = Puzzle.GetYearFromType(puzzle);

                        IAsciiAnimation? animation = Puzzle.GetAnimation<IAsciiAnimation>(year, day);

                        this.PushHistory(animation?.DayTitle!);

                        if (animation == null)
                        {
                            report.Add($"{year} Day {day:00} // Silver // FAILED // Could not create animation");
                            report.Add($"{year} Day {day:00} // Gold   // FAILED // Could not create animation");
                            continue;
                        }

                        this.RenderAndReport(
                            report,
                            SolutionType.Silver,
                            animation,
                            year,
                            day);

                        this.RenderAndReport(
                            report,
                            SolutionType.Gold,
                            animation,
                            year,
                            day);

                        this.PopHistory();
                    }

                    this.Reset();

                    foreach (string line in report)
                    {
                        PuzzleConsole.WriteLine(line);
                    }

                    PuzzleConsole.Flush();

                    Console.ReadKey();

                    break;
                }

                if (item?.Key != null)
                {
                    if(Reflector.IsTuple(item.Key))
                    {
                        (int Year, int Day) key = ((int Year, int Day))item.Key;

                        IConsoleMenu? menu = null;

                        switch (this.SelectedAnimationType)
                        {
                            case AnimationType.TwoDimension:
                                I2dAnimation? animation = Puzzle.GetAnimation<I2dAnimation>(key.Year, key.Day);
                                animation?.Render2DSilver();
                                menu = await new AsciiRendererMenu(this, key.Year, key.Day).Execute();
                                break;
                            case AnimationType.ThreeDimension:
                                break;
                            case AnimationType.Ascii:
                                menu = await new AsciiRendererMenu(this, key.Year, key.Day).Execute();
                                break;
                            case AnimationType.Console:
                                menu = await new ConsoleRendererMenu(this, key.Year, key.Day).Execute();
                                break;
                        }

                        if (menu != null
                            && menu.MainMenu)
                        {
                            break;
                        }
                    }
                    else if(item.GetKey<int>() == GenericMenu.Back)
                    {
                        this.PopHistory();
                        this.SelectedAnimationType = null;
                    }
                    else if (item.GetKey<int>() == GenericMenu.MainMenu)
                    {
                        break;
                    }
                }
            }

            return new NorthPoleOperationsMenu();
        }

        private void RenderAndReport(
            List<string> report,
            SolutionType solutionType,
            IAsciiAnimation animation,
            int year,
            int day)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            try
            {
                this.RenderPuzzle(solutionType, animation, year, day);

                stopwatch.Stop();

                report.Add($"{year} Day {day:00} // {solutionType,-6} // OK     // {stopwatch.Elapsed:hh\\:mm\\:ss\\.fff}");
            }
            catch (Exception ex)
            {
                stopwatch.Stop();

                report.Add($"{year} Day {day:00} // {solutionType,-6} // FAILED // {stopwatch.Elapsed:hh\\:mm\\:ss\\.fff} // {ex.Message}");
            }
        }

        private void RenderPuzzle(SolutionType type, IAsciiAnimation puzzle, int year, int day)
        {
            IAsciiRendererConfiguration configuration = puzzle.AsciiConfiguration();

            this.SetSubTitle($"Rendering {configuration.OutputFormat} puzzle");
            this.Reset();

            AsciiRenderer renderer = new(configuration, (SolutionType)type, year, day);

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
                return;
            }

            PuzzleConsole.WriteLine($"Animation created");
            PuzzleConsole.Flush();
        }

        private void AddMainMenuItems()
        {
            this.Items.Clear();
            this.Items.Add(TheGhostTheatreMenuType.TwoDAnimations, "2D Animations", "Render a puzzle in 2D");
            this.Items.Add(TheGhostTheatreMenuType.ThreeDAnimations, "3D Animations", "Render a puzzle in 3D");
            this.Items.Add(TheGhostTheatreMenuType.AsciiAnimations, "Ascii Animations", "Render a puzzle in Ascii");
            this.Items.Add(TheGhostTheatreMenuType.ConsoleAnimations, "Console Animations", "Step through a puzzle in the console");
            this.AddBackMenuItem("Return to North Pole Operations");
            this.AddExitMenuItem();
        }

        private void AddAnimationMenuItems(AnimationType animationType)
        {
            this.Items.Clear();

            foreach (Type animation in Animation.GetByType(animationType))
            {
                int day = Puzzle.GetDayFromType(animation);
                int year = Puzzle.GetYearFromType(animation);
                IPuzzle? puzzle = Puzzle.GetPuzzle(year, day);

                if (puzzle == null)
                {
                    continue;
                }

                this.Items.Add((year, day), puzzle.DayTitle, $"{year} Day {day}");
            }
        }
    }
}
