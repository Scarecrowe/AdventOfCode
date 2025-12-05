namespace AdventOfCode.Puzzles._2018.Day_25___Four_Dimensional_Adventure
{
    using AdventOfCode.Core.Extensions;

    public class FourDimensionalAdventure
    {
        public FourDimensionalAdventure(string[] input) => this.Stars = Parse(input);

        public List<FourDimensionalStar> Stars { get; }

        public int ConstellationCount()
        {
            List<List<FourDimensionalStar>> constellations = new();

            foreach (FourDimensionalStar starA in this.Stars.Where(star => !star.IsGrouped))
            {
                starA.Grouped();

                List<List<FourDimensionalStar>> matches;
                List<FourDimensionalStar> constellation;

                foreach (FourDimensionalStar starB in this.Stars.Where(star => star == starA || (!star.IsGrouped && star.Distance(starA) <= 3)))
                {
                    starB.Grouped();
                    matches = constellations.Where(c => c.Any(d => d.Distance(starB) <= 3)).ToList();
                    constellation = matches.SelectMany(c => c).ToList();
                    constellations.RemoveRange(matches);
                    constellation.Add(starB);
                    constellations.Add(constellation);
                }
            }

            return constellations.Count;
        }

        private static List<FourDimensionalStar> Parse(string[] input) => input.Select(x => new FourDimensionalStar(x)).ToList();
    }
}
