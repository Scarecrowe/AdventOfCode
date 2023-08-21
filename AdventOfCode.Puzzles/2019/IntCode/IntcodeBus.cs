namespace AdventOfCode.Puzzles._2019.IntCode
{
    public class IntcodeBus : Queue<long>
    {
        public void EnqueueRange(List<long> values)
        {
            foreach (long value in values)
            {
                this.Enqueue(value);
            }
        }

        public void EnqueueRange(List<int> values) => this.EnqueueRange(values.Select(c => (long)c).ToList());
    }
}
