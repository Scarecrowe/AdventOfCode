namespace AdventOfCode.Puzzles._2015.Day_21___RPG_Simulator_20XX
{
    public class Item
    {
        public Item(string name, int cost, int power, int defence, ItemType type)
        {
            this.Name = name;
            this.Cost = cost;
            this.Power = power;
            this.Defence = defence;
            this.Type = type;
        }

        public string Name { get; }

        public int Cost { get; }

        public int Power { get; }

        public int Defence { get; }

        public ItemType Type { get; }
    }
}
