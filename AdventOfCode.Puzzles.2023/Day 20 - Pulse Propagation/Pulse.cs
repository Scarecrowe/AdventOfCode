namespace AdventOfCode.Puzzles._2023.Day_20___Pulse_Propagation
{
    public class Pulse
    {
        public string Source { get; }

        public string Target { get; }

        public int Value { get; }

        public Pulse(string source, string target, int value)
        {
            Source = source;
            Target = target;
            Value = value;
        }
    }
}
