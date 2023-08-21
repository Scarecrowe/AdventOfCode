namespace AdventOfCode.Puzzles._2018.Day_08___Memory_Maneuver
{
    public class MemoryNode
    {
        public MemoryNode(MemoryNode? parent, char letter, int childrenCount, int metadataCount)
        {
            this.Letter = letter;
            this.Parent = parent;
            this.Children = new();
            this.Metadata = new();
            this.ChildrenCount = childrenCount;
            this.MetadataCount = metadataCount;
        }

        public char Letter { get; }

        public MemoryNode? Parent { get; }

        public Dictionary<int, MemoryNode> Children { get; }

        public List<int> Metadata { get; }

        public int ChildrenCount { get; }

        public int MetadataCount { get; }

        public MemoryNode AddNode(MemoryNode node)
        {
            this.Children.Add(this.Children.Count, node);

            return node;
        }

        public new string ToString() => this.Letter.ToString();
    }
}
