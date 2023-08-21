namespace AdventOfCode.Puzzles._2018.Day_02___Inventory_Management_System
{
    public class InventoryManagementSystem
    {
        public InventoryManagementSystem(string[] input) => this.BoxIds = input.ToList();

        public List<string> BoxIds { get; }

        public int Checksum()
        {
            int pair = 0;
            int triple = 0;

            foreach (string boxId in this.BoxIds)
            {
                Dictionary<char, int> counts = CountChars(boxId);

                pair += counts.Any(x => x.Value == 2) ? 1 : 0;
                triple += counts.Any(x => x.Value == 3) ? 1 : 0;
            }

            return pair * triple;
        }

        public string PrototypeFabric()
        {
            for (int i = 0; i < this.BoxIds.Count; i++)
            {
                for (int j = 0; j < this.BoxIds.Count; j++)
                {
                    if (i != j)
                    {
                        List<int> diff = new();

                        for (int k = 0; k < this.BoxIds[0].Length; k++)
                        {
                            if (this.BoxIds[i][k] != this.BoxIds[j][k])
                            {
                                diff.Add(k);
                            }
                        }

                        if (diff.Count == 1)
                        {
                            return this.BoxIds[i].Remove(diff[0], 1);
                        }
                    }
                }
            }

            return string.Empty;
        }

        private static Dictionary<char, int> CountChars(string boxId)
        {
            Dictionary<char, int> result = new();

            foreach (char c in boxId)
            {
                if (!result.ContainsKey(c))
                {
                    result.Add(c, 0);
                }

                result[c]++;
            }

            return result;
        }
    }
}
