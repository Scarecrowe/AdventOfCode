namespace AdventOfCode.Puzzles._2021.Day_12___Passage_Pathing
{
    public class Node
    {
        public Node(string value, Node? parent)
        {
            this.Value = value;
            this.Parent = parent;
            this.Children = new();
        }

        public string Value { get; }

        public Node? Parent { get; }

        public List<Node> Children { get; }

        public Node AddChild(string value)
        {
            Node result = new(value, this);

            this.Children.Add(result);

            return result;
        }

        public void AddChildren(string[] values)
        {
            foreach (string value in values)
            {
                Node result = new(value, this);

                this.Children.Add(result);
            }
        }
    }
}
