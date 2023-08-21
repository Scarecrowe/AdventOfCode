namespace AdventOfCode.Puzzles._2018.Day_14___Chocolate_Charts
{
    using System.Text;
    using AdventOfCode.Core.Extensions;

    public class ChocolateCharts
    {
        public ChocolateCharts(string input)
        {
            this.TotalRecipes = input.ToInt();
            this.Recipes = new int[50000000];
            this.Recipes[0] = 3;
            this.Recipes[1] = 7;
            this.RecipeCount = 2;
            this.ElfA = new(0);
            this.ElfB = new(1);
        }

        public Elf ElfA { get; }

        public Elf ElfB { get; }

        public int[] Recipes { get; }

        public int RecipeCount { get; private set; }

        public int TotalRecipes { get; }

        public string NextTenRecipes()
        {
            for (int i = 0; i < this.TotalRecipes + 11; i++)
            {
                int sum = this.Recipes[this.ElfA.Recipe] + this.Recipes[this.ElfB.Recipe];

                if (sum < 10)
                {
                    this.Recipes[this.RecipeCount] = sum;
                }
                else
                {
                    this.Recipes[this.RecipeCount] = 1;
                    this.RecipeCount++;
                    this.Recipes[this.RecipeCount] = sum - 10;
                }

                this.RecipeCount++;

                this.ElfA.Recipe = (this.ElfA.Recipe + 1 + this.Recipes[this.ElfA.Recipe]) % this.RecipeCount;
                this.ElfB.Recipe = (this.ElfB.Recipe + 1 + this.Recipes[this.ElfB.Recipe]) % this.RecipeCount;
            }

            int index = this.TotalRecipes;
            StringBuilder result = new();

            for (int i = this.TotalRecipes; i < this.TotalRecipes + 10; i++)
            {
                result.Append(this.Recipes[index]);
                index = (index + 1) % this.RecipeCount;
            }

            return result.ToString();
        }

        public int IndexOfRecipe()
        {
            int[] digits = this.TotalRecipes.ToString().Select(c => int.Parse(c.ToString())).ToArray();
            int beginIndex = 0;
            int digitIndex = 0;
            bool canLoop = true;

            while (canLoop)
            {
                int recipeA = this.Recipes[this.ElfA.Recipe];
                int recipeB = this.Recipes[this.ElfB.Recipe];
                int sum = recipeA + recipeB;

                if (sum < 10)
                {
                    this.Recipes[this.RecipeCount] = sum;
                }
                else
                {
                    this.Recipes[this.RecipeCount] = 1;
                    this.RecipeCount++;
                    this.Recipes[this.RecipeCount] = sum - 10;
                }

                this.RecipeCount++;

                while (beginIndex + digitIndex < this.RecipeCount)
                {
                    if (digits[digitIndex] == this.Recipes[beginIndex + digitIndex])
                    {
                        if (digitIndex == digits.Length - 1)
                        {
                            return beginIndex;
                        }

                        digitIndex++;
                    }
                    else
                    {
                        digitIndex = 0;
                        beginIndex++;
                    }
                }

                this.ElfA.Recipe = (this.ElfA.Recipe + 1 + recipeA) % this.RecipeCount;
                this.ElfB.Recipe = (this.ElfB.Recipe + 1 + recipeB) % this.RecipeCount;
            }

            return -1;
        }
    }
}
