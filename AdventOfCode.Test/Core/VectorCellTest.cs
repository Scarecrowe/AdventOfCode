namespace AdventOfCode.Test.Core
{
    using AdventOfCode.Core;
    using FluentAssertions;
    using NUnit.Framework;

    public class VectorCellTestBase : TheSubject<VectorCell<int, int>>
    {
        [SetUp]
        public void Init()
        {
            Subject = new VectorCell<int, int>(new(45, 99), Cardinal.SouthEast);
        }
    }

    [TestFixture]
    public class VectorCellTest : VectorCellTestBase
    {
        public class Constructor : VectorCellTestBase
        {
            [Test]
            public void When_created_with_a_value()
            {
                Subject = new VectorCell<int, int>(new(45, 99), 99);
                Subject.Point.Should().Be(new Vector<int>(45, 99));
                Subject.Direction.Should().Be(Cardinal.North);
                Subject.Value.Should().Be(99);
            }

            [Test]
            public void When_created_with_a_direction()
            {
                Subject.Point.Should().Be(new Vector<int>(45, 99));
                Subject.Direction.Should().Be(Cardinal.SouthEast);
                Subject.Value.Should().Be(default);
            }

            [Test]
            public void When_created_with_a_value_and_direction()
            {
                Subject = new VectorCell<int, int>(new(45, 99), 99, Cardinal.NorthEast);
                Subject.Point.Should().Be(new Vector<int>(45, 99));
                Subject.Direction.Should().Be(Cardinal.NorthEast);
                Subject.Value.Should().Be(99);
            }
        }

        public class ToTuple : VectorCellTestBase
        {
            [Test]
            public void When_told_to_cast_to_a_tuple()
            {
                Subject.ToTuple().Should().Be((45, 99));
            }
        }

        public new class GetHashCode : VectorCellTestBase
        {
            [Test]
            public void When_told_to_get_a_hash_code()
            {
                Subject = new VectorCell<int, int>(new(45, 99), Cardinal.SouthEast);
                int hashCodeA = Subject.GetHashCode();

                Subject = new VectorCell<int, int>(new(45, 99), Cardinal.SouthEast);

                Subject.GetHashCode().Should().Be(hashCodeA);
            }
        }
    }
}

