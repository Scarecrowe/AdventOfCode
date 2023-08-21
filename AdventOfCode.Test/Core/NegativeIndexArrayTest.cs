namespace AdventOfCode.Test.Core
{
    using AdventOfCode.Core;
    using FluentAssertions;
    using NUnit.Framework;

    public class NegativeIndexArrayTestBase : TheSubject<NegativeIndexArray<int>>
    {
        [SetUp]
        public void Init() => Subject = new NegativeIndexArray<int>(100, -10);
    }

    [TestFixture]
    public class NegativeIndexArrayTest : NegativeIndexArrayTestBase
    {
        public class Constructor : NegativeIndexArrayTestBase
        {
            [Test]
            public void When_created_with_a_negative_capacity()
            {
                Action action = () => new NegativeIndexArray<int>(-1, 10);

                action.Should().Throw<ArgumentException>();
            }

            [Test]
            public void When_created_with_a_positive_min()
            {
                Action action = () => new NegativeIndexArray<int>(100, 10);

                action.Should().Throw<ArgumentException>();
            }

            [Test]
            public void When_with_a_capacity_and_min()
            {
                Subject.Min.Should().Be(-10);
                Subject.Max.Should().Be(100 + (-10 * -1));
                Subject.Values.Should().NotBeNull();
                Subject.Values.Count().Should().Be(100);
                Subject.Length.Should().Be(Subject.Values.Count());
            }
        }

        public class Indexer : NegativeIndexArrayTestBase
        {
            [Test]
            public void When_told_to_get_an_index()
            {
                Subject[-10] = 50;
                Subject[-10].Should().Be(50);

                Subject[-5] = -10;
                Subject[-5].Should().Be(-10);
            }

            [Test]
            public void When_told_to_set_an_index()
            {
                Subject[-10] = 50;
                Subject[-10].Should().Be(50);

                Subject[-5] = -10;
                Subject[-5].Should().Be(-10);
            }

            [Test]
            public void When_told_to_set_an_index_out_of_range()
            {
                Action action = () => Subject[-50] = 50;

                action.Should().Throw<IndexOutOfRangeException>();
            } 
        }

        public class HasIndex : NegativeIndexArrayTestBase
        {
            [Test]
            public void When_told_to_get_a_min_index_out_of_range()
                => Subject.HasIndex(-50).Should().BeFalse();

            [Test]
            public void When_told_to_get_a_max_index_out_of_range()
                => Subject.HasIndex(150).Should().BeFalse();

            [Test]
            public void When_told_to_get_an_in_range_index()
                => Subject.HasIndex(25).Should().BeTrue();
        }
    }
}
