namespace AdventOfCode.Test.Core
{
    using AdventOfCode.Core;
    using AdventOfCode.Test.Builders;
    using FluentAssertions;
    using NUnit.Framework;

    public class BreadthFirstSearchItemTestBase : TheSubject<BreadthFirstSearchItem<int>>
    {
        public static Vector<int>? Point { get; set; }

        public static List<Vector<int>>? Path { get; set; }

        [SetUp]
        public void Init()
        {
            Point = new(0, 0);
            Path = DataBuilder.VectorList();
            Subject = new BreadthFirstSearchItem<int>();
        }
    }

    [TestFixture]
    public class BreadthFirstSearchItemTest : BreadthFirstSearchItemTestBase
    {
        public class Constructor : BreadthFirstSearchItemTestBase
        {
            [Test]
            public void When_told_to_create_with_the_empty_constructor()
            {
                Subject.Point.Should().Be(new Vector<int>(0, 0));
                Subject.Distance.Should().Be(0);
                Subject.Path.Should().NotBeNull();
                Subject.Path.Count.Should().Be(0);
            }

            [Test]
            public void When_told_to_create_with_a_null_point_and_distance()
            {
                Action action = () => Subject = new BreadthFirstSearchItem<int>(null, 0);

                action.Should().Throw<ArgumentNullException>();
            }

            [Test]
            public void When_told_to_create_with_a_point_and_negative_distance()
            {
                Action action = () => Subject = new BreadthFirstSearchItem<int>(Point, -1);

                action.Should().Throw<ArgumentException>();
            }

            [Test]
            public void When_told_to_create_with_a_point_and_distance()
            {
                Subject = new BreadthFirstSearchItem<int>(Point, 100);
                Subject.Point.Should().Be(new Vector<int>(0, 0));
                Subject.Distance.Should().Be(100);
                Subject.Path.Should().NotBeNull();
                Subject.Path.Count.Should().Be(1);
                Subject.Path.First().Should().Be(new Vector<int>(0, 0));
            }

            [Test]
            public void When_told_to_create_with_a_null_point_a_distance_and_path()
            {
                Action action = () => Subject = new BreadthFirstSearchItem<int>(null, 0, Path);

                action.Should().Throw<ArgumentNullException>();
            }

            [Test]
            public void When_told_to_create_with_a_point_a_negative_distance_and_path()
            {
                Action action = () => Subject = new BreadthFirstSearchItem<int>(Point, -1, Path);

                action.Should().Throw<ArgumentException>();
            }

            [Test]
            public void When_told_to_create_with_a_point_a_distance_and_path()
            {
                Subject = new BreadthFirstSearchItem<int>(Point, 100, Path);
                Subject.Point.Should().Be(new Vector<int>(0, 0));
                Subject.Distance.Should().Be(100);
                Subject.Path.Should().NotBeNull();
                Subject.Path.Count.Should().Be(11);
                Subject.Path.First().Should().Be(Path.First());
                Subject.Path.Last().Should().Be(Point);
            }
        }

        public class AddPath : BreadthFirstSearchItemTestBase
        {
            [Test]
            public void When_told_to_add_a_null_point()
            {
                Action action = () => Subject.AddPath(null);

                action.Should().Throw<ArgumentNullException>();
            }

            [Test]
            public void When_told_to_add_a_point()
            {
                Subject.AddPath(new Vector<int>(100, 100));
                Subject.AddPath(new Vector<int>(50, 50));

                Subject.Path.Count.Should().Be(2);
                Subject.Path.First().Should().Be(new Vector<int>(100, 100));
                Subject.Path.Last().Should().Be(new Vector<int>(50, 50));
            }
        }
    }
}
