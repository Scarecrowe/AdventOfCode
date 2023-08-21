namespace AdventOfCode.Test.Core
{
    using AdventOfCode.Core;
    using FluentAssertions;
    using NUnit.Framework;

    public class MathHelperTestBase : TheSubject<MathHelper>
    {
        [SetUp]
        public void Init()
        {
            Subject = new MathHelper();
        }
    }

    [TestFixture]
    public class MathHelperTest : MathHelperTestBase
    {
        public class GreatestCommonFactor : MathHelperTestBase
        {
            [Test]
            public void When_told_to_get_the_greatest_common_factor()
            {
                MathHelper.GreatestCommonFactor(12, 16).Should().Be(4);
                MathHelper.GreatestCommonFactor(30, 105).Should().Be(15);
                MathHelper.GreatestCommonFactor(24, 108).Should().Be(12);
                MathHelper.GreatestCommonFactor(45, 99).Should().Be(9);
                MathHelper.GreatestCommonFactor(1205, 105).Should().Be(5);
            }
        }

        public class LeastCommonMultiple : MathHelperTestBase
        {
            [Test]
            public void When_told_to_get_the_least_common_multiple()
            {
                MathHelper.LeastCommonMultiple(12, 16).Should().Be(48);
                MathHelper.LeastCommonMultiple(30, 105).Should().Be(210);
                MathHelper.LeastCommonMultiple(24, 108).Should().Be(216);
                MathHelper.LeastCommonMultiple(45, 99).Should().Be(495);
                MathHelper.LeastCommonMultiple(1205, 105).Should().Be(25305);
            }

            [Test]
            public void When_told_to_get_the_least_common_multiple_with_a_null_list()
            {
                Action action = () => MathHelper.LeastCommonMultiple(null);

                action.Should().Throw<ArgumentNullException>();
            }

            [Test]
            public void When_told_to_get_the_least_common_multiple_with_a_list_of_one_item()
            {
                Action action = () => MathHelper.LeastCommonMultiple(new() { 1 } );

                action.Should().Throw<ArgumentOutOfRangeException>();
            }

            [Test]
            public void When_told_to_get_the_least_common_multiple_with_a_list()
            {

                MathHelper.LeastCommonMultiple(new() { 12, 16 }).Should().Be(48);
                MathHelper.LeastCommonMultiple(new() { 30, 105 }).Should().Be(210);
                MathHelper.LeastCommonMultiple(new() { 24, 108 }).Should().Be(216);
                MathHelper.LeastCommonMultiple(new() { 45, 99 }).Should().Be(495);
                MathHelper.LeastCommonMultiple(new() { 1205, 105 }).Should().Be(25305);

                MathHelper.LeastCommonMultiple(new() { 12, 16, 18 }).Should().Be(144);
                MathHelper.LeastCommonMultiple(new() { 46, 105, 240 }).Should().Be(38640);
            }
        }

        public class Min : MathHelperTestBase
        {
            [Test]
            public void When_told_to_get_the_min_value_of_an_int_param_array()
            {
                MathHelper.Min(100, 23, 12, 45, 1203).Should().Be(12);
                MathHelper.Min(45, 12045, 12, 5, 402).Should().Be(5);
                MathHelper.Min(1234, 435, 234, 145, 929).Should().Be(145);
                MathHelper.Min(1, 2, 3, 4, 5).Should().Be(1);
                MathHelper.Min(99, 77).Should().Be(77);
            }

            [Test]
            public void When_told_to_get_the_min_value_of_an_long_param_array()
            {
                MathHelper.Min(100L, 23L, 12L, 45L, 1203L).Should().Be(12L);
                MathHelper.Min(45L, 12045L, 12L, 5L, 402L).Should().Be(5L);
                MathHelper.Min(1234L, 435L, 234L, 145L, 929L).Should().Be(145L);
                MathHelper.Min(1L, 2L, 3L, 4L, 5L).Should().Be(1L);
                MathHelper.Min(99L, 77L).Should().Be(77L);
            }
        }

        public class Max : MathHelperTestBase
        {
            [Test]
            public void When_told_to_get_the_max_value_of_an_int_param_array()
            {
                MathHelper.Max(100, 23, 12, 45, 1203).Should().Be(1203);
                MathHelper.Max(45, 12045, 12, 5, 402).Should().Be(12045);
                MathHelper.Max(1234, 435, 234, 145, 929).Should().Be(1234);
                MathHelper.Max(1, 2, 3, 4, 5).Should().Be(5);
                MathHelper.Max(99, 77).Should().Be(99);
            }

            [Test]
            public void When_told_to_get_the_max_value_of_an_long_param_array()
            {
                MathHelper.Max(100L, 23L, 12L, 45L, 1203L).Should().Be(1203L);
                MathHelper.Max(45L, 12045L, 12L, 5L, 402L).Should().Be(12045L);
                MathHelper.Max(1234L, 435L, 234L, 145L, 929L).Should().Be(1234L);
                MathHelper.Max(1L, 2L, 3L, 4L, 5L).Should().Be(5L);
                MathHelper.Max(99L, 77L).Should().Be(99L);
            }
        }

        public class SurfaceArea : MathHelperTestBase
        {
            [Test]
            public void When_told_to_get_the_surface_area_of_three_sides()
            {
                MathHelper.SurfaceArea(100, 100, 100).Should().Be(60000);
                MathHelper.SurfaceArea(5, 8, 5).Should().Be(210);
                MathHelper.SurfaceArea(1, 1, 1).Should().Be(6);
                MathHelper.SurfaceArea(10, 10, 4).Should().Be(360);
                MathHelper.SurfaceArea(23, 12, 3).Should().Be(762);
            }
        }

        public class SmallestSurface : MathHelperTestBase
        {
            [Test]
            public void When_told_to_get_the_smallest_surface_of_three_sides()
            {
                MathHelper.SmallestSurface(100, 100, 100).Should().Be(10000);
                MathHelper.SmallestSurface(5, 8, 5).Should().Be(25);
                MathHelper.SmallestSurface(1, 1, 1).Should().Be(1);
                MathHelper.SmallestSurface(10, 10, 4).Should().Be(40);
                MathHelper.SmallestSurface(23, 12, 3).Should().Be(36);
            }
        }

        public class Volume : MathHelperTestBase
        {
            [Test]
            public void When_told_to_get_the_volumne()
            {
                MathHelper.Volume(100, 100, 100).Should().Be(1000000);
                MathHelper.Volume(5, 8, 5).Should().Be(200);
                MathHelper.Volume(1, 1, 1).Should().Be(1);
                MathHelper.Volume(10, 10, 4).Should().Be(400);
                MathHelper.Volume(23, 12, 3).Should().Be(828);
            }
        }

        public class SmallestPerimeter : MathHelperTestBase
        {
            [Test]
            public void When_told_to_get_the_smallest_perimeter()
            {
                MathHelper.SmallestPerimeter(100, 100, 100).Should().Be(400);
                MathHelper.SmallestPerimeter(5, 8, 5).Should().Be(20);
                MathHelper.SmallestPerimeter(1, 1, 1).Should().Be(4);
                MathHelper.SmallestPerimeter(10, 10, 4).Should().Be(28);
                MathHelper.SmallestPerimeter(23, 12, 3).Should().Be(30);
            }
        }

        public class IsTriangle : MathHelperTestBase
        {
            [Test]
            public void When_told_to_get_if_three_sides_are_a_triangle()
            {
                MathHelper.IsTriangle(100, 100, 100).Should().BeTrue();
                MathHelper.IsTriangle(5, 9, 1).Should().BeFalse();
                MathHelper.IsTriangle(1, 1, 1).Should().BeTrue();
                MathHelper.IsTriangle(10, 10, 4).Should().BeTrue();
                MathHelper.IsTriangle(23, 12, 3).Should().BeFalse();
            }
        }
    }
}
