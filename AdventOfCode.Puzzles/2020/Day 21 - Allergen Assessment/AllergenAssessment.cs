namespace AdventOfCode.Puzzles._2020.Day_21___Allergen_Assessment
{
    using AdventOfCode.Core.Extensions;

    public class AllergenAssessment
    {
        public static int NonAllergens(string[] input)
        {
            Foods foods = Foods.Parse(input);

            List<string> processed = new();

            foreach (string alergen in foods.SelectMany(x => x.Allergens).Distinct())
            {
                List<Food> possible = foods.Where(x => x.Allergens.Contains(alergen)).ToList();

                foreach (Food subFood in possible)
                {
                    List<string> toRemove = new();

                    foreach (string ingrident in subFood.Ingredients)
                    {
                        if (possible.All(x => x.Ingredients.Contains(ingrident)))
                        {
                            toRemove.Add(ingrident);
                        }
                    }

                    foreach (string tmp in toRemove)
                    {
                        foods.ForEach(x => x.Ingredients.Remove(tmp));
                    }
                }
            }

            Dictionary<string, int> nonAlergens = new();

            int count = 0;

            foreach (Food food in foods)
            {
                foreach (string ingrident in food.Ingredients)
                {
                    if (!nonAlergens.ContainsKey(ingrident))
                    {
                        nonAlergens.Add(ingrident, 1);
                        count++;
                    }
                    else
                    {
                        nonAlergens[ingrident]++;
                        count++;
                    }
                }
            }

            return count;
        }

        public static string Dangerous(string[] input)
        {
            Foods foods = Foods.Parse(input);

            List<string> processed = new();
            List<(string ingrident, string alergen)> ingridents = new();

            foreach (string alergen in foods.SelectMany(x => x.Allergens).Distinct())
            {
                List<Food> possible = foods.Where(x => x.Allergens.Contains(alergen)).ToList();

                foreach (Food subFood in possible)
                {
                    List<string> toRemove = new();

                    foreach (string ingrident in subFood.Ingredients)
                    {
                        if (possible.All(x => x.Ingredients.Contains(ingrident)))
                        {
                            toRemove.Add(ingrident);
                            ingridents.Add((ingrident, alergen));
                        }
                    }

                    foreach (string tmp in toRemove)
                    {
                        foods.ForEach(x => x.Ingredients.Remove(tmp));
                    }
                }
            }

            Dictionary<string, int> nonAlergens = new();

            int count = 0;

            foreach (Food food in foods)
            {
                foreach (string ingrident in food.Ingredients)
                {
                    if (!nonAlergens.ContainsKey(ingrident))
                    {
                        nonAlergens.Add(ingrident, 1);
                        count++;
                    }
                    else
                    {
                        nonAlergens[ingrident]++;
                        count++;
                    }
                }
            }

            foods = Foods.Parse(input);

            foreach (Food food in foods)
            {
                foreach (KeyValuePair<string, int> alergen in nonAlergens)
                {
                    food.Ingredients.Remove(alergen.Key);
                }
            }

            List<(string indgrident, string alergen)> result = new();
            List<Food> non = foods.Where(x => x.Allergens.Count == 0).ToList();
            processed = new();
            List<string> allAlergens = foods.SelectMany(x => x.Allergens).Distinct().ToList();

            while (true)
            {
                foreach (string alergen in allAlergens)
                {
                    if (processed.Contains(alergen))
                    {
                        continue;
                    }

                    List<Food> possible = foods.Where(x => x.Allergens.Contains(alergen)).ToList();
                    List<string> remove = new();

                    foreach (string ingrident in possible.SelectMany(x => x.Ingredients).Distinct())
                    {
                        if (possible.All(x => x.Ingredients.Contains(ingrident)))
                        {
                            remove.Add(ingrident);
                        }
                    }

                    if (remove.Count > 1)
                    {
                        continue;
                    }

                    result.Add((remove[0], alergen));

                    foreach (string tmp in remove)
                    {
                        foods.ForEach(x => x.Ingredients.Remove(tmp));
                    }

                    processed.Add(alergen);

                    if (processed.Count == allAlergens.Count)
                    {
                        return result.OrderBy(x => x.alergen).Select(x => x.indgrident).Join(",");
                    }
                }
            }

            throw new InvalidOperationException();
        }
    }
}
