namespace AdventOfCode.Puzzles._2015.Day_15___Science_for_Hungry_People
{
    public class Ingrident
    {
        public Ingrident(string name, int capacity, int durability, int flavor, int texture, int calories)
        {
            this.Name = name;
            this.Capacity = capacity;
            this.Durability = durability;
            this.Flavor = flavor;
            this.Texture = texture;
            this.Calories = calories;
        }

        public string Name { get; }

        public int Capacity { get; }

        public int Durability { get; }

        public int Flavor { get; }

        public int Texture { get; }

        public int Calories { get; }
    }
}
