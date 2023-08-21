namespace AdventOfCode.Test.Core
{
    using AdventOfCode.Core;
    using AdventOfCode.Core.Extensions;
    using AdventOfCode.Test.Extensions;
    using FluentAssertions;
    using NUnit.Framework;

    public class VectorTestBase : TheSubject<Vector<int>>
    {
        [SetUp]
        public void Init()
        {
            Subject = new Vector<int>(100, 100);
        }
    }

    [TestFixture]
    public class VectorTest : VectorTestBase
    {
        public class EmptyConstructor : VectorTestBase
        {
            [Test]
            public void When_created()
            {
                Subject = new Vector<int>();
                Subject.X.Should().Be(0);
                Subject.Y.Should().Be(0);
                Subject.Z.Should().Be(0);
                Subject.T.Should().Be(0);
            }
        }

        public class IntConstructor : VectorTestBase
        {
            [Test]
            public void When_created_with_a_2D_axis()
            {
                Vector<int> result = new(100, 50);

                result.X.Should().Be(100);
                result.Y.Should().Be(50);
                result.Z.Should().Be(0);
                result.T.Should().Be(0);
            }

            [Test]
            public void When_created_with_a_3D_axis_int()
            {
                Vector<int> result = new(100, 50, 25);

                result.X.Should().Be(100);
                result.Y.Should().Be(50);
                result.Z.Should().Be(25);
                result.T.Should().Be(0);
            }

            [Test]
            public void When_created_with_a_3D_axis_long()
            {
                Vector<int> result = new(100L, 50L, 25L);

                result.X.Should().Be(100);
                result.Y.Should().Be(50);
                result.Z.Should().Be(25);
                result.T.Should().Be(0);
            }

            [Test]
            public void When_created_with_a_4D_axis_int()
            {
                Vector<int> result = new(100, 50, 25, 12);

                result.X.Should().Be(100);
                result.Y.Should().Be(50);
                result.Z.Should().Be(25);
                result.T.Should().Be(12);
            }

            [Test]
            public void When_created_with_a_4D_axis_long()
            {
                Vector<int> result = new(100L, 50L, 25L, 12L);

                result.X.Should().Be(100);
                result.Y.Should().Be(50);
                result.Z.Should().Be(25);
                result.T.Should().Be(12);
            }
        }

        public class LongConstructor : VectorTestBase
        {
            [Test]
            public void When_created_with_a_2D_axis()
            {
                Vector<long> result = new(100L, 50L);

                result.X.Should().Be(100L);
                result.Y.Should().Be(50L);
                result.Z.Should().Be(0L);
                result.T.Should().Be(0L);
            }

            [Test]
            public void When_created_with_a_3D_axis_with_long()
            {
                Vector<long> result = new(100L, 50L, 25L);

                result.X.Should().Be(100L);
                result.Y.Should().Be(50L);
                result.Z.Should().Be(25L);
                result.T.Should().Be(0L);
            }

            [Test]
            public void When_created_with_a_3D_axis_with_int()
            {
                Vector<long> result = new(100, 50, 25);

                result.X.Should().Be(100L);
                result.Y.Should().Be(50L);
                result.Z.Should().Be(25L);
                result.T.Should().Be(0L);
            }

            [Test]
            public void When_created_with_a_4D_axis()
            {
                Vector<long> result = new(100L, 50L, 25L, 12L);

                result.X.Should().Be(100L);
                result.Y.Should().Be(50L);
                result.Z.Should().Be(25L);
                result.T.Should().Be(12L);
            }
        }

        public class GenericConstructor : VectorTestBase
        {
            private static void GenericConstructorTest<TSize>(int x, int y, int? z = null, int? t = null)
            {
                Vector<TSize> result = new(x.ToGeneric<TSize>(), y.ToGeneric<TSize>());

                if (z != null && t == null)
                {
                    result = new(x.ToGeneric<TSize>(), y.ToGeneric<TSize>(), z.Value.ToGeneric<TSize>());
                }

                if (z != null && t != null)
                {
                    result = new(x.ToGeneric<TSize>(), y.ToGeneric<TSize>(), z.Value.ToGeneric<TSize>(), t.Value.ToGeneric<TSize>());
                }

                result.X.Should().Be(100L);
                result.Y.Should().Be(50L);

                if (z != null)
                {
                    result.Z.Should().Be(z);
                }

                if (t != null)
                {
                    result.T.Should().Be(t);
                }
            }

            [Test]
            public void When_created_with_a_2D_axis() => GenericConstructorTest<int>(100, 50);

            [Test]
            public void When_created_with_a_3D_axis() => GenericConstructorTest<int>(100, 50, 25);

            [Test]
            public void When_created_with_a_4D_axis() => GenericConstructorTest<int>(100, 50, 25, 12);
        }

        public class CollectionConstructor : VectorTestBase
        {
            [Test]
            public void When_created_with_a_collection_size_less_than_2D()
            {
                Action action = () => new Vector<int>(new List<int> { 100 });

                action.Should().Throw<ArgumentException>();
            }

            [Test]
            public void When_created_with_a_collection_size_greater_than_4D()
            {
                Action action = () => new Vector<int>(new List<int> { 100, 50, 25, 12, 6 });

                action.Should().Throw<ArgumentException>();
            }

            [Test]
            public void When_created_with_a_2D_collection()
            {
                Subject = new Vector<int>(new List<int> { 100, 50 });

                Subject.X.Should().Be(100);
                Subject.Y.Should().Be(50);
                Subject.Z.Should().Be(0);
                Subject.T.Should().Be(0);
            }

            [Test]
            public void When_created_with_a_3D_collection()
            {
                Subject = new Vector<int>(new List<int> { 100, 50, 25 });

                Subject.X.Should().Be(100);
                Subject.Y.Should().Be(50);
                Subject.Z.Should().Be(25);
                Subject.T.Should().Be(0);
            }

            [Test]
            public void When_created_with_a_4D_collection()
            {
                Subject = new Vector<int>(new List<int> { 100, 50, 25, 12 });

                Subject.X.Should().Be(100);
                Subject.Y.Should().Be(50);
                Subject.Z.Should().Be(25);
                Subject.T.Should().Be(12);
            }
        }

        public class VectorConstructor : VectorTestBase
        {
            [Test]
            public void When_created()
            {
                Subject = new Vector<int>(new Vector<int>(100, 50, 25, 12));

                Subject.X.Should().Be(100);
                Subject.Y.Should().Be(50);
                Subject.Z.Should().Be(25);
                Subject.T.Should().Be(12);
            }
        }

        public class Operators : VectorTest
        {
            public class IntEqualOperator : VectorTestBase
            {
                [Test]
                public void When_asked_if_two_matching_2D_vectors_are_equal()
                {
                    Vector<int> vectorA = new(100, 50);
                    Vector<int> vectorB = new(100, 50);

                    bool result = vectorA == vectorB;

                    result.Should().BeTrue();
                }

                [Test]
                public void When_asked_if_two_non_matching_2D_vectors_are_equal()
                {
                    Vector<int> vectorA = new(100, 50);
                    Vector<int> vectorB = new(50, 100);

                    bool result = vectorA == vectorB;

                    result.Should().BeFalse();
                }

                [Test]
                public void When_asked_if_two_matching_3D_vectors_are_equal()
                {
                    Vector<int> vectorA = new(100, 50, 25);
                    Vector<int> vectorB = new(100, 50, 25);

                    bool result = vectorA == vectorB;

                    result.Should().BeTrue();
                }

                [Test]
                public void When_asked_if_two_non_matching_3D_vectors_are_equal()
                {
                    Vector<int> vectorA = new(100, 50, 25);
                    Vector<int> vectorB = new(25, 50, 100);

                    bool result = vectorA == vectorB;

                    result.Should().BeFalse();
                }

                [Test]
                public void When_asked_if_two_matching_4D_vectors_are_equal()
                {
                    Vector<int> vectorA = new(100, 50, 25, 12);
                    Vector<int> vectorB = new(100, 50, 25, 12);

                    bool result = vectorA == vectorB;

                    result.Should().BeTrue();
                }

                [Test]
                public void When_asked_if_two_non_matching_4D_vectors_are_equal()
                {
                    Vector<int> vectorA = new(100, 50, 25, 12);
                    Vector<int> vectorB = new(12, 25, 50, 100);

                    bool result = vectorA == vectorB;

                    result.Should().BeFalse();
                }
            }

            public class IntNotEqualOperator : VectorTestBase
            {
                [Test]
                public void When_asked_if_two_matching_2D_vectors_are_not_equal()
                {
                    Vector<int> vectorA = new(100, 50);
                    Vector<int> vectorB = new(100, 50);

                    bool result = vectorA != vectorB;

                    result.Should().BeFalse();
                }

                [Test]
                public void When_asked_if_two_non_matching_2D_vectors_are_not_equal()
                {
                    Vector<int> vectorA = new(100, 50);
                    Vector<int> vectorB = new(50, 100);

                    bool result = vectorA != vectorB;

                    result.Should().BeTrue();
                }

                [Test]
                public void When_asked_if_two_matching_3D_vectors_are_not_equal()
                {
                    Vector<int> vectorA = new(100, 50, 25);
                    Vector<int> vectorB = new(100, 50, 25);

                    bool result = vectorA != vectorB;

                    result.Should().BeFalse();
                }

                [Test]
                public void When_asked_if_two_non_matching_3D_vectors_are_not_equal()
                {
                    Vector<int> vectorA = new(100, 50, 25);
                    Vector<int> vectorB = new(25, 50, 100);

                    bool result = vectorA != vectorB;

                    result.Should().BeTrue();
                }

                [Test]
                public void When_asked_if_two_matching_4D_vectors_are_not_equal()
                {
                    Vector<int> vectorA = new(100, 50, 25, 12);
                    Vector<int> vectorB = new(100, 50, 25, 12);

                    bool result = vectorA != vectorB;

                    result.Should().BeFalse();
                }

                [Test]
                public void When_asked_if_two_non_matching_4D_vectors_are_not_equal()
                {
                    Vector<int> vectorA = new(100, 50, 25, 12);
                    Vector<int> vectorB = new(12, 25, 50, 100);

                    bool result = vectorA != vectorB;

                    result.Should().BeTrue();
                }
            }

            public class IntSubtractionOperator : VectorTestBase
            {
                [Test]
                public void When_asked_to_subtract_2D_vectors()
                {
                    Vector<int> vectorA = new(100, 50);
                    Vector<int> vectorB = new(100, 50);

                    Vector<int> result = vectorA - vectorB;

                    result.Should().Be(new Vector<int>(0, 0));
                }

                [Test]
                public void When_asked_to_subtract_3D_vectors()
                {
                    Vector<int> vectorA = new(100, 50, 25);
                    Vector<int> vectorB = new(100, 50, 25);

                    Vector<int> result = vectorA - vectorB;

                    result.Should().Be(new Vector<int>(0, 0));
                }

                [Test]
                public void When_asked_to_subtract_4D_vectors()
                {
                    Vector<int> vectorA = new(100, 50, 25, 12);
                    Vector<int> vectorB = new(100, 50, 25, 12);

                    Vector<int> result = vectorA - vectorB;

                    result.Should().Be(new Vector<int>(0, 0));
                }

                [Test]
                public void When_asked_to_subtract_a_2D_vector_with_a_value()
                {
                    Vector<int> vector = new(100, 50);
                    Vector<int> result = vector - 10;

                    result.Should().Be(new Vector<int>(90, 40, -10, -10));
                }

                [Test]
                public void When_asked_to_subtract_a_3D_vector_with_a_value()
                {
                    Vector<int> vector = new(100, 50, 25);
                    Vector<int> result = vector - 10;

                    result.Should().Be(new Vector<int>(90, 40, 15, -10));
                }

                [Test]
                public void When_asked_to_subtract_a_4D_vector_with_a_value()
                {
                    Vector<int> vector = new(100, 50, 25, 12);
                    Vector<int> result = vector - 10;

                    result.Should().Be(new Vector<int>(90, 40, 15, 2));
                }
            }

            public class IntAdditionOperator : VectorTestBase
            {
                [Test]
                public void When_asked_to_add_2D_vectors()
                {
                    Vector<int> vectorA = new(100, 50);
                    Vector<int> vectorB = new(100, 50);

                    Vector<int> result = vectorA + vectorB;

                    result.Should().Be(new Vector<int>(200, 100));
                }

                [Test]
                public void When_asked_to_add_3D_vectors()
                {
                    Vector<int> vectorA = new(100, 50, 25);
                    Vector<int> vectorB = new(100, 50, 25);

                    Vector<int> result = vectorA + vectorB;

                    result.Should().Be(new Vector<int>(200, 100, 50));
                }

                [Test]
                public void When_asked_to_add_4D_vectors()
                {
                    Vector<int> vectorA = new(100, 50, 25, 12);
                    Vector<int> vectorB = new(100, 50, 25, 12);

                    Vector<int> result = vectorA + vectorB;

                    result.Should().Be(new Vector<int>(200, 100, 50, 24));
                }

                [Test]
                public void When_asked_to_add_a_2D_vector_with_a_value()
                {
                    Vector<int> vector = new(100, 50);
                    Vector<int> result = vector + 10;

                    result.Should().Be(new Vector<int>(110, 60, 10, 10));
                }

                [Test]
                public void When_asked_to_add_a_3D_vector_with_a_value()
                {
                    Vector<int> vector = new(100, 50, 25);
                    Vector<int> result = vector + 10;

                    result.Should().Be(new Vector<int>(110, 60, 35, 10));
                }

                [Test]
                public void When_asked_to_add_a_4D_vector_with_a_value()
                {
                    Vector<int> vector = new(100, 50, 25, 12);
                    Vector<int> result = vector + 10;

                    result.Should().Be(new Vector<int>(110, 60, 35, 22));
                }
            }

            public class IntMultiplicationOperator : VectorTestBase
            {
                [Test]
                public void When_asked_to_multiple_2D_vectors()
                {
                    Vector<int> vectorA = new(2, 4);
                    Vector<int> vectorB = new(4, 2);

                    Vector<int> result = vectorA * vectorB;

                    result.Should().Be(new Vector<int>(8, 8));
                }

                [Test]
                public void When_asked_to_multiple_3D_vectors()
                {
                    Vector<int> vectorA = new(2, 4, 6);
                    Vector<int> vectorB = new(2, 4, 6);

                    Vector<int> result = vectorA * vectorB;

                    result.Should().Be(new Vector<int>(4, 16, 36));
                }

                [Test]
                public void When_asked_to_multiple_4D_vectors()
                {
                    Vector<int> vectorA = new(2, 4, 6, 8);
                    Vector<int> vectorB = new(2, 4, 6, 8);

                    Vector<int> result = vectorA * vectorB;

                    result.Should().Be(new Vector<int>(4, 16, 36, 64));
                }

                [Test]
                public void When_asked_to_multiple_a_2D_vector_with_a_value()
                {
                    Vector<int> vector = new(2, 4);
                    Vector<int> result = vector * 10;

                    result.Should().Be(new Vector<int>(20, 40, 0, 0));
                }

                [Test]
                public void When_asked_to_multiple_a_3D_vector_with_a_value()
                {
                    Vector<int> vector = new(2, 4, 6);
                    Vector<int> result = vector * 10;

                    result.Should().Be(new Vector<int>(20, 40, 60, 0));
                }

                [Test]
                public void When_asked_to_multiple_a_4D_vector_with_a_value()
                {
                    Vector<int> vector = new(2, 4, 6, 8);
                    Vector<int> result = vector * 10;

                    result.Should().Be(new Vector<int>(20, 40, 60, 80));
                }
            }

            public class IntDivisionOperator : VectorTestBase
            {
                [Test]
                public void When_asked_to_divide_2D_vectors()
                {
                    Vector<int> vectorA = new(4, 4);
                    Vector<int> vectorB = new(2, 2);

                    Vector<int> result = vectorA / vectorB;

                    result.Should().Be(new Vector<int>(2, 2));
                }

                [Test]
                public void When_asked_to_divide_3D_vectors()
                {
                    Vector<int> vectorA = new(4, 4, 4);
                    Vector<int> vectorB = new(2, 2, 2);

                    Vector<int> result = vectorA / vectorB;

                    result.Should().Be(new Vector<int>(2, 2, 2));
                }

                [Test]
                public void When_asked_to_divide_4D_vectors()
                {
                    Vector<int> vectorA = new(4, 4, 4, 4);
                    Vector<int> vectorB = new(2, 2, 2, 2);

                    Vector<int> result = vectorA / vectorB;

                    result.Should().Be(new Vector<int>(2, 2, 2, 2));
                }

                [Test]
                public void When_asked_to_divide_a_2D_vector_with_a_value()
                {
                    Vector<int> vector = new(2, 4);
                    Vector<int> result = vector / 2;

                    result.Should().Be(new Vector<int>(1, 2, 0, 0));
                }

                [Test]
                public void When_asked_to_divide_a_3D_vector_with_a_value()
                {
                    Vector<int> vector = new(2, 4, 6);
                    Vector<int> result = vector / 2;

                    result.Should().Be(new Vector<int>(1, 2, 3, 0));
                }

                [Test]
                public void When_asked_to_divide_a_4D_vector_with_a_value()
                {
                    Vector<int> vector = new(2, 4, 6, 8);
                    Vector<int> result = vector / 2;

                    result.Should().Be(new Vector<int>(1, 2, 3, 4));
                }
            }

            public class LongEqualOperator : VectorTestBase
            {
                [Test]
                public void When_asked_if_two_matching_2D_vectors_are_equal()
                {
                    Vector<long> vectorA = new(100L, 50L);
                    Vector<long> vectorB = new(100L, 50L);

                    bool result = vectorA == vectorB;

                    result.Should().BeTrue();
                }

                [Test]
                public void When_asked_if_two_non_matching_2D_vectors_are_equal()
                {
                    Vector<long> vectorA = new(100L, 50L);
                    Vector<long> vectorB = new(50L, 100L);

                    bool result = vectorA == vectorB;

                    result.Should().BeFalse();
                }

                [Test]
                public void When_asked_if_two_matching_3D_vectors_are_equal()
                {
                    Vector<long> vectorA = new(100L, 50L, 25L);
                    Vector<long> vectorB = new(100L, 50L, 25L);

                    bool result = vectorA == vectorB;

                    result.Should().BeTrue();
                }

                [Test]
                public void When_asked_if_two_non_matching_3D_vectors_are_equal()
                {
                    Vector<long> vectorA = new(100L, 50L, 25L);
                    Vector<long> vectorB = new(25L, 50L, 100L);

                    bool result = vectorA == vectorB;

                    result.Should().BeFalse();
                }

                [Test]
                public void When_asked_if_two_matching_4D_vectors_are_equal()
                {
                    Vector<long> vectorA = new(100L, 50L, 25L, 12L);
                    Vector<long> vectorB = new(100L, 50L, 25L, 12L);

                    bool result = vectorA == vectorB;

                    result.Should().BeTrue();
                }

                [Test]
                public void When_asked_if_two_non_matching_4D_vectors_are_equal()
                {
                    Vector<long> vectorA = new(100L, 50L, 25L, 12L);
                    Vector<long> vectorB = new(12L, 25L, 50L, 100L);

                    bool result = vectorA == vectorB;

                    result.Should().BeFalse();
                }
            }

            public class LongNotEqualOperator : VectorTestBase
            {
                [Test]
                public void When_asked_if_two_matching_2D_vectors_are_not_equal()
                {
                    Vector<long> vectorA = new(100L, 50L);
                    Vector<long> vectorB = new(100L, 50L);

                    bool result = vectorA != vectorB;

                    result.Should().BeFalse();
                }

                [Test]
                public void When_asked_if_two_non_matching_2D_vectors_are_not_equal()
                {
                    Vector<long> vectorA = new(100L, 50L);
                    Vector<long> vectorB = new(50L, 100L);

                    bool result = vectorA != vectorB;

                    result.Should().BeTrue();
                }

                [Test]
                public void When_asked_if_two_matching_3D_vectors_are_not_equal()
                {
                    Vector<long> vectorA = new(100L, 50L, 25L);
                    Vector<long> vectorB = new(100L, 50L, 25L);

                    bool result = vectorA != vectorB;

                    result.Should().BeFalse();
                }

                [Test]
                public void When_asked_if_two_non_matching_3D_vectors_are_not_equal()
                {
                    Vector<long> vectorA = new(100L, 50L, 25L);
                    Vector<long> vectorB = new(25L, 50L, 100L);

                    bool result = vectorA != vectorB;

                    result.Should().BeTrue();
                }

                [Test]
                public void When_asked_if_two_matching_4D_vectors_are_not_equal()
                {
                    Vector<long> vectorA = new(100L, 50L, 25L, 12L);
                    Vector<long> vectorB = new(100L, 50L, 25L, 12L);

                    bool result = vectorA != vectorB;

                    result.Should().BeFalse();
                }

                [Test]
                public void When_asked_if_two_non_matching_4D_vectors_are_not_equal()
                {
                    Vector<long> vectorA = new(100L, 50L, 25L, 12L);
                    Vector<long> vectorB = new(12L, 25L, 50L, 100L);

                    bool result = vectorA != vectorB;

                    result.Should().BeTrue();
                }
            }

            public class LongSubtractionOperator : VectorTestBase
            {
                [Test]
                public void When_asked_to_subtract_2D_vectors()
                {
                    Vector<long> vectorA = new(100L, 50L);
                    Vector<long> vectorB = new(100L, 50L);

                    Vector<long> result = vectorA - vectorB;

                    result.Should().Be(new Vector<long>(0, 0));
                }

                [Test]
                public void When_asked_to_subtract_3D_vectors()
                {
                    Vector<long> vectorA = new(100, 50, 25);
                    Vector<long> vectorB = new(100, 50, 25);

                    Vector<long> result = vectorA - vectorB;

                    result.Should().Be(new Vector<long>(0, 0));
                }

                [Test]
                public void When_asked_to_subtract_4D_vectors()
                {
                    Vector<long> vectorA = new(100, 50, 25, 12);
                    Vector<long> vectorB = new(100, 50, 25, 12);

                    Vector<long> result = vectorA - vectorB;

                    result.Should().Be(new Vector<long>(0, 0));
                }

                [Test]
                public void When_asked_to_subtract_a_2D_vector_with_a_value()
                {
                    Vector<long> vector = new(100, 50);
                    Vector<long> result = vector - 10;

                    result.Should().Be(new Vector<long>(90, 40, -10, -10));
                }

                [Test]
                public void When_asked_to_subtract_a_3D_vector_with_a_value()
                {
                    Vector<long> vector = new(100, 50, 25);
                    Vector<long> result = vector - 10;

                    result.Should().Be(new Vector<long>(90, 40, 15, -10));
                }

                [Test]
                public void When_asked_to_subtract_a_4D_vector_with_a_value()
                {
                    Vector<long> vector = new(100, 50, 25, 12);
                    Vector<long> result = vector - 10;

                    result.Should().Be(new Vector<long>(90, 40, 15, 2));
                }
            }

            public class LongAdditionOperator : VectorTestBase
            {
                [Test]
                public void When_asked_to_add_2D_vectors()
                {
                    Vector<long> vectorA = new(100, 50);
                    Vector<long> vectorB = new(100, 50);

                    Vector<long> result = vectorA + vectorB;

                    result.Should().Be(new Vector<long>(200, 100));
                }

                [Test]
                public void When_asked_to_add_3D_vectors()
                {
                    Vector<long> vectorA = new(100, 50, 25);
                    Vector<long> vectorB = new(100, 50, 25);

                    Vector<long> result = vectorA + vectorB;

                    result.Should().Be(new Vector<long>(200, 100, 50));
                }

                [Test]
                public void When_asked_to_add_4D_vectors()
                {
                    Vector<long> vectorA = new(100, 50, 25, 12);
                    Vector<long> vectorB = new(100, 50, 25, 12);

                    Vector<long> result = vectorA + vectorB;

                    result.Should().Be(new Vector<long>(200, 100, 50, 24));
                }

                [Test]
                public void When_asked_to_add_a_2D_vector_with_a_value()
                {
                    Vector<long> vector = new(100, 50);
                    Vector<long> result = vector + 10;

                    result.Should().Be(new Vector<long>(110, 60, 10, 10));
                }

                [Test]
                public void When_asked_to_add_a_3D_vector_with_a_value()
                {
                    Vector<long> vector = new(100, 50, 25);
                    Vector<long> result = vector + 10;

                    result.Should().Be(new Vector<long>(110, 60, 35, 10));
                }

                [Test]
                public void When_asked_to_add_a_4D_vector_with_a_value()
                {
                    Vector<long> vector = new(100, 50, 25, 12);
                    Vector<long> result = vector + 10;

                    result.Should().Be(new Vector<long>(110, 60, 35, 22));
                }
            }

            public class LongMultiplicationOperator : VectorTestBase
            {
                [Test]
                public void When_asked_to_multiple_2D_vectors()
                {
                    Vector<long> vectorA = new(2, 4);
                    Vector<long> vectorB = new(4, 2);

                    Vector<long> result = vectorA * vectorB;

                    result.Should().Be(new Vector<long>(8, 8));
                }

                [Test]
                public void When_asked_to_multiple_3D_vectors()
                {
                    Vector<long> vectorA = new(2, 4, 6);
                    Vector<long> vectorB = new(2, 4, 6);

                    Vector<long> result = vectorA * vectorB;

                    result.Should().Be(new Vector<long>(4, 16, 36));
                }

                [Test]
                public void When_asked_to_multiple_4D_vectors()
                {
                    Vector<long> vectorA = new(2, 4, 6, 8);
                    Vector<long> vectorB = new(2, 4, 6, 8);

                    Vector<long> result = vectorA * vectorB;

                    result.Should().Be(new Vector<long>(4, 16, 36, 64));
                }

                [Test]
                public void When_asked_to_multiple_a_2D_vector_with_a_value()
                {
                    Vector<long> vector = new(2, 4);
                    Vector<long> result = vector * 10;

                    result.Should().Be(new Vector<long>(20, 40, 0, 0));
                }

                [Test]
                public void When_asked_to_multiple_a_3D_vector_with_a_value()
                {
                    Vector<long> vector = new(2, 4, 6);
                    Vector<long> result = vector * 10;

                    result.Should().Be(new Vector<long>(20, 40, 60, 0));
                }

                [Test]
                public void When_asked_to_multiple_a_4D_vector_with_a_value()
                {
                    Vector<long> vector = new(2, 4, 6, 8);
                    Vector<long> result = vector * 10;

                    result.Should().Be(new Vector<long>(20, 40, 60, 80));
                }
            }

            public class LongDivisionOperator : VectorTestBase
            {
                [Test]
                public void When_asked_to_divide_2D_vectors()
                {
                    Vector<long> vectorA = new(4, 4);
                    Vector<long> vectorB = new(2, 2);

                    Vector<long> result = vectorA / vectorB;

                    result.Should().Be(new Vector<long>(2, 2));
                }

                [Test]
                public void When_asked_to_divide_3D_vectors()
                {
                    Vector<long> vectorA = new(4, 4, 4);
                    Vector<long> vectorB = new(2, 2, 2);

                    Vector<long> result = vectorA / vectorB;

                    result.Should().Be(new Vector<long>(2, 2, 2));
                }

                [Test]
                public void When_asked_to_divide_4D_vectors()
                {
                    Vector<long> vectorA = new(4, 4, 4, 4);
                    Vector<long> vectorB = new(2, 2, 2, 2);

                    Vector<long> result = vectorA / vectorB;

                    result.Should().Be(new Vector<long>(2, 2, 2, 2));
                }

                [Test]
                public void When_asked_to_divide_a_2D_vector_with_a_value()
                {
                    Vector<long> vector = new(2, 4);
                    Vector<long> result = vector / 2;

                    result.Should().Be(new Vector<long>(1, 2, 0, 0));
                }

                [Test]
                public void When_asked_to_divide_a_3D_vector_with_a_value()
                {
                    Vector<long> vector = new(2, 4, 6);
                    Vector<long> result = vector / 2;

                    result.Should().Be(new Vector<long>(1, 2, 3, 0));
                }

                [Test]
                public void When_asked_to_divide_a_4D_vector_with_a_value()
                {
                    Vector<long> vector = new(2, 4, 6, 8);
                    Vector<long> result = vector / 2;

                    result.Should().Be(new Vector<long>(1, 2, 3, 4));
                }
            }
        }

        public class AxisEnumerator : VectorTestBase
        {
            [Test]
            public void When_told_to_get_with_a_width_and_a_height()
            {
                List<Vector<int>> result = Vector<int>.AxisEnumerator(100, 100).ToList();

                result.Count.Should().Be(100 * 100);
                result.First().Should().Be(new Vector<int>(0, 0));
                result.Last().Should().Be(new Vector<int>(99, 99));
            }

            [Test]
            public void When_told_to_get_with_a_start_a_width_and_a_height()
            {
                List<Vector<int>> result = Vector<int>.AxisEnumerator(new Vector<int>(50, 50), 100, 100).ToList();

                result.Count.Should().Be(50 * 50);
                result.First().Should().Be(new Vector<int>(50, 50));
                result.Last().Should().Be(new Vector<int>(99, 99));
            }
        }

        public class Clone : VectorTestBase
        {
            [Test]
            public void When_told_to_clone()
            {
                Vector<int> clone = Subject.Clone();

                bool result = clone == Subject;

                result.Should().BeTrue();
            }
        }

        public class ToTuple : VectorTestBase
        {
            [Test]
            public void When_told_to_create_a_2D_tuple()
            {
                (int X, int Y) result = new Vector<int>(1, 2).ToTuple2D();

                result.X.Should().Be(1);
                result.Y.Should().Be(2);
            }

            [Test]
            public void When_told_to_create_a_3D_tuple()
            {
                (int X, int Y, int Z) result = new Vector<int>(1, 2, 3).ToTuple3D();

                result.X.Should().Be(1);
                result.Y.Should().Be(2);
                result.Z.Should().Be(3);
            }

            [Test]
            public void When_told_to_create_a_4D_tuple()
            {
                (int X, int Y, int Z, int T) result = new Vector<int>(1, 2, 3, 4).ToTuple4D();

                result.X.Should().Be(1);
                result.Y.Should().Be(2);
                result.Z.Should().Be(3);
                result.T.Should().Be(4);
            }
        }

        public class ToKey : VectorTestBase
        {
            [Test]
            public void When_told_to_create_a_2D_key()
            {
                string result = new Vector<int>(1, 2).ToKey2D();

                result.Should().Be($"{1}:{2}");
            }

            [Test]
            public void When_told_to_create_a_3D_key()
            {
                string result = new Vector<int>(1, 2, 3).ToKey3D();

                result.Should().Be($"{1}:{2}:{3}");
            }

            [Test]
            public void When_told_to_create_a_4D_key()
            {
                string result = new Vector<int>(1, 2, 3, 4).ToKey4D();

                result.Should().Be($"{1}:{2}:{3}:{4}");
            }
        }

        public class ToList : VectorTestBase
        {
            [Test]
            public void When_told_to_create_a_2D_list()
            {
                List<int> result = new Vector<int>(1, 2, 3, 4).ToList2D();

                result.Count.Should().Be(2);
                result[0].Should().Be(1);
                result[1].Should().Be(2);
            }

            [Test]
            public void When_told_to_create_a_3D_list()
            {
                List<int> result = new Vector<int>(1, 2, 3, 4).ToList3D();

                result.Count.Should().Be(3);
                result[0].Should().Be(1);
                result[1].Should().Be(2);
                result[2].Should().Be(3);
            }

            [Test]
            public void When_told_to_create_a_4D_list()
            {
                List<int> result = new Vector<int>(1, 2, 3, 4).ToList4D();

                result.Count.Should().Be(4);
                result[0].Should().Be(1);
                result[1].Should().Be(2);
                result[2].Should().Be(3);
                result[3].Should().Be(4);
            }
        }

        public class ToIntArray : VectorTestBase
        {
            [Test]
            public void When_told_to_create_an_int_array()
            {
                int[] result = new Vector<int>(1, 2, 3, 4).ToIntArray();

                result.Length.Should().Be(4);
                result[0].Should().Be(1);
                result[1].Should().Be(2);
                result[2].Should().Be(3);
                result[3].Should().Be(4);
            }
        }

        public class ToLongArray : VectorTestBase
        {
            [Test]
            public void When_told_to_create_a_long_array()
            {
                long[] result = new Vector<long>(1L, 2L, 3L, 4L).ToLongArray();

                result.Length.Should().Be(4);
                result[0].Should().Be(1L);
                result[1].Should().Be(2L);
                result[2].Should().Be(3L);
                result[3].Should().Be(4L);
            }
        }

        public new class ToString : VectorTestBase
        {
            [Test]
            public void When_told_to_format_as_string()
            {
                string result = new Vector<int>(1, 2, 3, 4).ToString();

                result.Should().Be("x: 1, y: 2, z: 3, z: 4");
            }
        }

        public new class Equals : VectorTestBase
        {
            [Test]
            public void When_asked_if_two_matching_vectors_are_equal()
            {
                Vector<int> vectorA = new(100, 50);
                Vector<int> vectorB = new(100, 50);

                bool result = vectorA.Equals(vectorB);

                result.Should().BeTrue();
            }

            [Test]
            public void When_asked_if_two_non_matching_vectors_are_equal()
            {
                Vector<int> vectorA = new(100, 50);
                Vector<int> vectorB = new(50, 100);

                bool result = vectorA.Equals(vectorB);

                result.Should().BeFalse();
            }

            [Test]
            public void When_asked_if_a_vector_is_equal_to_null()
            {
                Vector<int> vectorA = new(100, 50);

                bool result = vectorA.Equals(null);

                result.Should().BeFalse();
            }
        }

        public new class GetHashCode : VectorTestBase
        {
            [Test]
            public void When_told_to_create_a_hash_code()
            {
                int result = new Vector<int>(1, 2, 3, 4).GetHashCode();

                result.Should().Be((1, 2, 3, 4).GetHashCode());
            }
        }

        public class IntAbsolute : VectorTestBase
        {
            [Test]
            public void When_told_to_get_the_absolute_value()
            {
                int result = new Vector<int>(-1, -2, 3, 4).Absolute();

                result.Should().Be(10);
            }
        }

        public class LongAbsolute : VectorTestBase
        {
            [Test]
            public void When_told_to_get_the_absolute_value()
            {
                long result = new Vector<long>(-1L, -2L, 3L, 4L).Absolute();

                result.Should().Be(10L);
            }
        }

        public class IntDistance : VectorTestBase
        {
            [Test]
            public void When_asked_the_manhatten_distance_between_two_2D_vectors()
            {
                Vector<int> vectorA = new(10, 10);
                Vector<int> vectorB = new(20, 20);

                int result = vectorA.Distance(vectorB);

                result.Should().Be(20);
            }

            [Test]
            public void When_asked_the_manhatten_distance_between_two_3D_vectors()
            {
                Vector<int> vectorA = new(10, 10, 10);
                Vector<int> vectorB = new(20, 20, 20);

                int result = vectorA.Distance(vectorB);

                result.Should().Be(30);
            }

            [Test]
            public void When_asked_the_manhatten_distance_between_two_4D_vectors()
            {
                Vector<int> vectorA = new(10, 10, 10, 10);
                Vector<int> vectorB = new(20, 20, 20, 20);

                int result = vectorA.Distance(vectorB);

                result.Should().Be(40);
            }
        }

        public class LongDistance : VectorTestBase
        {
            [Test]
            public void When_asked_the_manhatten_distance_between_two_2D_vectors()
            {
                Vector<long> vectorA = new(10L, 10L);
                Vector<long> vectorB = new(20L, 20L);

                long result = vectorA.Distance(vectorB);

                result.Should().Be(20L);
            }

            [Test]
            public void When_asked_the_manhatten_distance_between_two_3D_vectors()
            {
                Vector<long> vectorA = new(10L, 10L, 10L);
                Vector<long> vectorB = new(20L, 20L, 20L);

                long result = vectorA.Distance(vectorB);

                result.Should().Be(30L);
            }

            [Test]
            public void When_asked_the_manhatten_distance_between_two_4D_vectors()
            {
                Vector<long> vectorA = new(10L, 10L, 10L, 10L);
                Vector<long> vectorB = new(20L, 20L, 20L, 20L);

                long result = vectorA.Distance(vectorB);

                result.Should().Be(40L);
            }
        }

        public class IntNegate : VectorTestBase
        {
            [Test]
            public void When_told_to_negate_a_positive_vector()
            {
                Vector<int> result = new Vector<int>(100, 100).Negate();

                result.X.Should().Be(-100);
                result.Y.Should().Be(-100);
            }

            [Test]
            public void When_told_to_negate_a_negative_vector()
            {
                Vector<int> result = new Vector<int>(-100, -100).Negate();

                result.X.Should().Be(100);
                result.Y.Should().Be(100);
            }
        }

        public class LongNegate : VectorTestBase
        {
            [Test]
            public void When_told_to_negate_a_positive_vector()
            {
                Vector<long> result = new Vector<long>(100L, 100L).Negate();

                result.X.Should().Be(-100L);
                result.Y.Should().Be(-100L);
            }

            [Test]
            public void When_told_to_negate_a_negative_vector()
            {
                Vector<long> result = new Vector<long>(-100L, -100L).Negate();

                result.X.Should().Be(100L);
                result.Y.Should().Be(100L);
            }
        }

        public class IntAbsoluteValues : VectorTestBase
        {
            [Test]
            public void When_told_to_create_a_list_of_the_absolute_coordinates()
            {
                List<int> result = new Vector<int>(1, 2, -3, -4).AbsoluteValues();

                result.Count.Should().Be(4);
                result[0].Should().Be(1);
                result[1].Should().Be(2);
                result[2].Should().Be(3);
                result[3].Should().Be(4);
            }
        }

        public class LongAbsoluteValues : VectorTestBase
        {
            [Test]
            public void When_told_to_create_a_list_of_the_absolute_coordinates()
            {
                List<long> result = new Vector<long>(1L, 2L, -3L, -4L).AbsoluteValues();

                result.Count.Should().Be(4);
                result[0].Should().Be(1L);
                result[1].Should().Be(2L);
                result[2].Should().Be(3L);
                result[3].Should().Be(4L);
            }
        }

        public class IntAround : VectorTestBase
        {
            [Test]
            public void When_told_to_get_the_vectors_around()
            {
                List<Vector<int>> result = Subject.Around(0, 0, 101, 101).ToList();

                result.Count.Should().Be(4);
                result[0].Should().Be(new Vector<int>(101, 100));
                result[1].Should().Be(new Vector<int>(99, 100));
                result[2].Should().Be(new Vector<int>(100, 101));
                result[3].Should().Be(new Vector<int>(100, 99));
            }
        }

        public class LongAround : VectorTestBase
        {
            [Test]
            public void When_told_to_get_the_vectors_around()
            {
                List<Vector<long>> result = new Vector<long>(100, 100).Around(0L, 0L, 101L, 101L).ToList();

                result.Count.Should().Be(4);
                result[0].Should().Be(new Vector<long>(101L, 100L));
                result[1].Should().Be(new Vector<long>(99L, 100L));
                result[2].Should().Be(new Vector<long>(100L, 101L));
                result[3].Should().Be(new Vector<long>(100L, 99L));
            }
        }

        public class IsLineOfSight : VectorTestBase
        {
            [Test]
            public void When_vector_equals_point_a()
            {
                bool result = new Vector<int>(0, 0).IsLineOfSight(new(0, 0), new(100, 100));

                result.Should().BeFalse();
            }

            [Test]
            public void When_vector_equals_point_b()
            {
                bool result = new Vector<int>(0, 0).IsLineOfSight(new(100, 100), new(0, 0));

                result.Should().BeFalse();
            }

            [Test]
            public void When_vector_a_equals_point_b()
            {
                bool result = new Vector<int>(0, 0).IsLineOfSight(new(100, 100), new(100, 100));

                result.Should().BeFalse();
            }

            [Test]
            public void When_all_vectors_are_in_line_of_sight()
            {
                bool result = new Vector<int>(20, 100).IsLineOfSight(new(30, 100), new(50, 100));

                result.Should().BeTrue();
            }
        }

        public class IsPointsInLine : VectorTestBase
        {
            [Test]
            public void When_asked_if_points_are_in_line()
            {
                bool result = Convert.ToBoolean(Subject.InvokePrivateMethod("IsPointsInLine", new Vector<int>(100, 50), new Vector<int>(100, 75)));

                result.Should().BeTrue();
            }


            [Test]
            public void When_asked_if_points_are_not_in_line()
            {
                bool result = Convert.ToBoolean(Subject.InvokePrivateMethod("IsPointsInLine", new Vector<int>(100, 50), new Vector<int>(90, 75)));

                result.Should().BeFalse();
            }
        }

        public class Rotate : VectorTestBase
        {
            [Test]
            public void When_told_to_rotate_90_degrees()
            {
                Vector<int> result = Subject.Rotate(90);

                result.Should().Be(new Vector<int>(-100, 100));
            }

            [Test]
            public void When_told_to_rotate_180_degrees()
            {
                Vector<int> result = Subject.Rotate(180);

                result.Should().Be(new Vector<int>(-100, -100));
            }

            [Test]
            public void When_told_to_rotate_360_degrees()
            {
                Vector<int> result = Subject.Rotate(360);

                result.Should().Be(new Vector<int>(100, 100));
            }
        }

        public class Transform : VectorTestBase
        {
            [Test]
            public void When_told_to_transform_north()
            {
                Subject.Transform(Cardinal.North);

                Subject.Should().Be(new Vector<int>(100, 99));
            }

            [Test]
            public void When_told_to_transform_south()
            {
                Subject.Transform(Cardinal.South);

                Subject.Should().Be(new Vector<int>(100, 101));
            }

            [Test]
            public void When_told_to_transform_west()
            {
                Subject.Transform(Cardinal.West);

                Subject.Should().Be(new Vector<int>(99, 100));
            }

            [Test]
            public void When_told_to_transform_east()
            {
                Subject.Transform(Cardinal.East);

                Subject.Should().Be(new Vector<int>(101, 100));
            }

            [Test]
            public void When_told_to_transform_north_west()
            {
                Subject.Transform(Cardinal.NorthWest);

                Subject.Should().Be(new Vector<int>(99, 99));
            }

            [Test]
            public void When_told_to_transform_north_east()
            {
                Subject.Transform(Cardinal.NorthEast);

                Subject.Should().Be(new Vector<int>(101, 99));
            }

            [Test]
            public void When_told_to_transform_south_west()
            {
                Subject.Transform(Cardinal.SouthWest);

                Subject.Should().Be(new Vector<int>(99, 101));
            }

            [Test]
            public void When_told_to_transform_south_east()
            {
                Subject.Transform(Cardinal.SouthEast);

                Subject.Should().Be(new Vector<int>(101, 101));
            }
        }

        public class SymbolTransform : VectorTestBase
        {
            [Test]
            public void When_told_to_transform_north()
            {
                Subject.SymbolTransform('^');

                Subject.Should().Be(new Vector<int>(100, 99));
            }

            [Test]
            public void When_told_to_transform_lowercase_south()
            {
                Subject.SymbolTransform('v');

                Subject.Should().Be(new Vector<int>(100, 101));
            }

            [Test]
            public void When_told_to_transform_uppercase_south()
            {
                Subject.SymbolTransform('V');

                Subject.Should().Be(new Vector<int>(100, 101));
            }

            [Test]
            public void When_told_to_transform_west()
            {
                Subject.SymbolTransform('<');

                Subject.Should().Be(new Vector<int>(99, 100));
            }

            [Test]
            public void When_told_to_transform_east()
            {
                Subject.SymbolTransform('>');

                Subject.Should().Be(new Vector<int>(101, 100));
            }
        }

        public class LetterTransform : VectorTestBase
        {
            [Test]
            public void When_told_to_transform_north()
            {
                Subject.LetterTransform('U');

                Subject.Should().Be(new Vector<int>(100, 99));
            }

            [Test]
            public void When_told_to_transform_south()
            {
                Subject.LetterTransform('D');

                Subject.Should().Be(new Vector<int>(100, 101));
            }

            [Test]
            public void When_told_to_transform_west()
            {
                Subject.LetterTransform('L');

                Subject.Should().Be(new Vector<int>(99, 100));
            }

            [Test]
            public void When_told_to_transform_east()
            {
                Subject.LetterTransform('R');

                Subject.Should().Be(new Vector<int>(101, 100));
            }
        }

        public class CompassTransform : VectorTestBase
        {
            [Test]
            public void When_told_to_transform_north()
            {
                Subject.CompassTransform("N");

                Subject.Should().Be(new Vector<int>(100, 99));
            }

            [Test]
            public void When_told_to_transform_south()
            {
                Subject.CompassTransform("S");

                Subject.Should().Be(new Vector<int>(100, 101));
            }

            [Test]
            public void When_told_to_transform_west()
            {
                Subject.CompassTransform("W");

                Subject.Should().Be(new Vector<int>(99, 100));
            }

            [Test]
            public void When_told_to_transform_east()
            {
                Subject.CompassTransform("E");

                Subject.Should().Be(new Vector<int>(101, 100));
            }

            [Test]
            public void When_told_to_transform_north_west()
            {
                Subject.CompassTransform("NW");

                Subject.Should().Be(new Vector<int>(99, 99));
            }

            [Test]
            public void When_told_to_transform_north_east()
            {
                Subject.CompassTransform("NE");

                Subject.Should().Be(new Vector<int>(101, 99));
            }

            [Test]
            public void When_told_to_transform_south_west()
            {
                Subject.CompassTransform("SW");

                Subject.Should().Be(new Vector<int>(99, 101));
            }

            [Test]
            public void When_told_to_transform_south_east()
            {
                Subject.CompassTransform("SE");

                Subject.Should().Be(new Vector<int>(101, 101));
            }
        }
    }
}
