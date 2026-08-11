namespace AdventOfCode.Runner.The_Archivists_Library
{
    using AdventOfCode.Core;
    using AdventOfCode.Core.ConsoleMenu;
    using AdventOfCode.Runner.North_Pole_Operations;

    public class TheArchivistsLibraryMenu : ConsoleMenu, IConsoleMenu
    {
        private static readonly string RunsPath = Path.Combine(AppContext.BaseDirectory, "Runs");

        public TheArchivistsLibraryMenu()
            : base("The Archivist's Library")
        {
            this.AddMenuItems();
        }

        public async Task<IConsoleMenu> Execute()
        {
            while (true)
            {
                this.Reset();

                IConsoleMenuItem? item = await this.WriteMenu();

                if (item == null)
                {
                    return new TheArchivistsLibraryMenu();
                }

                if (item.GetKey<int>() == GenericMenu.Back ||
                    item.GetKey<int>() == GenericMenu.MainMenu)
                {
                    return new NorthPoleOperationsMenu();
                }

                if (item.GetKey<int>() == GenericMenu.Exit)
                {
                    return await new ExitMenu().Execute();
                }

                if (item.Key is string filePath)
                {
                    this.ViewRun(filePath);
                    this.WaitForUser();
                }
            }
        }

        private void AddMenuItems()
        {
            this.Items.Clear();

            if (!Directory.Exists(RunsPath))
            {
                this.Items.Add(TheArchivistsLibraryMenuType.NoRunsFound, "No Past Runs Found", "Santa's Gauntlet has not saved any runs yet");
                this.AddGenericMenuItems("Return to North Pole Operations");
                return;
            }

            string[] files = Directory
                .GetFiles(RunsPath, "*.txt", SearchOption.TopDirectoryOnly)
                .OrderByDescending(File.GetCreationTime)
                .ToArray();

            if (files.Length == 0)
            {
                this.Items.Add(TheArchivistsLibraryMenuType.NoRunsFound, "No Past Runs Found", "Santa's Gauntlet has not saved any runs yet");
                this.AddGenericMenuItems("Return to North Pole Operations");
                return;
            }

            foreach (string file in files)
            {
                FileInfo info = new(file);

                this.Items.Add(
                    file,
                    Path.GetFileNameWithoutExtension(file),
                    $"{info.CreationTime:yyyy-MM-dd HH:mm:ss} // {info.Length:N0} bytes");
            }

            this.AddBackMenuItem("Return to North Pole Operations");
            this.AddExitMenuItem();
        }

        private void ViewRun(string filePath)
        {
            this.SetSubTitle(Path.GetFileNameWithoutExtension(filePath));
            this.Reset();

            if (!File.Exists(filePath))
            {
                PuzzleConsole.WriteLine("The Archivist could not find that run.");
                PuzzleConsole.Flush();
                return;
            }

            foreach (string line in File.ReadAllLines(filePath))
            {
                PuzzleConsole.WriteLine(line);
            }

            PuzzleConsole.Flush();
        }
    }
}