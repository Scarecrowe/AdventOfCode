namespace AdventOfCode.Puzzles._2018.Day_08___Memory_Maneuver
{
    using AdventOfCode.Core.Extensions;

    public class MemoryManeuver
    {
        public MemoryManeuver(string input) => this.Values = input.SplitSpace().ToInt();

        public int[] Values { get; }

        public MemoryNode? Tree { get; private set; }

        public int MetadataSum { get; private set; }

        public MemoryManeuver BuildTree()
        {
            int letter = 'A';
            this.Tree = new(null, (char)letter, this.Values[0], this.Values[1]);
            MemoryParseType parser = this.Tree.ChildrenCount == 0 ? MemoryParseType.MetaData : MemoryParseType.Header;
            int index = 2;
            int metaIndex = 0;
            MemoryNode? current = this.Tree;

            while (index < this.Values.Length)
            {
                if (parser == MemoryParseType.Header)
                {
                    current = current.AddNode(new(current, (char)++letter, this.Values[index], this.Values[index + 1]));
                    parser = current.ChildrenCount == 0 ? MemoryParseType.MetaData : MemoryParseType.Header;
                    index += 2;
                }
                else
                {
                    this.MetadataSum += this.Values[index];
                    current.Metadata.Add(this.Values[index]);
                    metaIndex++;
                    index++;

                    if (metaIndex >= current.MetadataCount)
                    {
                        current = current.Parent;

                        if (current == null)
                        {
                            break;
                        }

                        parser = current.Children.Count == current.ChildrenCount ? MemoryParseType.MetaData : MemoryParseType.Header;
                        metaIndex = 0;
                    }
                }
            }

            return this;
        }

        public int RootValue(MemoryNode? current = null)
        {
            if (current == null)
            {
                current = this.Tree;
            }

            if (current?.ChildrenCount == 0)
            {
                return current.Metadata.Sum();
            }

            int result = 0;

            foreach (int index in current?.Metadata ?? new())
            {
                MemoryNode? child = (current?.Children.ContainsKey(index - 1) ?? false) ? current.Children[index - 1] : null;

                if (child != null)
                {
                    result += this.RootValue(child);
                }
            }

            return result;
        }
    }
}
