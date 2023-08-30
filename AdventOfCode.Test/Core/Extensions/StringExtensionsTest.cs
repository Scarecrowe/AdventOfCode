namespace AdventOfCode.Test.Core
{
    using AdventOfCode.Core.Extensions;
    using FluentAssertions;
    using NUnit.Framework;

    public class StringExtensionsTestBase
    {
        [SetUp]
        public void Init()
        {
        }
    }

    [TestFixture]
    public class StringExtensionsTest : StringExtensionsTestBase
    {
        public class ToMd5 : StringExtensionsTestBase
        {
            [Test]
            public void When_told_to_md5_hash_a_string()
            {
                string input = "Hello, Advent Of Code!";
                string expectedMd5 = "6C2D886D9B7D453820FA28D994BEC050";

                string actualMd5 = input.ToMd5();

                actualMd5.Should().Be(expectedMd5);
            }

            [Test]
            public void When_told_to_md5_hash_an_empty_string()
            {
                string input = "";
                string expectedMd5 = "D41D8CD98F00B204E9800998ECF8427E";

                string actualMd5 = input.ToMd5();

                actualMd5.Should().Be(expectedMd5);
            }
        }

        public class SplitSpace : StringExtensionsTestBase
        {
            [Test]
            public void When_told_to_split_a_string_via_a_space_with_no_empty_entries()
            {
                string input = "Hello  world! This  is a  test";

                string[] result = input.SplitSpace();

                result.Should().BeEquivalentTo("Hello", "world!", "This", "is", "a", "test");
            }

            [Test]
            public void When_told_to_split_a_string_via_a_space_with_empty_entries()
            {
                string input = "Hello  world! This  is a  test";

                string[] result = input.SplitSpace(StringSplitOptions.None);

                result.Should().BeEquivalentTo("Hello", "", "world!", "This", "", "is", "a", "", "test");
            }

            [Test]
            public void When_told_to_split_a_string_via_a_space_with_an_empty_string()
            {
                string input = "";

                string[] result = input.SplitSpace();

                result.Should().BeEmpty();
            }
        }

        public class SplitComma : StringExtensionsTestBase
        {
            [Test]
            public void When_told_to_split_a_string_via_a_comma_with_no_empty_entries()
            {
                string input = "apple,banana,orange";

                string[] result = input.SplitComma();

                result.Should().BeEquivalentTo("apple", "banana", "orange");
            }

            [Test]
            public void When_told_to_split_a_string_via_a_comma_with_empty_entries()
            {
                string input = "apple,,banana,orange,";

                string[] result = input.SplitComma(StringSplitOptions.None);

                result.Should().BeEquivalentTo("apple", "", "banana", "orange", "");
            }

            [Test]
            public void When_told_to_split_a_string_via_a_comma_with_an_empty_string()
            {
                string input = "";

                string[] result = input.SplitComma();

                result.Should().BeEmpty();
            }
        }

        public class SwapPosition : StringExtensionsTestBase
        {
            [Test]
            public void When_told_to_swap_the_positions_of_two_indexes()
            {
                string input = "abcdef";

                string result = input.SwapPosition(1, 4);

                result.Should().Be("aecdbf");
            }

            [Test]
            public void When_told_to_swap_the_position_of_the_same_index()
            {
                string input = "xyz";

                string result = input.SwapPosition(0, 0);

                result.Should().Be("xyz");
            }

            [Test]
            public void When_told_to_swap_the_position_of_an_out_of_range_index()
            {
                string input = "abc";

                string result = input.SwapPosition(0, 5);

                result.Should().Be("abc");
            }
        }

        public class SwapLetter : StringExtensionsTestBase
        {
            [Test]
            public void When_told_to_swap_a_letter_when_the_string_contains_both()
            {
                string input = "abcdef";

                string result = input.SwapLetter('b', 'e');

                result.Should().Be("aecdbf");
            }

            [Test]
            public void When_told_to_swap_the_same_letter()
            {
                string input = "xyz";

                string result = input.SwapLetter('x', 'x');

                result.Should().Be("xyz");
            }

            [Test]
            public void When_told_to_swap_a_non_existant_letter()
            {
                string input = "abc";

                string result = input.SwapLetter('x', 'y');

                result.Should().Be("abc");
            }
        }

        public class RotateLeft : StringExtensionsTestBase
        {
            [Test]
            public void When_told_to_rotate_left()
            {
                string input = "abcdef";

                string result = input.RotateLeft(2);

                result.Should().Be("cdefab");
            }

            [Test]
            public void When_told_to_rotate_left_with_a_length_greater_than_the_string()
            {
                string input = "xyz";

                string result = input.RotateLeft(5);

                result.Should().Be("xyz");
            }

            [Test]
            public void When_told_to_rotate_left_with_a_negative_value()
            {
                string input = "abc";

                string result = input.RotateLeft(-1);

                result.Should().Be("abc");
            }
        }

        public class RotateRight : StringExtensionsTestBase
        {
            [Test]
            public void When_told_to_rotate_right()
            {
                string input = "abcdef";

                string result = input.RotateRight(2);

                result.Should().Be("efabcd");
            }

            [Test]
            public void When_told_to_rotate_right_with_a_length_greater_than_the_string()
            {
                string input = "xyz";

                string result = input.RotateRight(5);

                result.Should().Be("xyz");
            }

            [Test]
            public void When_told_to_rotate_right_with_a_negative_value()
            {
                string input = "abc";

                string result = input.RotateRight(-1);

                result.Should().Be("abc");
            }
        }

        public class RotateAroundChar : StringExtensionsTestBase
        {
            [Test]
            public void When_told_to_rotate_around_a_char()
            {
                string input = "abcdefg";

                string result = input.RotateAroundChar('c');

                result.Should().Be("efgabcd");
            }

            [Test]
            public void When_told_to_rotate_around_a_non_existant_char()
            {
                string input = "xyz";

                string result = input.RotateAroundChar('a');

                result.Should().Be("xyz");
            }
        }

        public class Reverse : StringExtensionsTestBase
        {
            [Test]
            public void When_told_to_reverse_a_string_for_a_given_range()
            {
                string input = "abcdefg";

                string result = input.Reverse(1, 4);

                result.Should().Be("aedcbfg");
            }

            [Test]
            public void When_told_to_reverse_a_string_for_the_same_index()
            {
                string input = "xyz";

                string result = input.Reverse(1, 1);

                result.Should().Be("xyz");
            }
        }

        public class Move : StringExtensionsTestBase
        {
            [Test]
            public void When_told_to_move_with_a_given_range()
            {
                string input = "abcdefg";

                string result = input.Move(1, 4);

                result.Should().Be("acdebfg");
            }

            [Test]
            public void When_told_to_move_with_the_same_index()
            {
                string input = "xyz";

                string result = input.Move(1, 1);

                result.Should().Be("xyz");
            }
        }

        public class Remove : StringExtensionsTestBase
        {
            [Test]
            public void When_told_to_remove_for_an_empty_string()
            {
                string input = "Hello, world! This is a test.";

                string result = input.Remove("world!");

                result.Should().Be("Hello,  This is a test.");
            }

            [Test]
            public void When_told_to_remove_an_empty_string()
            {
                string input = "Hello, world! This is a test.";

                string result = input.Remove(string.Empty);

                result.Should().Be("Hello, world! This is a test.");
            }

            [Test]
            public void When_told_to_remove_a_non_existant_string()
            {
                string input = "Hello, world! This is a test.";

                string result = input.Remove("foo");

                result.Should().Be("Hello, world! This is a test.");
            }
        }

        public class Strip : StringExtensionsTestBase
        {
            [Test]
            public void When_told_to_strip()
            {
                string input = "  Hello, world!  ";

                string result = input.Strip(' ');

                result.Should().Be("Hello, world!");
            }

            [Test]
            public void When_told_to_strip_with_a_non_existant_char()
            {
                string input = "Hello, world!";

                string result = input.Strip(' ');

                result.Should().Be("Hello, world!");
            }

            [Test]
            public void When_told_to_strip_an_empty_string()
            {
                string input = string.Empty;

                string result = input.Strip(' ');

                result.Should().Be(string.Empty);
            }

            [Test]
            public void When_told_to_strip_only_from_the_start()
            {
                string input = "  Hello, world!";

                string result = input.Strip(' ');

                result.Should().Be("Hello, world!");
            }

            [Test]
            public void When_told_to_strip_only_from_the_end()
            {
                string input = "Hello, world!  ";

                string result = input.Strip(' ');

                result.Should().Be("Hello, world!");
            }
        }

        public class In : StringExtensionsTestBase
        {
            [Test]
            public void When_asked_whats_in_with_all_matching_chars()
            {
                string input = "abcdef";

                string result = input.In("ace");

                result.Should().Be("ace");
            }

            [Test]
            public void When_asked_whats_in_on_an_empty_string()
            {
                string input = string.Empty;

                string result = input.In("ace");

                result.Should().Be(string.Empty);
            }

            [Test]
            public void When_asked_whats_in_with_an_empty_string()
            {
                string input = "abcdef";

                string result = input.In(string.Empty);

                result.Should().Be(string.Empty);
            }

            [Test]
            public void When_asked_whats_in_with_non_existant_chars()
            {
                string input = "abcdef";

                string result = input.In("xyz");

                result.Should().Be("");
            }

            [Test]
            public void When_asked_whats_in_with_duplicate_chars()
            {
                string input = "abcabc";

                string result = input.In("ac");

                result.Should().Be("acac");
            }
        }

        public class Repeat : StringExtensionsTestBase
        {
            [Test]
            public void When_told_to_repeat()
            {
                string input = "abc";

                string result = input.Repeat(3);

                result.Should().Be("abcabcabc");
            }

            [Test]
            public void When_told_to_repeat_with_a_zero_count()
            {
                string input = "abc";

                string result = input.Repeat(0);

                result.Should().Be(string.Empty);
            }

            [Test]
            public void When_told_to_repeat_with_a_negative_count()
            {
                string input = "abc";

                string result = input.Repeat(-2);

                result.Should().Be(string.Empty);
            }

            [Test]
            public void When_told_to_repeat_with_an_empty_string()
            {
                string input = string.Empty;

                string result = input.Repeat(5);

                result.Should().Be(string.Empty);
            }
        }

        public class ToAscii : StringExtensionsTestBase
        {
            [Test]
            public void When_told_to_cast_as_ascii()
            {
                string input = "abc";

                List<int> result = input.ToAscii();

                result[0].Should().Be(97);
                result[1].Should().Be(98);
                result[2].Should().Be(99);
            }

            [Test]
            public void When_told_to_cast_as_ascii_with_an_empty_string()
            {
                string input = string.Empty;

                List<int> result = input.ToAscii();

                result.Should().BeEmpty();
            }

            [Test]
            public void When_told_to_cast_as_string_with_digits()
            {
                string input = "123";

                List<int> result = input.ToAscii();

                result[0].Should().Be(49);
                result[1].Should().Be(50);
                result[2].Should().Be(51);
            }
        }

        public class Tokens : StringExtensionsTestBase
        {
            [Test]
            public void When_told_to_return_the_tokens_in_a_string()
            {
                string input = "Hello, world! This is a test.";

                string[] result = input.Tokens();

                result.Should().BeEquivalentTo("Hello", "world", "This", "is", "a", "test");
            }

            [Test]
            public void When_told_to_return_the_tokens_of_an_empty_string()
            {
                string input = string.Empty;

                string[] result = input.Tokens();

                result.Should().BeEmpty();
            }

            [Test]
            public void When_told_to_return_the_tokens_of_a_string_that_contains_digits_only()
            {
                string input = "123 456 789";

                string[] result = input.Tokens();

                result.Should().BeEquivalentTo("123", "456", "789");
            }

            [Test]
            public void When_told_to_return_the_tokens_of_a_string_that_contains_special_characters()
            {
                string input = "!@#$Hello, World%^&*";

                string[] result = input.Tokens();

                result.Should().BeEquivalentTo("Hello", "World");
            }
        }

        public class ToLong : StringExtensionsTestBase
        {
            [Test]
            public void When_told_to_return_a_string_as_long()
            {
                string input = "12345";

                long result = input.ToLong();

                result.Should().Be(12345);
            }

            [Test]
            public void When_told_to_return_an_empty_string_as_long()
            {
                string input = string.Empty;

                long result = input.ToLong();

                result.Should().Be(0);
            }

            [Test]
            public void When_told_to_return_a_negative_string_as_long()
            {
                string input = "-123";

                long result = input.ToLong();

                result.Should().Be(-123);
            }
        }

        public class ToULong : StringExtensionsTestBase
        {
            [Test]
            public void When_told_to_return_a_string_as_ulong()
            {
                string input = "12345";

                ulong result = input.ToULong();

                result.Should().Be(12345);
            }

            [Test]
            public void When_told_to_return_an_empty_string_as_ulong()
            {
                string input = string.Empty;

                ulong result = input.ToULong();

                result.Should().Be(0);
            }
        }

        public class ToInt : StringExtensionsTestBase
        {
            [Test]
            public void When_told_to_return_a_string_as_int()
            {
                string input = "12345";

                int result = input.ToInt();

                result.Should().Be(12345);
            }

            [Test]
            public void When_told_to_return_an_empty_string_as_int()
            {
                string input = string.Empty;

                int result = input.ToInt();

                result.Should().Be(0);
            }

            [Test]
            public void When_told_to_return_a_negative_string_as_int()
            {
                string input = "-123";

                int result = input.ToInt();

                result.Should().Be(-123);
            }
        }

        public class ToIntFromBase : StringExtensionsTestBase
        {
            [Test]
            public void When_told_to_return_as_int_from_a_given_base()
            {
                string input = "12345";

                int result = input.ToInt(10);

                result.Should().Be(12345);
            }

            [Test]
            public void When_told_to_return_as_int_from_binary()
            {
                string input = "1010";

                int result = input.ToInt(2);

                result.Should().Be(10);
            }

            [Test]
            public void When_told_to_return_an_empty_string_as_int_from_a_given_base()
            {
                string input = string.Empty;

                int result = input.ToInt(10);

                result.Should().Be(0);
            }
        }

        public class ToUInt : StringExtensionsTestBase
        {
            [Test]
            public void When_told_to_return_a_string_as_uint()
            {
                string input = "12345";

                uint result = input.ToUInt();

                result.Should().Be(12345);
            }

            [Test]
            public void When_told_to_return_an_empty_string_as_uint()
            {
                string input = string.Empty;

                uint result = input.ToUInt();

                result.Should().Be(0);
            }
        }

        public class ToFloat : StringExtensionsTestBase
        {
            [Test]
            public void When_told_to_return_a_string_as_float()
            {
                string input = "123.45";

                float result = input.ToFloat();

                result.Should().Be(123.45f);
            }

            [Test]
            public void When_told_to_return_an_empty_string_as_float()
            {
                string input = string.Empty;

                float result = input.ToFloat();

                result.Should().Be(0);
            }

            [Test]
            public void When_told_to_return_a_string_as_float_with_a_negative_number()
            {
                string input = "-123.45";

                float result = input.ToFloat();

                result.Should().Be(-123.45f);
            }
        }

        public class ToDouble : StringExtensionsTestBase
        {
            [Test]
            public void When_told_to_return_a_string_as_double()
            {
                string input = "123.45";

                double result = input.ToDouble();

                result.Should().Be(123.45);
            }

            [Test]
            public void When_told_to_return_an_empty_string_as_double()
            {
                string input = string.Empty;

                double result = input.ToDouble();

                result.Should().Be(0);
            }

            [Test]
            public void When_told_to_return_a_string_as_double_with_a_negative_number()
            {
                string input = "-123.45";

                double result = input.ToDouble();

                result.Should().Be(-123.45);
            }
        }

        public class ToDecimal : StringExtensionsTestBase
        {
            [Test]
            public void When_told_to_return_a_string_as_decimal()
            {
                string input = "123.45";

                decimal result = input.ToDecimal();

                result.Should().Be(123.45M);
            }

            [Test]
            public void When_told_to_return_an_empty_string_as_decimal()
            {
                string input = string.Empty;

                decimal result = input.ToDecimal();

                result.Should().Be(0);
            }

            [Test]
            public void When_told_to_return_a_string_as_decimal_with_a_negative_number()
            {
                string input = "-123.45";

                decimal result = input.ToDecimal();

                result.Should().Be(-123.45M);
            }
        }

        public class ToByte : StringExtensionsTestBase
        {
            [Test]
            public void When_told_to_return_as_byte()
            {
                string input = "42";

                byte result = input.ToByte();

                result.Should().Be(42);
            }

            [Test]
            public void When_told_to_return_an_empty_string_as_byte()
            {
                string input = string.Empty;

                byte result = input.ToByte();

                result.Should().Be(0);
            }
        }

        public class ToDateTime : StringExtensionsTestBase
        {
            [Test]
            public void When_told_to_return_a_string_as_datetime()
            {
                string input = "2023-08-30";

                DateTime result = input.ToDateTime();

                result.Should().Be(new DateTime(2023, 08, 30));
            }

            [Test]
            public void When_told_to_return_an_empty_string_as_datetime()
            {
                string input = string.Empty;

                DateTime result = input.ToDateTime();

                result.Should().Be(default(DateTime));
            }
        }

        public class Replace : StringExtensionsTestBase
        {
            [Test]
            public void When_told_to_replace_a_search_term_from_a_string()
            {
                string input = "Hello, world! This is a test.";

                string result = input.Replace("world!");

                result.Should().Be("Hello,  This is a test.");
            }

            [Test]
            public void When_told_to_replace_an_empty_string_from_a_string()
            {
                string input = "Hello, world! This is a test.";

                string result = input.Replace(string.Empty);

                result.Should().Be("Hello, world! This is a test.");
            }

            [Test]
            public void When_told_to_replace_a_non_existant_string_from_a_string()
            {
                string input = "Hello, world! This is a test.";

                string result = input.Replace("foo");

                result.Should().Be("Hello, world! This is a test.");
            }
        }

        public class Numbers : StringExtensionsTestBase
        {
            [Test]
            public void When_told_to_return_only_digits_from_a_string()
            {
                string input = "abc123def456gh78i";

                string result = input.Numbers();

                result.Should().Be("12345678");
            }

            [Test]
            public void When_told_to_return_only_digits_from_an_empty_string()
            {
                string input = string.Empty;

                string result = input.Numbers();

                result.Should().Be(string.Empty);
            }

            [Test]
            public void When_told_to_return_only_digits_from_a_string_containing_non_digits()
            {
                string input = "abcdef";

                string result = input.Numbers();

                result.Should().Be(string.Empty);
            }

            [Test]
            public void When_told_to_return_only_digits_from_a_string_containing_only_digits()
            {
                string input = "0123456789";

                string result = input.Numbers();

                result.Should().Be("0123456789");
            }
        }

        public class ToStack : StringExtensionsTestBase
        {
            [Test]
            public void When_told_to_return_a_string_as_stack()
            {
                string input = "abcdef";

                Stack<char> result = input.ToStack();

                result.Should().BeEquivalentTo(new Stack<char>("fedcba"));
            }

            [Test]
            public void When_told_to_return_an_empty_string_as_stack()
            {
                string input = string.Empty;

                Stack<char> result = input.ToStack();

                result.Should().BeEmpty();
            }

            [Test]
            public void When_told_to_return_a_single_char_as_stack()
            {
                string input = "x";

                Stack<char> result = input.ToStack();

                result.Should().BeEquivalentTo(new Stack<char>("x"));
            }
        }

        public class ContainsSpecialCharacters : StringExtensionsTestBase
        {
            [Test]
            public void When_asked_if_a_string_contains_special_characters()
            {
                string input = "Hello, world!";

                bool result = input.ContainsSpecialCharacters();

                result.Should().BeTrue();
            }

            [Test]
            public void When_asked_if_an_empty_string_contains_special_characters()
            {
                string input = string.Empty;

                bool result = input.ContainsSpecialCharacters();

                result.Should().BeFalse();
            }

            [Test]
            public void When_asked_if_a_string_contains_special_characters_that_contains_none()
            {
                string input = "abcdef12345";

                bool result = input.ContainsSpecialCharacters();

                result.Should().BeFalse();
            }

            [Test]
            public void When_asked_if_a_string_that_contains_a_mix_characters()
            {
                string input = "abc123!@#";

                bool result = input.ContainsSpecialCharacters();

                result.Should().BeTrue();
            }
        }
    }
}
