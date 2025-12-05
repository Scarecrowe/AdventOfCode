namespace AdventOfCode.Puzzles._2020.Day_21___Allergen_Assessment
{
    using AdventOfCode.Core.Extensions;

    public class Foods : List<Food>
    {
        public static Foods Parse(string[] input)
        {
            Foods foods = new();

            foreach (string line in input)
            {
                string ingredient = string.Empty;

                Food food = new(line);

                for (int i = 0; i < line.Length; i++)
                {
                    switch (line[i])
                    {
                        case ' ':
                            if (string.IsNullOrEmpty(ingredient))
                            {
                                continue;
                            }

                            food.Ingredients.Add(ingredient);

                            ingredient = string.Empty;
                            break;
                        case '(':
                            int closing = line.IndexOf(')', i + 1);

                            string[] alergens = line.Substring(i + 1, (closing - i) - 1).Replace("contains ").Split(",");

                            foreach (string alergen in alergens)
                            {
                                food.Allergens.Add(alergen.Trim());
                            }

                            i = closing + 1;
                            break;
                        default:
                            ingredient += line[i];

                            break;
                    }
                }

                foods.Add(food);
            }

            return foods;
        }
    }
}
