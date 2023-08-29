namespace AdventOfCode.Test.Core
{
    using AdventOfCode.Core.Extensions;
    using FluentAssertions;
    using NUnit.Framework;

    public class IntArrayExtensionsTestBase
    {
        [SetUp]
        public void Init()
        {
        }
    }

    [TestFixture]
    public class IntArrayExtensionsTest : IntArrayExtensionsTestBase
    {
        public class RotateClockWise : IntArrayExtensionsTestBase
        {
            [Test]
            public void When_told_to_rotate_an_int_array_clock_wise()
            {
                int[,] matrix = new int[,]
                {
                    { 1, 2, 3 },
                    { 4, 5, 6 },
                    { 7, 8, 9 }
                };

                int[,] expectedRotatedMatrix = new int[,]
                {
                    { 7, 4, 1 },
                    { 8, 5, 2 },
                    { 9, 6, 3 }
                };

                int[,] result = matrix.RotateClockWise(3);

                result.Should().BeEquivalentTo(expectedRotatedMatrix);
            }
        }

        public class FlipHorizontally : IntArrayExtensionsTestBase
        {
            [Test]
            public void When_told_to_flip_an_int_array_horizonatally()
            {
                int[,] matrix = new int[,]
                {
                    { 1, 2, 3 },
                    { 4, 5, 6 },
                    { 7, 8, 9 }
                };

                int[,] expectedFlippedMatrix = new int[,]
                {
                    { 3, 2, 1 },
                    { 6, 5, 4 },
                    { 9, 8, 7 }
                };

                int[,] result = matrix.FlipHorizontally(3);

                result.Should().BeEquivalentTo(expectedFlippedMatrix);
            }
        }

        public class FlipVertically : IntArrayExtensionsTestBase
        {
            [Test]
            public void When_told_to_flip_an_int_array_vertically()
            {
                int[,] matrix = new int[,]
                {
                    { 1, 2, 3 },
                    { 4, 5, 6 },
                    { 7, 8, 9 }
                };

                int[,] expectedFlippedMatrix = new int[,]
                {
                    { 7, 8, 9 },
                    { 4, 5, 6 },
                    { 1, 2, 3 }
                };

                int[,] result = matrix.FlipVertically(3);

                result.Should().BeEquivalentTo(expectedFlippedMatrix);
            }
        }

        public class Trim : IntArrayExtensionsTestBase
        {
            [Test]
            public void When_told_to_trim_with_a_specified_row_and_column()
            {
                int[,] matrix = new int[,]
                {
                    { 1, 2, 3 },
                    { 4, 5, 6 },
                    { 7, 8, 9 }
                };

                int[,] expectedTrimmedMatrix = new int[,]
                {
                    { 1, 3 },
                    { 7, 9 }
                };

                int[,] result = matrix.Trim(1, 1);

                result.Should().BeEquivalentTo(expectedTrimmedMatrix);
            }

            [Test]
            public void When_told_to_trim_with_a_single_element()
            {
                int[,] matrix = new int[,]
                {
                    { 42 }
                };

                int[,] expectedTrimmedMatrix = new int[0, 0];

                int[,] result = matrix.Trim(0, 0);

                result.Should().BeEquivalentTo(expectedTrimmedMatrix);
            }
        }

        public class SumByIndex : IntArrayExtensionsTestBase
        {
            [Test]
            public void When_told_to_sum_an_int_array_with_a_given_set_of_indexes()
            {
                int[] array = new int[] { 2, 5, 8, 11, 14 };

                int result = array.SumByIndex(1, 3);

                result.Should().Be(16);
            }

            [Test]
            public void When_told_to_sum_an_empty_int_array()
            {
                int[] array = Array.Empty<int>();

                int result = array.SumByIndex(0, 1, 2);

                result.Should().Be(0);
            }
        }

        public class ProductByIndex : IntArrayExtensionsTestBase
        {
            [Test]
            public void When_told_to_product_an_int_array_with_a_given_set_of_indexes()
            {
                int[] array = new int[] { 2, 3, 4, 5 };

                int result = array.ProductByIndex(1, 3);

                result.Should().Be(15);
            }

            [Test]
            public void When_told_to_product_an_empty_int_array()
            {
                int[] array = new int[0];

                int result = array.ProductByIndex(0, 1, 2);

                result.Should().Be(0);
            }
        }

        public class SumRange : IntArrayExtensionsTestBase
        {
            [Test]
            public void When_told_to_sum_a_range_from_an_int_array_with_a_given_range()
            {
                int[] array = new int[] { 2, 5, 8, 11, 14 };

                int result = array.SumRange(1, 3);

                result.Should().Be(24);
            }

            [Test]
            public void When_told_to_sum_a_range_from_an_int_array_when_the_start_equals_the_end()
            {
                int[] array = new int[] { 2, 5, 8, 11, 14 };

                int result = array.SumRange(2, 2);

                result.Should().Be(8);
            }

            [Test]
            public void When_told_to_sum_a_range_that_is_out_of_range()
            {
                int[] array = new int[] { 2, 5, 8, 11, 14 };

                int result = array.SumRange(5, 10);

                result.Should().Be(0);
            }
        }

        public class ProductRange : IntArrayExtensionsTestBase
        {
            [Test]
            public void When_told_to_product_a_range_from_an_int_array_with_a_given_range()
            {
                int[] array = new int[] { 2, 3, 4, 5 };

                int result = array.ProductRange(1, 3);

                result.Should().Be(60);
            }

            [Test]
            public void When_told_to_product_a_range_from_an_int_array_when_the_start_equals_the_end()
            {
                int[] array = new int[] { 2, 3, 4, 5 };

                int result = array.ProductRange(2, 2);

                result.Should().Be(4);
            }

            [Test]
            public void When_told_to_product_a_range_that_is_out_of_range()
            {
                int[] array = new int[] { 2, 3, 4, 5 };

                int result = array.ProductRange(1, 10);

                result.Should().Be(60);
            }

            [Test]
            public void When_told_to_product_a_range_with_an_empty_int_array()
            {
                int[] array = Array.Empty<int>();

                int result = array.ProductRange(1, 10);

                result.Should().Be(0);
            }
        }
    }
}
