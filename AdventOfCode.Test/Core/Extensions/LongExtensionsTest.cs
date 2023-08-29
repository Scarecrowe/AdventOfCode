namespace AdventOfCode.Test.Core
{
    using AdventOfCode.Core.Extensions;
    using FluentAssertions;
    using NUnit.Framework;

    public class LongExtensionsTestBase
    {
        [SetUp]
        public void Init()
        {
        }
    }

    [TestFixture]
    public class LongExtensionsTest : LongExtensionsTestBase
    {
        public class Factorial : LongExtensionsTestBase
        {
            [Test]
            public void When_told_to_calculate_the_factorial()
            {
                long value = 5;

                long result = value.Factorial();

                result.Should().Be(120);
            }

            [Test]
            public void When_told_to_calculate_the_factorial_of_zero()
            {
                long value = 0;

                long result = value.Factorial();

                result.Should().Be(1);
            }
        }

        public class Factors : LongExtensionsTestBase
        {
            [Test]
            public void When_told_to_get_the_factors_of_a_non_prime_number()
            {
                long value = 12;

                List<long> factors = value.Factors().ToList();

                factors[0].Should().Be(1);
                factors[1].Should().Be(2);
                factors[2].Should().Be(3);
                factors[3].Should().Be(4);
                factors[4].Should().Be(6);
                factors[5].Should().Be(12);
            }

            [Test]
            public void When_told_to_get_the_factors_of_a_prime_number()
            {
                long value = 17;

                IEnumerable<long> factors = value.Factors();

                factors.ElementAt(0).Should().Be(1);
                factors.ElementAt(1).Should().Be(17);
            }

            [Test]
            public void When_told_to_get_the_factors_for_one()
            {
                long value = 1;

                IEnumerable<long> factors = value.Factors();

                factors.ElementAt(0).Should().Be(1);
            }
        }

        public class IncrementWrap : IntExtensionsTestBase
        {
            [Test]
            public void When_told_to_increment_wrap_a_long()
            {
                long value = 2;
                int length = 5;

                long result = value.IncrementWrap(length);

                result.Should().Be(1);
            }

            [Test]
            public void When_told_to_increment_wrap_and_the_value_is_zero()
            {
                long value = 0;
                int length = 7;

                long result = value.IncrementWrap(length);

                result.Should().Be(6);
            }
        }

        public class ToBinary : IntExtensionsTestBase
        {
            [Test]
            public void When_told_to_cast_an_int_to_binary()
            {
                long value = 42;

                string binaryString = value.ToBinary();

                binaryString.Should().Be("101010");
            }
        }

        public new class ToString : IntExtensionsTestBase
        {
            [Test]
            public void When_told_to_cast_to_various_number_systems()
            {
                long value = 42;

                string base2String = value.ToString(2);
                string base8String = value.ToString(8);
                string base16String = value.ToString(16);

                base2String.Should().Be("101010");
                base8String.Should().Be("52");
                base16String.Should().Be("2a");
            }
        }

        public class Abs : IntExtensionsTestBase
        {
            [Test]
            public void When_told_to_return_the_absolute_value()
            {
                long positiveValue = 42;
                long negativeValue = -42;

                long positiveAbs = positiveValue.Abs();
                long negativeAbs = negativeValue.Abs();

                positiveAbs.Should().Be(42);
                negativeAbs.Should().Be(42);
            }

            [Test]
            public void When_told_to_return_the_absolute_value_of_zero()
            {
                long zeroValue = 0;

                long zeroAbs = zeroValue.Abs();

                zeroAbs.Should().Be(0);
            }
        }
    }
}
