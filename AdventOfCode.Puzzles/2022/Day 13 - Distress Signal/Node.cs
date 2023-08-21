namespace AdventOfCode.Puzzles._2022.Day_13___Distress_Signal
{
    using System.Text;

    public class Node
    {
        public Node(Node? parent)
        {
            this.Value = -1;
            this.Parent = parent;
            this.Children = new();
            this.IsList = true;
        }

        public Node(Node parent, int value)
        {
            this.Value = value;
            this.Parent = parent;
            this.Children = new();
        }

        public Node? Parent { get; private set; }

        public List<Node> Children { get; private set; }

        public int Value { get; private set; }

        public bool IsList { get; private set; }

        public new string ToString()
        {
            StringBuilder sb = new();

            this.BuildString(sb, this);

            return sb.ToString();
        }

        private void BuildString(StringBuilder sb, Node node)
        {
            if (sb.Length > 0 && sb[^1] == ']')
            {
                sb.Append(',');
            }

            sb.Append('[');

            foreach (Node child in node.Children)
            {
                if (child.IsList)
                {
                    this.BuildString(sb, child);
                }
                else
                {
                    if (sb[^1] == ']')
                    {
                        sb.Append(',');
                    }

                    sb.Append(child.Value);

                    if (child != node.Children.Last())
                    {
                        sb.Append(',');
                    }
                }
            }

            sb.Append(']');
        }
    }
}
