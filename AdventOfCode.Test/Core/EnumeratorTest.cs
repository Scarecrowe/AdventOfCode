namespace AdventOfCode.Test.Core
{
    using AdventOfCode.Core;
    using FluentAssertions;
    using NUnit.Framework;

    public class EnumeratorTestBase
    {
        [SetUp]
        public void Init()
        {
        }
    }

    [TestFixture]
    public class EnumeratorTest : EnumeratorTestBase
    {
        public class Range2D : EnumerableExtensionsTestBase
        {
            [Test]
            public void When_told_to_enumerate_a_2D_axis_with_a_negative_length()
            {
                Action action = () => Enumerator.Range2D(-1).ToList();

                action.Should().Throw<ArgumentException>();
            }

            [Test]
            public void When_told_to_enumerate_a_2D_axis_with_a_positive_length()
            {
                List<(int i, int j)> result = Enumerator.Range2D(100).ToList();

                result.Count.Should().Be(100 * 100);
                result.First().Should().Be(new(0, 0));
                result.Last().Should().Be(new(99, 99));
            }

            [Test]
            public void When_told_to_enumerate_a_2D_axis_with_a_negative_width()
            {
                Action action = () => Enumerator.Range2D(-1, 100).ToList();

                action.Should().Throw<ArgumentException>();
            }

            [Test]
            public void When_told_to_enumerate_a_2D_axis_with_a_negative_height()
            {
                Action action = () => Enumerator.Range2D(100, -1).ToList();

                action.Should().Throw<ArgumentException>();
            }

            [Test]
            public void When_told_to_enumerate_a_2D_axis_with_a_positive_width_and_height()
            {
                List<(int i, int j)> result = Enumerator.Range2D(100, 100).ToList();

                result.Count.Should().Be(100 * 100);
                result.First().Should().Be(new(0, 0));
                result.Last().Should().Be(new(99, 99));
            }
        }

        public class Range3D : EnumerableExtensionsTestBase
        {
            [Test]
            public void When_told_to_enumerate_a_3D_axis_with_a_negative_length()
            {
                Action action = () => Enumerator.Range3D(-1).ToList();

                action.Should().Throw<ArgumentException>();
            }

            [Test]
            public void When_told_to_enumerate_a_3D_axis_with_a_positive_length()
            {
                List<(int i, int j, int k)> result = Enumerator.Range3D(100).ToList();

                result.Count.Should().Be(100 * 100 * 100);
                result.First().Should().Be(new(0, 0, 0));
                result.Last().Should().Be(new(99, 99, 99));
            }
        }
    }
}
