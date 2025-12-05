namespace AdventOfCode.Puzzles._2018.Day_20___A_Regular_Map
{
    using AdventOfCode.Core;

    public class ARegularMap
    {
        public ARegularMap(string[] input)
        {
            this.Rule = input[0];
            this.Map = new();
        }

        private string Rule { get; }

        private VectorDictionary<int, Node> Map { get; }

        public Node Get(Vector<int> point)
        {
            this.Map.TryGetValue(point, out Node? result);

            if (result != null)
            {
                return result;
            }

            result = new(point, int.MaxValue);
            this.Map.Add(point, result);

            return result;
        }

        public Node AddNode(int dx, int dy, Node currentNode)
        {
            Node node = this.Get(new(currentNode.Point.X + dx, currentNode.Point.Y + dy));
            node.Distance = Math.Min(node.Distance, currentNode.Distance + 1);
            return node;
        }

        public int Furthest() => this.Map.Max(x => x.Value.Distance);

        public int Shortest() => this.Map.Count(x => x.Value.Distance >= 1000);

        public ARegularMap BuildMap()
        {
            Node node = new(new(0, 0), 0);
            Stack<Node> stack = new();
            stack.Push(node);

            foreach (char character in this.Rule)
            {
                switch (character)
                {
                    case 'N': node = this.AddNode(0, -1, node); break;
                    case 'S': node = this.AddNode(0, 1, node); break;
                    case 'E': node = this.AddNode(1, 0, node); break;
                    case 'W': node = this.AddNode(-1, 0, node); break;
                    case '(': stack.Push(node); break;
                    case ')': node = stack.Pop(); break;
                    case '|': node = stack.First(); break;
                    default: break;
                }
            }

            return this;
        }
    }
}
