namespace AdventOfCode.Core
{
    public class DisjointSetUnion(int count)
    {
        public int Sets { get; private set; } = count;

        private int[] Parent { get; set; } = Enumerable.Range(0, count).ToArray();

        private int[] Rank { get; set; } = Enumerable.Repeat(1, count).ToArray();

        public int Find(int x)
        {
            while (this.Parent[x] != x)
            {
                this.Parent[x] = this.Parent[this.Parent[x]];
                x = this.Parent[x];
            }

            return x;
        }

        public bool Union(int a, int b)
        {
            int rootA = this.Find(a);
            int rootB = this.Find(b);

            if (rootA == rootB)
            {
                return false;
            }

            if (this.Rank[rootA] < this.Rank[rootB])
            {
                (rootB, rootA) = (rootA, rootB);
            }

            this.Parent[rootB] = rootA;
            this.Rank[rootA] += this.Rank[rootB];
            this.Sets--;

            return true;
        }
    }
}
