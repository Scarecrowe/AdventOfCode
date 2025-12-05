namespace AdventOfCode.Puzzles._2021.Day_10___Syntax_Scoring
{
    public class Node
    {
        public Node(char value, Node? parent)
        {
            this.Illegalvalues = new();

            if (SyntaxScoring.IsOpening(value))
            {
                this.Illegalvalues.AddRange(SyntaxScoring.Not(SyntaxScoring.Invert(value)));

                if (parent != null)
                {
                    char inverted = SyntaxScoring.Invert(parent.Value);

                    if (!this.Illegalvalues.Contains(inverted) && parent.Value != value)
                    {
                        this.Illegalvalues.Add(inverted);
                    }
                }
            }

            this.Value = value;
            this.Parent = parent;
        }

        public char Value { get; }

        public Node? Parent { get; }

        public Node? Child { get; private set; }

        public List<char> Illegalvalues { get; private set; }

        public Node AddChild(char value)
        {
            this.Child = new(value, this);

            return this.Child;
        }

        public void RemoveChild() => this.Child = null;
    }
}
