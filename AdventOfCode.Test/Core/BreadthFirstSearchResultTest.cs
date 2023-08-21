namespace AdventOfCode.Test.Core
{
    using AdventOfCode.Core;
    using AdventOfCode.Test.Builders;
    using FluentAssertions;
    using NUnit.Framework;

    public class BreadthFirstSearchResultTestBase : TheSubject<BreadthFirstSearchResult<int>>
    {
        public static List<Vector<int>>? Path { get; set; }

        [SetUp]
        public void Init()
        {
            Path = DataBuilder.VectorList();
            Subject = new BreadthFirstSearchResult<int>(Path);
        }
    }

    [TestFixture]
    public class BreadthFirstSearchResultTest : BreadthFirstSearchResultTestBase
    {
        public class Constructor : BreadthFirstSearchResultTestBase
        {
            [Test]
            public void When_told_to_create_with_the_empty_constructor()
            {
                Subject.Distance.Should().Be(10);
                Subject.Path.Should().NotBeNull();
                Subject.Path.Count.Should().Be(10);
            }

            [Test]
            public void When_told_to_create_with_a_null_path()
            {
                Action action = () => Subject = new BreadthFirstSearchResult<int>(null);

                action.Should().Throw<ArgumentNullException>();
            }
        }
    }
}
