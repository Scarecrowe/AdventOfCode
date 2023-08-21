namespace AdventOfCode.Test.Builders
{
    using AdventOfCode.Core;
    using AdventOfCode.Core.Extensions;
    using Bogus;

    public static class DataBuilder
    {
        public static List<Vector<int>> VectorList(int count = 10)
        {
            List<Vector<int>> result = new();
            Faker<Vector<int>> faker = new();
            faker.RuleFor(x => x.X, (faker) => faker.Random.Number(1, 100))
                .RuleFor(x => x.Y, (faker) => faker.Random.Number(1, 100));

            Enumerable.Range(1, count).ForEach(i => result.Add(faker.Generate()));

            return result;
        }
    }
}
