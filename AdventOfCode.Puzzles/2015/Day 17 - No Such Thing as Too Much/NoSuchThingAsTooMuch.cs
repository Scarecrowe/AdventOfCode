namespace AdventOfCode.Puzzles._2015.Day_17___No_Such_Thing_as_Too_Much
{
    using AdventOfCode.Core.Extensions;

    public class NoSuchThingAsTooMuch
    {
        public static int ContainerCount(string[] input, int liters) => input.ToLongList().CombinationsOfTotal(liters).Count;

        public static int CombinationCount(string[] input, int liters)
        {
            List<List<long>> combinations = input.ToLongList().CombinationsOfTotal(liters);

            return combinations.Count(x => x.Count == combinations.Min(x => x.Count));
        }
    }
}