namespace AdventOfCode.Animation.Renderers.ConsoleRenderer
{
    using AdventOfCode.Animation.Renderers.Configuration;
    using AdventOfCode.Core;

    public class ConsoleRenderer : IConsoleRenderer
    {
        public ConsoleRenderer(
            IConsoleRendererConfiguration configuration,
            SolutionType solutionType,
            int year,
            int day)
        {
            this.SolutionType = solutionType;
            this.Configuration = configuration;
            this.Year = year;
            this.Day = day;
            this.DayTitle = configuration.Title;
            this.Frames = new Frames();
        }

        public IConsoleRendererConfiguration Configuration { get; set; }

        public IFrames Frames { get; }

        public SolutionType SolutionType { get; }

        public int Year { get; }

        public int Day { get; }

        public string DayTitle { get; }

        private bool AutoPlay { get; set; }

        public void Finish()
        {
            throw new NotImplementedException();
        }

        public void Render()
        {
            throw new NotImplementedException();
        }

        public void RenderFrame(IFrame frame)
        {
            while(true)
            {
                Console.Clear();

                foreach (string row in frame.Data)
                {
                    for(int i = 0; i < row.Length; i++)
                    {
                        if (this.Configuration != null
                           && this.Configuration.Characters != null
                           && this.Configuration.Characters.Any())
                        {
                            ICharacterConfiguration? configuration = this.Configuration.Characters.GetByCharacter(row[i]);

                            if (configuration != null)
                            {
                                PuzzleConsole.Write(configuration.Replace);
                            }
                            else
                            {
                                PuzzleConsole.Write(row[i]);
                            }
                        }
                        else
                        {
                            PuzzleConsole.Write(row[i]);
                        }
                    }

                    PuzzleConsole.WriteLine();
                }

                PuzzleConsole.WriteLine();

                if (this.AutoPlay)
                {
                    PuzzleConsole.Flush();
                    Thread.Sleep(50);
                    return;
                }

                PuzzleConsole.WriteLine("1. Next frame");
                PuzzleConsole.WriteLine("2. Previous frame");
                PuzzleConsole.WriteLine("3. Auto play");

                PuzzleConsole.Flush();

                string? result = Console.ReadLine();

                bool isNumber = int.TryParse(result, out int number);

                if (isNumber)
                {
                    switch (number)
                    {
                        case 1:
                            return;
                        case 2:
                            break;
                        case 3:
                            this.AutoPlay = true;
                            return;
                    }
                }
                else
                {
                    PuzzleConsole.WriteLine("Invalid selection");
                    Console.ReadKey();
                }
            }
        }
    }
}
