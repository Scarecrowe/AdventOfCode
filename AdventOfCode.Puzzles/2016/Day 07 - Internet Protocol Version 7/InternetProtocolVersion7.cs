namespace AdventOfCode.Puzzles._2016.Day_07___Internet_Protocol_Version_7
{
    public class InternetProtocolVersion7
    {
        public InternetProtocolVersion7(string[] input) => this.Addresses = Parse(input);

        public List<InternetAddress> Addresses { get; }

        public int SupportsTls() => this.Addresses.Sum(x => x.SupportsTls() ? 1 : 0);

        public int SupportsSsl() => this.Addresses.Sum(x => x.SupportsSsl() ? 1 : 0);

        private static List<InternetAddress> Parse(string[] input) => input.Select(x => new InternetAddress(x)).ToList();
    }
}
