namespace AdventOfCode.Test.Core
{
    using AdventOfCode.Core.Extensions;
    using FluentAssertions;
    using NUnit.Framework;

    public class StringArrayExtensionsTestBase
    {
        [SetUp]
        public void Init()
        {
        }
    }

    [TestFixture]
    public class StringArrayExtensionsTest : StringArrayExtensionsTestBase
    {
        public class ToInt : StringArrayExtensionsTestBase
        {
            [Test]
            public void When_told_to_cast_a_string_array_to_an_int_array()
            {
                string[] stringArray = { "1", "2", "3", "4", "5" };

                int[] intArray = stringArray.ToInt();

                intArray[0].Should().Be(1);
                intArray[1].Should().Be(2);
                intArray[2].Should().Be(3);
                intArray[3].Should().Be(4);
                intArray[4].Should().Be(5);
            }

            [Test]
            public void When_told_to_cast_an_empty_string_array_to_an_int_array()
            {
                string[] stringArray = Array.Empty<string>();

                int[] intArray = stringArray.ToInt();

                intArray.Should().BeEmpty();
            }
        }

        public class ToUInt : StringArrayExtensionsTestBase
        {
            [Test]
            public void When_told_to_cast_a_string_array_to_a_uint_array()
            {
                string[] stringArray = { "1", "200", "3000", "40000", "500000" };

                uint[] uintArray = stringArray.ToUInt();

                uintArray[0].Should().Be(1u);
                uintArray[1].Should().Be(200u);
                uintArray[2].Should().Be(3000u);
                uintArray[3].Should().Be(40000u);
                uintArray[4].Should().Be(500000u);
            }

            [Test]
            public void When_told_to_cast_an_empty_string_array_to_a_uint_array()
            {
                string[] stringArray = Array.Empty<string>();

                uint[] uintArray = stringArray.ToUInt();

                uintArray.Should().BeEmpty();
            }
        }

        public class ToLong : StringArrayExtensionsTestBase
        {
            [Test]
            public void When_told_to_cast_a_string_array_to_a_long_array()
            {
                string[] stringArray = { "100", "20000", "3000000", "400000000", "50000000000" };

                long[] longArray = stringArray.ToLong();

                longArray[0].Should().Be(100L);
                longArray[1].Should().Be(20000L);
                longArray[2].Should().Be(3000000L);
                longArray[3].Should().Be(400000000L);
                longArray[4].Should().Be(50000000000L);
            }

            [Test]
            public void When_told_to_cast_an_empty_string_array_to_a_long_array()
            {
                string[] stringArray = Array.Empty<string>();

                long[] longArray = stringArray.ToLong();

                longArray.Should().BeEmpty();
            }
        }

        public class ToULong : StringArrayExtensionsTestBase
        {
            [Test]
            public void When_told_to_cast_a_string_array_to_a_ulong_array()
            {
                string[] stringArray = { "100", "20000", "3000000", "400000000", "50000000000" };

                ulong[] ulongArray = stringArray.ToULong();

                ulongArray[0].Should().Be(100UL);
                ulongArray[1].Should().Be(20000UL);
                ulongArray[2].Should().Be(3000000UL);
                ulongArray[3].Should().Be(400000000UL);
                ulongArray[4].Should().Be(50000000000UL);
            }

            [Test]
            public void When_told_to_cast_an_empty_string_array_to_a_ulong_array()
            {
                string[] stringArray = Array.Empty<string>();

                ulong[] ulongArray = stringArray.ToULong();

                ulongArray.Should().BeEmpty();
            }
        }

        public class ToFloat : StringArrayExtensionsTestBase
        {
            [Test]
            public void When_told_to_cast_a_string_array_to_a_float_array()
            {
                string[] stringArray = { "1.5", "2.25", "3.75", "4.5", "5" };

                float[] floatArray = stringArray.ToFloat();

                floatArray[0].Should().Be(1.5f);
                floatArray[1].Should().Be(2.25f);
                floatArray[2].Should().Be(3.75f);
                floatArray[3].Should().Be(4.5f);
                floatArray[4].Should().Be(5f);
            }

            [Test]
            public void When_told_to_cast_an_empty_string_array_to_a_float_array()
            {
                string[] stringArray = Array.Empty<string>();

                float[] floatArray = stringArray.ToFloat();

                floatArray.Should().BeEmpty();
            }
        }

        public class ToDouble : StringArrayExtensionsTestBase
        {
            [Test]
            public void When_told_to_cast_a_string_array_to_a_double_array()
            {
                string[] stringArray = { "1.5", "2.25", "3.75", "4.5", "5" };

                double[] doubleArray = stringArray.ToDouble();

                doubleArray[0].Should().Be(1.5);
                doubleArray[1].Should().Be(2.25);
                doubleArray[2].Should().Be(3.75);
                doubleArray[3].Should().Be(4.5);
                doubleArray[4].Should().Be(5.0);
            }

            [Test]
            public void When_told_to_cast_an_empty_string_array_to_a_double_array()
            {
                string[] stringArray = Array.Empty<string>();

                double[] doubleArray = stringArray.ToDouble();

                doubleArray.Should().BeEmpty();
            }
        }

        public class ToDecimal : StringArrayExtensionsTestBase
        {
            [Test]
            public void When_told_to_cast_a_string_array_to_a_decimal_array()
            {
                string[] stringArray = { "1.5", "2.25", "3.75", "4.5", "5" };

                decimal[] decimalArray = stringArray.ToDecimal();

                decimalArray[0].Should().Be(1.5m);
                decimalArray[1].Should().Be(2.25m);
                decimalArray[2].Should().Be(3.75m);
                decimalArray[3].Should().Be(4.5m);
                decimalArray[4].Should().Be(5.0m);
            }

            [Test]
            public void When_told_to_cast_an_empty_string_array_to_a_decimal_array()
            {
                string[] stringArray = Array.Empty<string>();

                decimal[] decimalArray = stringArray.ToDecimal();

                decimalArray.Should().BeEmpty();
            }
        }

        public class ToByte : StringArrayExtensionsTestBase
        {
            [Test]
            public void When_told_to_cast_a_string_array_to_a_byte_array()
            {
                string[] stringArray = { "1", "2", "255", "0", "128" };

                byte[] byteArray = stringArray.ToByte();

                byteArray[0].Should().Be(1);
                byteArray[1].Should().Be(2);
                byteArray[2].Should().Be(255);
                byteArray[3].Should().Be(0);
                byteArray[4].Should().Be(128);
            }

            [Test]
            public void When_told_to_cast_an_empty_string_array_to_a_byte_array()
            {
                string[] stringArray = Array.Empty<string>();

                byte[] byteArray = stringArray.ToByte();

                byteArray.Should().BeEmpty();
            }
        }

        public class ToIntList : StringArrayExtensionsTestBase
        {
            [Test]
            public void When_told_to_cast_a_string_array_to_an_int_list()
            {
                string[] stringArray = { "1", "2", "3", "4", "5" };

                List<int> intList = stringArray.ToIntList();

                intList[0].Should().Be(1);
                intList[1].Should().Be(2);
                intList[2].Should().Be(3);
                intList[3].Should().Be(4);
                intList[4].Should().Be(5);
            }

            [Test]
            public void When_told_to_cast_an_empty_string_array_to_an_int_list()
            {
                string[] stringArray = Array.Empty<string>();

                List<int> intList = stringArray.ToIntList();

                intList.Should().BeEmpty();
            }
        }

        public class ToLongList : StringArrayExtensionsTestBase
        {
            [Test]
            public void When_told_to_cast_a_string_array_to_a_long_list()
            {
                string[] stringArray = { "100", "20000", "3000000", "400000000", "50000000000" };

                List<long> longList = stringArray.ToLongList();

                longList[0].Should().Be(100L);
                longList[1].Should().Be(20000L);
                longList[2].Should().Be(3000000L);
                longList[3].Should().Be(400000000L);
                longList[4].Should().Be(50000000000L);
            }

            [Test]
            public void When_told_to_cast_an_empty_string_array_to_a_long_list()
            {
                string[] stringArray = Array.Empty<string>();

                List<long> longList = stringArray.ToLongList();

                longList.Should().BeEmpty();
            }
        }

        public class ToFloatList : StringArrayExtensionsTestBase
        {
            [Test]
            public void When_told_to_cast_a_string_array_to_a_float_list()
            {
                string[] stringArray = { "1.5", "2.25", "3.75", "4.5", "5" };

                List<float> floatList = stringArray.ToFloatList();

                floatList[0].Should().Be(1.5f);
                floatList[1].Should().Be(2.25f);
                floatList[2].Should().Be(3.75f);
                floatList[3].Should().Be(4.5f);
                floatList[4].Should().Be(5f);
            }

            [Test]
            public void When_told_to_cast_an_empty_string_array_to_a_float_list()
            {
                string[] stringArray = Array.Empty<string>();

                List<float> floatList = stringArray.ToFloatList();

                floatList.Should().BeEmpty();
            }
        }

        public class ToDoubleList : StringArrayExtensionsTestBase
        {
            [Test]
            public void When_told_to_cast_a_string_array_to_a_double_list()
            {
                string[] stringArray = { "1.5", "2.25", "3.75", "4.5", "5" };

                List<double> doubleList = stringArray.ToDoubleList();

                doubleList[0].Should().Be(1.5f);
                doubleList[1].Should().Be(2.25f);
                doubleList[2].Should().Be(3.75f);
                doubleList[3].Should().Be(4.5f);
                doubleList[4].Should().Be(5f);
            }

            [Test]
            public void When_told_to_cast_an_empty_string_array_to_a_double_list()
            {
                string[] stringArray = Array.Empty<string>();

                List<double> doubleList = stringArray.ToDoubleList();

                doubleList.Should().BeEmpty();
            }
        }

        public class ToDecimalList : StringArrayExtensionsTestBase
        {
            [Test]
            public void When_told_to_cast_a_string_array_to_a_decimal_list()
            {
                string[] stringArray = { "1.5", "2.25", "3.75", "4.5", "5" };

                List<decimal> decimalList = stringArray.ToDecimalList();

                decimalList[0].Should().Be(1.5m);
                decimalList[1].Should().Be(2.25m);
                decimalList[2].Should().Be(3.75m);
                decimalList[3].Should().Be(4.5m);
                decimalList[4].Should().Be(5.0m);
            }

            [Test]
            public void When_told_to_cast_an_empty_string_array_to_a_decimal_list()
            {
                string[] stringArray = Array.Empty<string>();

                List<decimal> decimalList = stringArray.ToDecimalList();

                decimalList.Should().BeEmpty();
            }
        }

        public class ToByteList : StringArrayExtensionsTestBase
        {
            [Test]
            public void When_told_to_cast_a_string_array_to_a_byte_list()
            {
                string[] stringArray = { "1", "2", "255", "0", "128" };

                List<byte> byteList = stringArray.ToByteList();

                byteList[0].Should().Be(1);
                byteList[1].Should().Be(2);
                byteList[2].Should().Be(255);
                byteList[3].Should().Be(0);
                byteList[4].Should().Be(128);
            }

            [Test]
            public void When_told_to_cast_an_empty_string_array_to_a_byte_list()
            {
                string[] stringArray = Array.Empty<string>();

                List<byte> byteList = stringArray.ToByteList();

                byteList.Should().BeEmpty();
            }
        }

        public class ForEach : StringArrayExtensionsTestBase
        {
            [Test]
            public void When_told_to_enumerate_an_apply_a_given_actions()
            {
                int[] intArray = { 1, 2, 3, 4, 5 };
                int sum = 0;

                intArray.ForEach(value => sum += value);

                sum.Should().Be(15);
            }

            [Test]
            public void When_told_to_enumerate_an_empty_collection()
            {
                int[] intArray = Array.Empty<int>();
                int sum = 0;

                intArray.ForEach(value => sum += value);

                sum.Should().Be(0);
            }
        }
    }
}
