namespace AdventOfCode.Animation.Renderers.AsciiRenderer.ConsoleMenu
{
    using System.Diagnostics;
    using System.Threading.Tasks;
    using AdventOfCode.Core;
    using AdventOfCode.Core.ConsoleMenu;

    public class AsciiRendererViewMenu : ConsoleMenu, IConsoleMenu
    {
        public AsciiRendererViewMenu(
            IConsoleMenu parent,
            IAsciiRendererConfiguration? configuration,
            int year,
            int day)
            : base(parent, string.Empty)
        {
            this.Year = year;
            this.Day = day;
            this.Configuration = configuration;
            this.PushHistory("View");
            this.SetTitle(Puzzle.GetTitle(this.Year, this.Day));
        }

        public IAsciiRendererConfiguration? Configuration { get; }

        public int Year { get; private set; }

        public int Day { get; private set; }

        public async Task<IConsoleMenu> Execute()
        {
            while (true)
            {
                this.Reset();
                this.ViewMenu();

                IConsoleMenuItem? selection = await this.WriteMenu();

                switch (selection.Key)
                {
                    case GenericMenu.Back:
                        break;
                    case GenericMenu.MainMenu:
                        this.Parent.GotoMainMenu();
                        break;
                    default:
                        Process.Start(new ProcessStartInfo
                        {
                            FileName = selection.GetKey<string>(),
                            UseShellExecute = true
                        });

                        continue;
                }

                break;
            }

            this.PopHistory();

            return this.Parent;
        }

        private void ViewMenu()
        {
            this.Items.Clear();

            string outputPath = Animation.GetRenderPath(this.Year, this.Day, this.Title);

            string[] files = Directory.GetFiles(outputPath);

            foreach (string file in files)
            {
                string ext = Path.GetExtension(file);

                if (ext == ".mp4"
                    || ext == ".gif")
                {
                    string name = Path.GetFileNameWithoutExtension(file);

                    string puzzleName = name.Split("-")[1];

                    puzzleName =
                        char.ToUpper(puzzleName[0]) +
                        puzzleName[1..];

                    string formattedExt =
                        char.ToUpper(ext[1]) +
                        ext[2..];

                    this.Items.Add(
                        file,
                        $"{puzzleName} {formattedExt}",
                        string.Empty);
                }
            }

            this.AddGenericMenuItems("Return to this puzzles ascii render menu");
        }
    }
}
