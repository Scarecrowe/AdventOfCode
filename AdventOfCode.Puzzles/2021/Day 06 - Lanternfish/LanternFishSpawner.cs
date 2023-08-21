namespace AdventOfCode.Puzzles._2021.Day_06___Lanternfish
{
    using AdventOfCode.Core.Extensions;

    public class LanternFishSpawner
    {
        public LanternFishSpawner(string[] input) => this.LanternFish = this.ParseInput(input);

        public List<LanternFish> LanternFish { get; set; }

        public LanternFishSpawner Run(int days)
        {
            for (int day = 1; day <= days; day++)
            {
                int count = this.LanternFish.Count;
                bool infant = false;
                LanternFish infants = new(8, 1);

                for (int i = 0; i < count; i++)
                {
                    this.LanternFish[i].InternalTimer--;

                    if (this.LanternFish[i].InternalTimer < 0)
                    {
                        this.LanternFish[i].InternalTimer = 6;

                        if (!infant)
                        {
                            infants = new(8, this.LanternFish[i].Total);
                            this.LanternFish.Add(infants);
                            infant = true;
                        }
                        else
                        {
                            infants.Total += this.LanternFish[i].Total;
                        }
                    }
                }
            }

            return this;
        }

        public long TotalFish() => this.LanternFish.Sum(x => x.Total);

        private List<LanternFish> ParseInput(string[] input)
        {
            List<LanternFish> result = new();

            string[] tokens = input[0].Split(",");

            this.LanternFish = new();
            Dictionary<int, int> sorter = new();

            foreach (string part in tokens)
            {
                int alive = part.ToInt();

                if (!sorter.ContainsKey(alive))
                {
                    sorter.Add(alive, 1);
                }
                else
                {
                    sorter[alive]++;
                }
            }

            foreach (KeyValuePair<int, int> kvp in sorter)
            {
                result.Add(new(kvp.Key, kvp.Value));
            }

            return result;
        }
    }
}
