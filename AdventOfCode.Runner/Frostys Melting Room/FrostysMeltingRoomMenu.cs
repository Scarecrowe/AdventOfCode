namespace AdventOfCode.Runner.Frostys_Melting_Room
{
    using AdventOfCode.Core;
    using AdventOfCode.Core.ConsoleMenu;
    using AdventOfCode.Runner.North_Pole_Operations;

    public class FrostysMeltingRoomMenu : ConsoleMenu, IConsoleMenu
    {
        private static readonly string AnimationsPath =
            Path.Combine(AppContext.BaseDirectory, "Animations");

        private static readonly string RunsPath =
            Path.Combine(AppContext.BaseDirectory, "Runs");

        public FrostysMeltingRoomMenu()
            : base("Frosty's Melting Room")
        {
            this.Items.Add(FrostysMeltingRoomMenuType.CleanGeneratedAnimations, "Clean Generated Animations", "Delete rendered animation files");
            this.Items.Add(FrostysMeltingRoomMenuType.CleanSavedRuns, "Clean Saved Runs", "Delete archived puzzle runs");
            this.Items.Add(FrostysMeltingRoomMenuType.CleanEverything, "Clean Everything", "Delete animations and archived runs");

            this.AddBackMenuItem("Return to North Pole Operations");
            this.AddExitMenuItem();
        }

        public async Task<IConsoleMenu> Execute()
        {
            this.Reset();

            IConsoleMenuItem? item = await this.WriteMenu();

            switch (item?.Key)
            {
                case FrostysMeltingRoomMenuType.CleanGeneratedAnimations:
                    this.Reset();

                    if (PromptBool("Are you sure you want to delete generated animations? (y/n): "))
                    {
                        CleanGeneratedAnimations();
                    }

                    this.WaitForUser();
                    break;

                case FrostysMeltingRoomMenuType.CleanSavedRuns:
                    this.Reset();

                    if (PromptBool("Are you sure you want to delete saved runs? (y/n): "))
                    {
                        CleanSavedRuns();
                    }

                    this.WaitForUser();
                    break;

                case FrostysMeltingRoomMenuType.CleanEverything:
                    this.Reset();

                    if (PromptBool("Are you sure you want to delete generated animations and saved runs? (y/n): "))
                    {
                        CleanGeneratedAnimations();
                        CleanSavedRuns();
                    }

                    this.WaitForUser();
                    break;

                case GenericMenu.Back:
                case GenericMenu.MainMenu:
                    return new NorthPoleOperationsMenu();

                case GenericMenu.Exit:
                    return await new ExitMenu().Execute();
            }

            return new FrostysMeltingRoomMenu();
        }

        private static void CleanGeneratedAnimations()
        {
            DeleteFiles(AnimationsPath, "animation");
        }

        private static void CleanSavedRuns()
        {
            DeleteFiles(RunsPath, "run");
        }

        private static void DeleteFiles(
            string path,
            string description)
        {
            if (!Directory.Exists(path))
            {
                PuzzleConsole.WriteLine($"No {description} folder found.");
                PuzzleConsole.Flush();
                return;
            }

            string[] files = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);

            foreach (string file in files)
            {
                File.Delete(file);
            }

            PuzzleConsole.WriteLine($"Cleaned {files.Length:N0} {description} file(s).");
            PuzzleConsole.Flush();
        }
    }
}