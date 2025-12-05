namespace AdventOfCode.Puzzles._2024.Day_25___Code_Chronicle
{
    using AdventOfCode.Core;

    public class CodeChronicle
    {
        public List<VectorArray<int, char>> Locks { get; private set; }

        public List<VectorArray<int, char>> Keys { get; private set; }


        public CodeChronicle(string[] input)
        {
            this.Locks = [];
            this.Keys = [];
            this.Parse(input);
        }

        public int Unique()
        {
            int result = 0;
            int height = 0;

            foreach (var key in this.Keys)
            {
                List<int> keyHeights = [];

                for (int x = 0; x < key.Width; x++)
                {
                     height = 0;

                    for (int y = key.Height - 1; y >= 0; y--)
                    {
                        if (key[new(x, y)] == '#')
                        {
                            height++;
                        }
                        else
                        {
                            break;
                        }
                    }

                    keyHeights.Add(height - 1);
                }

                foreach(var @lock in this.Locks)
                {
                    List<int> lockHeights = [];

                    for (int x = 0; x < @lock.Width; x++)
                    {
                        height = 0;

                        for (int y = 0; y < @lock.Height; y++)
                        {
                            if (@lock[new(x, y)] == '#')
                            {
                                height++;
                            }
                            else
                            {
                                break;
                            }
                        }

                        lockHeights.Add(height - 1);
                    }

                    bool isMatch = true;

                    for(int i = 0; i < keyHeights.Count; i++)
                    {
                        int keyHeight = keyHeights[i];
                        int lockHeight = lockHeights[i];

                        int total = keyHeight + lockHeight;

                        if (total > 5)
                        {
                            isMatch = false;
                            break;
                        }
                    }

                    if (isMatch)
                    {
                        result++;
                    }
                }
            }

            return result;
        }

        private void Parse(string[] input)
        {
            List<string> lines = [];
            List<VectorArray<int, char>> maps = [];

            foreach (string line in input)
            {
                if (string.IsNullOrEmpty(line))
                {
                    maps.Add(new([.. lines], c => c));
                    lines.Clear();
                    continue;
                }

                lines.Add(line);
            }

            maps.Add(new([.. lines], c => c));

            foreach(var map in maps)
            {
                bool isLock = true;

                for(int x = 0; x < map.Width; x++)
                {
                    if (map[new(x, 0)] != '#')
                    {
                        isLock = false;
                        break;
                    }
                }

                if(isLock)
                {
                    this.Locks.Add(map);
                }
                else
                {
                    this.Keys.Add(map);
                }
            }
        }
    }
}
