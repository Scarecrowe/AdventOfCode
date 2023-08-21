namespace AdventOfCode.Puzzles._2019.Day_14___Space_Stoichiometry
{
    public class Topological
    {
        public Topological(Dictionary<string, Reaction> reaction)
        {
            this.DepthFirstOrder = new List<string>(reaction.Count);
            this.Marked = new HashSet<string>(reaction.Count);

            foreach (string item in reaction.Keys)
            {
                if (!this.Marked.Contains(item))
                {
                    this.DepthFirstSearch(reaction, item);
                }
            }
        }

        private List<string> DepthFirstOrder { get; }

        private HashSet<string> Marked { get; }

        public IEnumerable<string> GetOrdered() => this.DepthFirstOrder;

        private void DepthFirstSearch(Dictionary<string, Reaction> reaction, string start)
        {
            this.Marked.Add(start);

            foreach (var item in reaction[start].GetProducts())
            {
                if (!this.Marked.Contains(item.Key))
                {
                    this.DepthFirstSearch(reaction, item.Key);
                }
            }

            this.DepthFirstOrder.Add(start);
        }
    }
}
