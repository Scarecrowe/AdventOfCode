namespace AdventOfCode.Runner
{
    using AdventOfCode.Core.Extensions;

    public class CommandArguments : ICommandArguments
    {
        public CommandArguments(string[] args)
        {
            if (args.Length != 4)
            {
                Console.WriteLine("Invalid command arguments length");
                this.Valid = false;
                return;
            }

            this.Valid = true;
            this.Year = args[1].ToInt();
            this.Day = args[2].ToInt();
            this.BatchCount = args[3].ToInt();
        }

        public int Year { get; }

        public int Day { get; }

        public int BatchCount { get; }

        public bool Valid { get; }
    }
}
