namespace AdventOfCode.Puzzles._2017.Day_12___Digital_Plumber
{
    public class DigitalPlumber
    {
        public DigitalPlumber(string[] input) => this.Programs = Parse(input);

        public Dictionary<int, Program> Programs { get; private set; }

        public int GroupCountByProgramId(int programId) => this.Programs[programId].Group(this.Programs).Count;

        public int GroupCount()
        {
            var programs = this.Programs.Select(x => x.Value).ToList();
            var result = 0;

            while (programs.Any())
            {
                HashSet<int> group = programs[0].Group(this.Programs);
                programs.RemoveAll(x => group.Contains(x.Id));
                result++;
            }

            return result;
        }

        private static Dictionary<int, Program> Parse(string[] input) => input.Select(x => new Program(x)).ToDictionary(key => key.Id, value => value);
    }
}
