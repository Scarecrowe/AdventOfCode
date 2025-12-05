namespace AdventOfCode.Puzzles._2020.Day_21___Allergen_Assessment
{
    public class Food
    {
        public Food(string raw)
        {
            this.Allergens = new();
            this.Ingredients = new();
            this.Raw = raw;
        }

        public List<string> Allergens { get; }

        public List<string> Ingredients { get; }

        public string Raw { get; }
    }
}
