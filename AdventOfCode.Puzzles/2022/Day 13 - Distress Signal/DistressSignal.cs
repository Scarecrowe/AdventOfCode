namespace AdventOfCode.Puzzles._2022.Day_13___Distress_Signal
{
    using AdventOfCode.Core.Extensions;

    public class DistressSignal
    {
        public DistressSignal(string[] input) => this.Nodes = Parse(input);

        private List<Node> Nodes { get; set; }

        public int SumOfIndices()
        {
            int result = 0;

            for (int i = 0; i < this.Nodes.Count; i += 2)
            {
                if (this.CompareList(this.Nodes[i].Children, this.Nodes[i + 1].Children) == CompareResult.Less)
                {
                    result += (i / 2) + 1;
                }
            }

            return result;
        }

        public int DecoderKey()
        {
            int result = 1;

            Node? dividerA = ParseNode("[[2]]");
            Node? dividerB = ParseNode("[[6]]");

            this.Nodes.Add(dividerA ?? new(null));
            this.Nodes.Add(dividerB ?? new(null));

            this.Nodes.Sort((Node left, Node right) => (int)this.CompareList(left.Children, right.Children));

            for (int i = 0; i < this.Nodes.Count; i++)
            {
                if (this.Nodes[i] == dividerA || this.Nodes[i] == dividerB)
                {
                    result *= i + 1;
                }
            }

            return result;
        }

        private static List<Node> Parse(string[] input) => input.Select(x => ParseNode(x) ?? new Node(null)).ToList();

        private static Node? ParseNode(string value)
        {
            Node? current = new(null);

            for (int i = 0; i < value.Length; i++)
            {
                switch (value[i])
                {
                    case '[':
                        Node child = new(current);
                        current?.Children.Add(child);
                        current = child;
                        break;
                    case ']':
                        current = current?.Parent;
                        break;
                    case ',':
                        break;
                    default:
                        string number = $"{value[i]}";

                        if (char.IsNumber(value[i + 1]))
                        {
                            number = $"{number}{value[i + 1]}";
                            i++;
                        }

                        current?.Children.Add(new(current, number.ToInt()));
                        break;
                }
            }

            return current?.Children.First();
        }

        private static CompareResult CompareValue(Node a, Node b)
        {
            if (a.Value < b.Value)
            {
                return CompareResult.Less;
            }

            if (a.Value > b.Value)
            {
                return CompareResult.Greater;
            }

            return CompareResult.Equal;
        }

        private CompareResult CompareList(List<Node> left, List<Node> right)
        {
            for (int i = 0; i < left.Count && i < right.Count; i++)
            {
                Node a = left[i];
                Node b = right[i];

                if (!a.IsList && !b.IsList)
                {
                    CompareResult result = CompareValue(a, b);

                    if (result != CompareResult.Equal)
                    {
                        return result;
                    }
                }
                else
                {
                    CompareResult result = this.CompareList(a.IsList ? a.Children : new() { a }, b.IsList ? b.Children : new() { b });

                    if (result != CompareResult.Equal)
                    {
                        return result;
                    }
                }
            }

            if (left.Count < right.Count)
            {
                return CompareResult.Less;
            }

            if (left.Count > right.Count)
            {
                return CompareResult.Greater;
            }

            return CompareResult.Equal;
        }
    }
}
