namespace AdventOfCode.Puzzles._2018.Day_14___Chocolate_Charts
{
    public class Elf
    {
        public Elf(int recipe) => this.Recipe = recipe;

        public int Recipe { get; set; }

        public void Move(int recipeCount, int[] recipes) => this.Recipe = (this.Recipe + 1 + recipes[this.Recipe]) % recipeCount;
    }
}
