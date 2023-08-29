namespace AdventOfCode.Test.Core
{
    using AdventOfCode.Core;
    using AdventOfCode.Core.Extensions;
    using FluentAssertions;
    using NUnit.Framework;

    public class EnumerableExtensionsTestBase
    {
        [SetUp]
        public void Init()
        {
        }
    }

    [TestFixture]
    public class EnumerableExtensionsTest : EnumerableExtensionsTestBase
    {
        public class ToInt : EnumerableExtensionsTestBase
        {
            [Test]
            public void When_told_to_cast_an_array_of_strings_to_int()
            {
                IEnumerable<string> stringValues = new List<string> { "42", "100", "300" };

                int[] result = stringValues.ToInt();

                result[0].Should().Be(42);
                result[1].Should().Be(100);
                result[2].Should().Be(300);
            }

            [Test]
            public void When_told_to_cast_an_array_strings_with_an_invalid_int()
            {
                IEnumerable<string> stringValues = new List<string> { "42", "invalid", "300" };

                Action action = () => stringValues.ToInt();

                action.Should().Throw<FormatException>();
            }
        }

        public class ToUInt : EnumerableExtensionsTestBase
        {
            [Test]
            public void When_told_to_cast_an_array_of_strings_to_uint()
            {
                IEnumerable<string> stringValues = new List<string> { "42", "100", "300" };

                uint[] result = stringValues.ToUInt();

                result[0].Should().Be(42U);
                result[1].Should().Be(100U);
                result[2].Should().Be(300U);
            }

            [Test]
            public void When_told_to_cast_an_array_strings_with_an_invalid_uint()
            {
                IEnumerable<string> stringValues = new List<string> { "42", "invalid", "300" };

                Action action = () => stringValues.ToUInt();

                action.Should().Throw<FormatException>();
            }
        }

        public class ToLong : EnumerableExtensionsTestBase
        {
            [Test]
            public void When_told_to_cast_an_array_of_strings_to_long()
            {
                IEnumerable<string> stringValues = new List<string> { "4200000000", "100", "3000000000" };

                long[] result = stringValues.ToLong();

                result[0].Should().Be(4200000000L);
                result[1].Should().Be(100L);
                result[2].Should().Be(3000000000L);
            }

            [Test]
            public void When_told_to_cast_an_array_strings_with_an_invalid_long()
            {
                IEnumerable<string> stringValues = new List<string> { "42", "invalid", "300" };

                Action action = () => stringValues.ToLong();

                action.Should().Throw<FormatException>();
            }
        }

        public class ToULong : EnumerableExtensionsTestBase
        {
            [Test]
            public void When_told_to_cast_an_array_of_strings_to_ulong()
            {
                IEnumerable<string> stringValues = new List<string> { "4200000000", "100", "3000000000" };

                ulong[] result = stringValues.ToULong();

                result[0].Should().Be(4200000000UL);
                result[1].Should().Be(100UL);
                result[2].Should().Be(3000000000UL);
            }

            [Test]
            public void When_told_to_cast_an_array_strings_with_an_invalid_ulong()
            {
                IEnumerable<string> stringValues = new List<string> { "42", "invalid", "300" };

                Action action = () => stringValues.ToULong();

                action.Should().Throw<FormatException>();
            }
        }

        public class ToFloat : EnumerableExtensionsTestBase
        {
            [Test]
            public void When_told_to_cast_an_array_of_strings_to_float()
            {
                IEnumerable<string> stringValues = new List<string> { "42.5", "100", "3.14159" };

                float[] result = stringValues.ToFloat();

                result[0].Should().Be(42.5f);
                result[1].Should().Be(100f);
                result[2].Should().Be(3.14159f);
            }

            [Test]
            public void When_told_to_cast_an_array_strings_with_an_invalid_float()
            {
                IEnumerable<string> stringValues = new List<string> { "42.5", "invalid", "3.14159" };

                Action action = () => stringValues.ToFloat();

                action.Should().Throw<FormatException>();
            }
        }

        public class ToDouble : EnumerableExtensionsTestBase
        {
            [Test]
            public void When_told_to_cast_an_array_of_strings_to_double()
            {
                IEnumerable<string> stringValues = new List<string> { "42.5", "100", "3.14159" };

                double[] result = stringValues.ToDouble();

                result[0].Should().Be(42.5);
                result[1].Should().Be(100.0);
                result[2].Should().Be(3.14159);
            }

            [Test]
            public void When_told_to_cast_an_array_strings_with_an_invalid_double()
            {
                IEnumerable<string> stringValues = new List<string> { "42.5", "invalid", "3.14159" };

                Action action = () => stringValues.ToDouble();

                action.Should().Throw<FormatException>();
            }
        }

        public class ToDecimal : EnumerableExtensionsTestBase
        {
            [Test]
            public void When_told_to_cast_an_array_of_strings_to_decimal()
            {
                IEnumerable<string> stringValues = new List<string> { "42.5", "100", "3.14159" };

                decimal[] result = stringValues.ToDecimal();

                result[0].Should().Be(42.5m);
                result[1].Should().Be(100m);
                result[2].Should().Be(3.14159m);
            }

            [Test]
            public void When_told_to_cast_an_array_strings_with_an_invalid_decimal()
            {
                IEnumerable<string> stringValues = new List<string> { "42.5", "invalid", "3.14159" };

                Action action = () => stringValues.ToDecimal();

                action.Should().Throw<FormatException>();
            }
        }

        public class ToByte : EnumerableExtensionsTestBase
        {
            [Test]
            public void When_told_to_cast_an_array_of_strings_to_byte()
            {
                IEnumerable<string> stringValues = new List<string> { "42", "100", "200" };

                byte[] result = stringValues.ToByte();

                result[0].Should().Be((byte)42);
                result[1].Should().Be((byte)100);
                result[2].Should().Be((byte)200);
            }

            [Test]
            public void When_told_to_cast_an_array_strings_with_an_invalid_byte()
            {
                IEnumerable<string> stringValues = new List<string> { "42", "invalid", "200" };

                Action action = () => stringValues.ToByte();

                action.Should().Throw<FormatException>();
            }
        }

        public class ToIntList : EnumerableExtensionsTestBase
        {
            [Test]
            public void When_told_to_cast_a_list_of_strings_to_int()
            {
                IEnumerable<string> stringValues = new List<string> { "42", "100", "200" };

                List<int> result = stringValues.ToIntList();

                result[0].Should().Be(42);
                result[1].Should().Be(100);
                result[2].Should().Be(200);
            }

            [Test]
            public void When_told_to_cast_a_list_strings_with_an_invalid_int()
            {
                IEnumerable<string> stringValues = new List<string> { "42", "invalid", "200" };

                Action action = () => stringValues.ToIntList();

                action.Should().Throw<FormatException>();
            }
        }

        public class ToLongList : EnumerableExtensionsTestBase
        {
            [Test]
            public void When_told_to_cast_a_list_of_strings_to_long()
            {
                IEnumerable<string> stringValues = new List<string> { "4200000000", "100", "3000000000" };

                List<long> result = stringValues.ToLongList();

                result[0].Should().Be(4200000000L);
                result[1].Should().Be(100L);
                result[2].Should().Be(3000000000L);
            }

            [Test]
            public void When_told_to_cast_a_list_strings_with_an_invalid_long()
            {
                IEnumerable<string> stringValues = new List<string> { "4200000000", "invalid", "3000000000" };

                Action action = () => stringValues.ToLongList();

                action.Should().Throw<FormatException>();
            }
        }

        public class ToFloatList : EnumerableExtensionsTestBase
        {
            [Test]
            public void When_told_to_cast_a_list_of_strings_to_float()
            {
                IEnumerable<string> stringValues = new List<string> { "4200000000", "100", "3000000000" };

                List<long> result = stringValues.ToLongList();

                result[0].Should().Be(4200000000L);
                result[1].Should().Be(100L);
                result[2].Should().Be(3000000000L);
            }

            [Test]
            public void When_told_to_cast_a_list_strings_with_an_invalid_float()
            {
                IEnumerable<string> stringValues = new List<string> { "4200000000", "invalid", "3000000000" };

                Action action = () => stringValues.ToFloatList();

                action.Should().Throw<FormatException>();
            }
        }

        public class ToDoubleList : EnumerableExtensionsTestBase
        {
            [Test]
            public void When_told_to_cast_a_list_of_strings_to_double()
            {
                IEnumerable<string> stringValues = new List<string> { "42.5", "100", "3.14159" };

                List<double> result = stringValues.ToDoubleList();

                result[0].Should().Be(42.5);
                result[1].Should().Be(100.0);
                result[2].Should().Be(3.14159);
            }

            [Test]
            public void When_told_to_cast_a_list_strings_with_an_invalid_double()
            {
                IEnumerable<string> stringValues = new List<string> { "42.5", "invalid", "3.14159" };

                Action action = () => stringValues.ToDoubleList();

                action.Should().Throw<FormatException>();
            }
        }

        public class ToDecimalList : EnumerableExtensionsTestBase
        {
            [Test]
            public void When_told_to_cast_a_list_of_strings_to_decimal()
            {
                IEnumerable<string> stringValues = new List<string> { "42.5", "100", "3.14159" };

                List<decimal> result = stringValues.ToDecimalList();

                result[0].Should().Be(42.5m);
                result[1].Should().Be(100m);
                result[2].Should().Be(3.14159m);
            }

            [Test]
            public void When_told_to_cast_a_list_strings_with_an_invalid_decimal()
            {
                IEnumerable<string> stringValues = new List<string> { "42.5", "invalid", "3.14159" };

                Action action = () => stringValues.ToDecimalList();

                action.Should().Throw<FormatException>();
            }
        }

        public class ToByteList : EnumerableExtensionsTestBase
        {
            [Test]
            public void When_told_to_cast_a_list_of_strings_to_byte()
            {
                IEnumerable<string> stringValues = new List<string> { "42", "100", "200" };

                List<byte> result = stringValues.ToByteList();

                result[0].Should().Be((byte)42);
                result[1].Should().Be((byte)100);
                result[2].Should().Be((byte)200);
            }

            [Test]
            public void When_told_to_cast_a_list_strings_with_an_invalid_byte()
            {
                IEnumerable<string> stringValues = new List<string> { "42", "invalid", "200" };

                Action action = () => stringValues.ToByteList();

                action.Should().Throw<FormatException>();
            }
        }

        public class Split : EnumerableExtensionsTestBase
        {
            [Test]
            public void When_told_to_split_a_collection_on_a_given_element()
            {
                IEnumerable<int> collection = new List<int> { 1, 2, 0, 3, 0, 4, 5 };

                var result = collection.Split(0).ToList();

                result.Should().HaveCount(3);
                result[0].Should().BeEquivalentTo(new List<int> { 1, 2 });
                result[1].Should().BeEquivalentTo(new List<int> { 3 });
                result[2].Should().BeEquivalentTo(new List<int> { 4, 5 });
            }

            [Test]
            public void When_told_to_split_a_collection_on_a_non_existant_element()
            {
                IEnumerable<int> collection = new List<int> { 1, 2, 3, 4, 5 };

                var result = collection.Split(0).ToList();

                result.Should().HaveCount(1);
                result[0].Should().BeEquivalentTo(new List<int> { 1, 2, 3, 4, 5 });
            }

            [Test]
            public void When_told_to_split_a_collection_that_contains_null_values()
            {
                IEnumerable<int?> collection = new List<int?> { 1, null, 2, null, 3 };

                var result = collection.Split(null).ToList();

                result.Should().HaveCount(1);
                result[0].Should().BeEquivalentTo(new List<int> { 1, 2, 3 });
            }
        }

        public class HashSequence : EnumerableExtensionsTestBase
        {
            [Test]
            public void When_told_to_generate_a_hash_sequence_for_a_list_of_byte()
            {
                IEnumerable<byte> lengths = new List<byte> { 3, 4, 1 };
                int repeat = 2;

                var resultHashes = lengths.HashSequence(repeat).ToList();

                resultHashes.Should().HaveCount(7);
            }
        }

        public class TakeCount : EnumerableExtensionsTestBase
        {
            [Test]
            public void When_told_to_take_count_with_a_valid_in_range_value()
            {
                IEnumerable<int> collection = new List<int> { 1, 2, 3, 4, 5 };

                var result = collection.TakeCount(2).ToList();

                result[0].Should().Be(1);
                result[1].Should().Be(2);
                result[2].Should().Be(3);
            }

            [Test]
            public void When_told_to_take_count_with_a_value_greater_than_the_collections_count()
            {
                IEnumerable<int> collection = new List<int> { 1, 2, 3, 4, 5 };

                var result = collection.TakeCount(10);

                result.Should().BeEmpty();
            }

            [Test]
            public void When_told_to_take_count_with_a_count_of_zero()
            {
                IEnumerable<int> collection = new List<int> { 1, 2, 3, 4, 5 };

                var result = collection.TakeCount(0);

                result.Should().BeEquivalentTo(collection);
            }
        }

        public class Join : EnumerableExtensionsTestBase
        {
            [Test]
            public void When_told_to_join_with_a_non_empty_collection()
            {
                IEnumerable<string> collection = new List<string> { "one", "two", "three" };

                string result = collection.Join(", ");

                result.Should().Be("one, two, three");
            }

            [Test]
            public void When_told_to_join_with_an_empty_collection()
            {
                IEnumerable<string> collection = new List<string>();

                string result = collection.Join(", ");

                result.Should().BeEmpty();
            }

            [Test]
            public void When_told_to_join_with_the_default_seperator()
            {
                IEnumerable<string> collection = new List<string> { "one", "two", "three" };

                string result = collection.Join();

                result.Should().Be("onetwothree");
            }
        }

        public class ForEach : EnumerableExtensionsTestBase
        {
            [Test]
            public void When_told_to_enumerate_the_collection_and_apply_the_given_action()
            {
                IEnumerable<int> collection = new List<int> { 1, 2, 3 };
                List<int> results = new();

                collection.ForEach(x => results.Add(x * 2));

                results[0].Should().Be(2);
                results[1].Should().Be(4);
                results[2].Should().Be(6);
            }

            [Test]
            public void When_told_to_enumerate_an_empty_collection()
            {
                IEnumerable<int> collection = new List<int>();
                bool actionInvoked = false;

                collection.ForEach(x => actionInvoked = true);

                actionInvoked.Should().BeFalse();
            }
        }

        public class Product : EnumerableExtensionsTestBase
        {
            [Test]
            public void When_told_to_return_the_product_of_a_non_empty_collection()
            {
                IEnumerable<int> collection = new List<int> { 2, 3, 4, 5 };

                long result = collection.Product();

                result.Should().Be(2 * 3 * 4 * 5);
            }

            [Test]
            public void When_told_to_return_the_product_of_an_empty_collection()
            {
                IEnumerable<int> collection = new List<int>();

                long result = collection.Product();

                result.Should().Be(0);
            }

            [Test]
            public void When_told_to_return_the_product_of_a_collection_with_a_single_element()
            {
                IEnumerable<int> collection = new List<int> { 7 };

                long result = collection.Product();

                result.Should().Be(7);
            }
        }
    }
}
