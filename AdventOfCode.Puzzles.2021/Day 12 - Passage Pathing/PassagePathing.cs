namespace AdventOfCode.Puzzles._2021.Day_12___Passage_Pathing
{
    using AdventOfCode.Core;

    public class PassagePathing
    {
        public PassagePathing(string[] input) => this.Links = ParseInput(input);

        public List<Link> Links { get; }

        public Node? Tree { get; private set; }

        public static int HasParent(Node node, string value)
        {
            int total = 0;

            while (true)
            {
                if (node.Value == value)
                {
                    total++;
                }

                if (node.Parent == null)
                {
                    break;
                }

                node = node.Parent;
            }

            return total;
        }

        public PassagePathing BuildTree(string multipleCave = "", bool multipleVisits = false)
        {
            this.Tree = null;
            this.BuildTreeRecursive(this.Tree, "start", multipleCave, multipleVisits);

            return this;
        }

        public int UniquePathCount(bool multipleVisits = false)
        {
            int total = 0;

            List<string> smallCaves = this.GetSmallCaves().Distinct().ToList();
            smallCaves.Remove("start");
            smallCaves.Remove("end");

            List<string> unique = new();

            foreach (string cave in smallCaves)
            {
                this.BuildTree(cave, multipleVisits);

                total += this.UniquePaths(unique);
            }

            return total;
        }

        public int UniquePaths(List<string> unique)
        {
            int total = 0;
            this.TraverseUniquePaths(this.Tree, unique, string.Empty, ref total);

            return total;
        }

        private static bool IsAllUpper(string input)
        {
            for (int i = 0; i < input.Length; i++)
            {
                if (!char.IsUpper(input[i]))
                {
                    return false;
                }
            }

            return true;
        }

        private static List<Link> ParseInput(string[] input)
        {
            List<Link> links = new();
            List<Link> result = links;

            foreach (string line in input)
            {
                string[] tokens = line.Split("-");

                result.Add(new(tokens[0], tokens[1]));
            }

            return result;
        }

        private List<Link> GetLinks(string value) => this.Links.Where(x => x.A == value || x.B == value).ToList();

        private void TraverseUniquePaths(Node? current, List<string> unique, string value, ref int total)
        {
            string run = value += $"{current?.Value}-";

            if (current?.Value == "end")
            {
                if (!unique.Contains(value))
                {
                    unique.Add(value);
                    total++;
                }
            }

            foreach (Node child in current?.Children ?? new())
            {
                this.TraverseUniquePaths(child, unique, run, ref total);
            }
        }

        private List<string> GetSmallCaves()
        {
            List<string> result = new();

            foreach (Link link in this.Links)
            {
                if (!IsAllUpper(link.A))
                {
                    result.Add(link.A);
                }

                if (!IsAllUpper(link.B))
                {
                    result.Add(link.B);
                }
            }

            return result;
        }

        private void BuildTreeRecursive(Node? current, string value, string multipleCave, bool multipleVisits)
        {
            List<Link> nodes = this.GetLinks(value);

            if (current == null)
            {
                current = new Node(value, null);
                this.Tree = current;
            }

            string[] joins = nodes.Select(x => x.Not(value)).ToArray();

            foreach (string join in joins)
            {
                int searchCount = (multipleVisits && !string.IsNullOrEmpty(multipleCave) && join == multipleCave) ? 1 : 0;

                if (!IsAllUpper(join) && HasParent(current, join) > searchCount)
                {
                    continue;
                }

                Node node = current.AddChild(join);

                this.BuildTreeRecursive(node, node.Value, multipleCave, multipleVisits);
            }
        }
    }
}
