namespace AdventOfCode.Runner
{
    using AdventOfCode.Core.Extensions;

    public class CommandArguments : ICommandArguments
    {
        public CommandArguments(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                switch (args[i])
                {
                    case "-year":
                        this.Year = this.ParseArgument(args, i);
                        i++;
                        break;
                    case "-day":
                        this.Day = this.ParseArgument(args, i);
                        i++;
                        break;
                    case "-batch":
                        this.BatchCount = this.ParseArgument(args, i);
                        i++;
                        break;
                }
            }

            if (this.BatchCount == 0)
            {
                this.BatchCount = 1;
            }
        }

        public int Year { get; }

        public int Day { get; }

        public int BatchCount { get; }

        public bool Valid
        {
            get
            {
                return !((this.Year < 2015 || this.Year > 2022)
                    || (this.Day < 1 || this.Day > 25));
            }
        }

        private int ParseArgument(string[] args, int i)
        {
            int value = 0;

            if (i + 1 >= args.Length
            || !int.TryParse(args[i + 1], out value))
            {
                throw new ArgumentException($"Invalid ${args[i].Replace("-")} command argument");
            }

            return value;
        }
    }
}
