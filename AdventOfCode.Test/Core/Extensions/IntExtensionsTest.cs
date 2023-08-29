namespace AdventOfCode.Test.Core
{
    using AdventOfCode.Core.Extensions;
    using FluentAssertions;
    using NUnit.Framework;

    public class IntExtensionsTestBase
    {
        [SetUp]
        public void Init()
        {
        }
    }

    [TestFixture]
    public class IntExtensionsTest : IntExtensionsTestBase
    {
        public class Factorial : IntExtensionsTestBase
        {
            [Test]
            public void When_told_to_calculate_the_factorial()
            {
                int value = 5;

                int result = value.Factorial();

                result.Should().Be(120);
            }

            [Test]
            public void When_told_to_calculate_the_factorial_of_zero()
            {
                int value = 0;

                int result = value.Factorial();

                result.Should().Be(1);
            }
        }

        public class Factors : IntExtensionsTestBase
        {
            [Test]
            public void When_told_to_get_the_factors_of_a_non_prime_number()
            {
                int value = 12;

                List<int> factors = value.Factors().ToList();

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
                int value = 17;

                IEnumerable<int> factors = value.Factors();

                factors.ElementAt(0).Should().Be(1);
                factors.ElementAt(1).Should().Be(17);
            }

            [Test]
            public void When_told_to_get_the_factors_for_one()
            {
                int value = 1;

                IEnumerable<int> factors = value.Factors();

                factors.ElementAt(0).Should().Be(1);
            }
        }

        public class IncrementWrap : IntExtensionsTestBase
        {
            [Test]
            public void When_told_to_increment_wrap_an_int()
            {
                int value = 2;
                int length = 5;

                int result = value.IncrementWrap(length);

                result.Should().Be(1);
            }

            [Test]
            public void When_told_to_increment_wrap_and_the_value_is_zero()
            {
                int value = 0;
                int length = 7;

                int result = value.IncrementWrap(length);

                result.Should().Be(6);
            }
        }

        public class ToGenericInt : IntExtensionsTestBase
        {
            [Test]
            public void When_told_to_cast_to_a_generic_with_a_valid_value()
            {
                int value = 42;

                int intValue = value.ToGeneric<int>();
                long longValue = value.ToGeneric<long>();

                intValue.Should().Be(42);
                longValue.Should().Be(42);
            }

            [Test]
            public void When_told_to_cast_to_a_generic_with_an_invalid_value()
            {
                int value = 42;

                Action stringConversion = () => value.ToGeneric<string>();

                stringConversion.Should().Throw<InvalidCastException>();
            }
        }

        public class ToGenericLong : IntExtensionsTestBase
        {
            [Test]
            public void When_told_to_cast_to_a_generic_with_a_valid_value()
            {
                long value = 42;

                int intValue = value.ToGeneric<int>();
                long longValue = value.ToGeneric<long>();

                intValue.Should().Be(42);
                longValue.Should().Be(42);
            }

            [Test]
            public void When_told_to_cast_to_a_generic_with_an_invalid_value()
            {
                long value = 42;

                Action intConversion = () => value.ToGeneric<string>();

                intConversion.Should().Throw<InvalidCastException>();
            }
        }

        public class ToBinary : IntExtensionsTestBase
        {
            [Test]
            public void When_told_to_cast_an_int_to_binary()
            {
                int value = 42;

                string binaryString = value.ToBinary();

                binaryString.Should().Be("101010");
            }
        }

        public new class ToString : IntExtensionsTestBase
        {
            [Test]
            public void When_told_to_cast_to_various_number_systems()
            {
                int value = 42;

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
                int positiveValue = 42;
                int negativeValue = -42;

                int positiveAbs = positiveValue.Abs();
                int negativeAbs = negativeValue.Abs();

                positiveAbs.Should().Be(42);
                negativeAbs.Should().Be(42);
            }

            [Test]
            public void When_told_to_return_the_absolute_value_of_zero()
            {
                int zeroValue = 0;

                int zeroAbs = zeroValue.Abs();

                zeroAbs.Should().Be(0);
            }
        }

        public class Toggle : IntExtensionsTestBase
        {
            [Test]
            public void When_told_to_toggle_between_zero_and_one()
            {
                int value1 = 0;
                int value2 = 1;

                int toggledValue1 = value1.Toggle();
                int toggledValue2 = value2.Toggle();

                toggledValue1.Should().Be(1);
                toggledValue2.Should().Be(0);
            }
        }

        public class ZeroIfNegative : IntExtensionsTestBase
        {
            [Test]
            public void When_told_to_set_to_zero_if_negative()
            {
                int negativeValue = -42;
                int positiveValue = 42;
                int zeroValue = 0;

                int resultNegative = negativeValue.ZeroIfNegative();
                int resultPositive = positiveValue.ZeroIfNegative();
                int resultZero = zeroValue.ZeroIfNegative();

                resultNegative.Should().Be(0);
                resultPositive.Should().Be(42);
                resultZero.Should().Be(0);
            }
        }
    }
}
