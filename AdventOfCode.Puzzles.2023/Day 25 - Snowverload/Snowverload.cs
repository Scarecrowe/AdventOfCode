namespace AdventOfCode.Puzzles._2023.Day_25___Snowverload
{
    public class Snowverload
    {
        private readonly List<(int, int)> Edges = [];
        private readonly Dictionary<string, int> Map = [];
        private readonly List<string> ReverseMap = [];
        private readonly Random Random = new();

        public Snowverload(string[] input)
        {
            ParseInput(input);
        }

        private void ParseInput(string[] input)
        {
            foreach (var line in input)
            {
                string[] split = line.Split(": ");
                string component = split[0];

                int cId = this.GetId(component);

                foreach (var c in split[1].Split(' '))
                {
                    int vId = GetId(c);
                    this.Edges.Add((cId, vId));
                }
            }
        }

        private int GetId(string name)
        {
            if (!this.Map.TryGetValue(name, out int id))
            {
                id = this.ReverseMap.Count;
                this.Map[name] = id;
                this.ReverseMap.Add(name);
            }

            return id;
        }

        public string BigRedReset()
        {
            int n = this.ReverseMap.Count;

            while (true)
            {
                UnionFind dsu = new (n);
                int groups = n;

                while (groups > 2)
                {
                    var (u, v) = Edges[Random.Next(Edges.Count)];

                    if (dsu.Find(u) != dsu.Find(v))
                    {
                        dsu.Union(u, v);
                        groups--;
                    }
                }

                int cuts = 0;

                foreach (var (u, v) in Edges)
                {
                    if (dsu.Find(u) != dsu.Find(v))
                    {
                        cuts++;
                    }
                }

                if (cuts == 3)
                {
                    Dictionary<int, int> sizes = new();

                    for (int i = 0; i < n; i++)
                    {
                        int root = dsu.Find(i);

                        if (!sizes.ContainsKey(root))
                        {
                            sizes[root] = 0;
                        }

                        sizes[root]++;
                    }

                    long product = 1;

                    foreach (var size in sizes.Values)
                    {
                        product *= size;
                    }

                    return product.ToString();
                }
            }
        }

        private class UnionFind
        {
            private readonly int[] parent;
            private readonly int[] rank;

            public UnionFind(int n)
            {
                parent = new int[n];
                rank = new int[n];

                for (int i = 0; i < n; i++)
                {
                    parent[i] = i;
                }
            }

            public int Find(int x)
            {
                if (parent[x] != x)
                {
                    parent[x] = Find(parent[x]);
                }
    
                return parent[x];
            }

            public void Union(int a, int b)
            {
                a = Find(a);
                b = Find(b);

                if (a == b)
                {
                    return;
                }

                if (rank[a] < rank[b])
                {
                    parent[a] = b;
                }
                else if (rank[a] > rank[b])
                {
                    parent[b] = a;
                }
                else
                { 
                    parent[b] = a; rank[a]++; 
                }
            }
        }
    }
}
