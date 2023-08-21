namespace AdventOfCode.Puzzles._2018.Day_04___Repose_Record
{
    using AdventOfCode.Core.Extensions;

    public class Log
    {
        public Log(string line)
        {
            this.TimeStamp = line[1..line.IndexOf(']')].ToDateTime();

            line = line[(line.IndexOf(']') + 2) ..];

            if (line.StartsWith("Guard"))
            {
                this.Guard = line.Replace("Guard #").Replace(" begins shift").ToInt();
                this.Action = ActionType.BeginShift;
            }
            else if (line == "falls asleep")
            {
                this.Action = ActionType.FallAsleep;
            }
            else
            {
                this.Action = ActionType.WakeUp;
            }
        }

        public DateTime TimeStamp { get; }

        public int Guard { get; }

        public ActionType Action { get; }
    }
}
