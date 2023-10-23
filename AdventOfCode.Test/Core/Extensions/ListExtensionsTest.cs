namespace AdventOfCode.Test.Core
{
    using AdventOfCode.Core;
    using AdventOfCode.Core.Extensions;
    using FluentAssertions;
    using NUnit.Framework;

    public class ListExtensionsTestBase
    {
        [SetUp]
        public void Init()
        {
        }
    }

    [TestFixture]
    public class ListExtensionsTest : ListExtensionsTestBase
    {
        public class FillInt : ListExtensionsTestBase
        {
            [Test]
            public void When_told_to_fill_a_range()
            {
                var list = new List<int>();
                list.Fill(1, 5);
                list.Should().BeEquivalentTo(new int[] { 1, 2, 3, 4, 5 });
            }

            [Test]
            public void When_told_to_fill_a_range_with_matching_start_and_end()
            {
                var list = new List<int>();
                list.Fill(10, 10);
                list.Should().BeEquivalentTo(new int[] { 10 });
            }

            [Test]
            public void When_told_to_fill_a_negative_range()
            {
                var list = new List<int>();
                list.Fill(-3, -1);
                list.Should().BeEquivalentTo(new int[] { -3, -2, -1 });
            }
        }

        public class FillLong : ListExtensionsTestBase
        {
            [Test]
            public void When_told_to_fill_a_range()
            {
                var list = new List<long>();
                list.Fill(1, 5);
                list.Should().BeEquivalentTo(new long[] { 1, 2, 3, 4, 5 });
            }

            [Test]
            public void When_told_to_fill_a_range_with_matching_start_and_end()
            {
                var list = new List<long>();
                list.Fill(10, 10);
                list.Should().BeEquivalentTo(new long[] { 10 });
            }

            [Test]
            public void When_told_to_fill_a_negative_range()
            {
                var list = new List<long>();
                list.Fill(-3, -1);
                list.Should().BeEquivalentTo(new long[] { -3, -2, -1 });
            }
        }

        public class FillWith : ListExtensionsTestBase
        {
            [Test]
            public void When_told_to_fill_a_list_with_a_given_value_and_length()
            {
                var list = new List<int>();
                list.FillWith(5, 3);
                list.Should().BeEquivalentTo(new int[] { 5, 5, 5, 5 });
            }

            [Test]
            public void When_told_to_fill_a_list_with_a_given_value_and_zero_length()
            {
                var list = new List<int>();
                list.FillWith(2, 0);
                list.Should().BeEmpty();
            }

            [Test]
            public void When_told_to_fill_a_list_with_a_given_negative_value_and_length()
            {
                var list = new List<int>();
                list.FillWith(-1, 2);
                list.Should().BeEquivalentTo(new int[] { -1, -1, -1 });
            }
        }

        public class Permutations : ListExtensionsTestBase
        {
            [Test]
            public void When_told_to_generate_a_set_of_permutations()
            {
                var list = new List<int> { 1, 2, 3 };
                var permutations = list.Permutations(3);

                permutations.Should().HaveCount(6);

                foreach (var permutation in permutations)
                {
                    permutation.Should().HaveCount(3);
                    permutation.Should().OnlyHaveUniqueItems();
                    list.Should().Contain(permutation);
                }
            }

            [Test]
            public void When_told_to_generate_a_set_of_permutations_with_an_invalid_length()
            {
                var list = new List<string> { "A", "B", "C" };
                Action action = () => list.Permutations(0);
                action.Should().Throw<ArgumentException>();
            }

            [Test]
            public void When_told_to_generate_a_set_of_permutations_with_an_empty_list()
            {
                var list = new List<char>();
                var permutations = list.Permutations(2);
                permutations.Should().BeEmpty();
            }
        }

        public class CombinationsWithRepetition : ListExtensionsTestBase
        {
            [Test]
            public void When_told_to_generate_a_set_of_combinations_with_repetition()
            {
                var list = new List<int> { 1, 2, 3 };
                var combinations = list.CombinationsWithRepetition(3).ToList();

                combinations.Should().HaveCount(27);

                foreach (var combination in combinations)
                {
                    combination.Should().HaveLength(3);
                    foreach (var c in combination)
                    {
                        char.IsDigit(c).Should().BeTrue();
                    }

                    foreach (var item in list)
                    {
                        combination.Count(c => c == char.Parse(item.ToString())).Should().BeLessOrEqualTo(3);
                    }
                }
            }

            [Test]
            public void When_told_to_generate_a_set_of_combinations_with_repetition_with_an_invalid_length()
            {
                var list = new List<int> { 1, 2, 3 };
                var combinations = list.CombinationsWithRepetition(0);
                combinations.Should().BeEquivalentTo(string.Empty);
            }

            [Test]
            public void When_told_to_generate_a_set_of_combinations_with_repetition_with_an_empty_list()
            {
                var list = new List<int>();
                var combinations = list.CombinationsWithRepetition(2);
                combinations.Should().BeEmpty();
            }
        }

        public class CombinationsOfTotal : ListExtensionsTestBase
        {
            [Test]
            public void When_told_to_generate_combinations_of_total()
            {
                var list = new List<long> { 1, 2, 3, 4 };
                var total = 7;
                var combinations = list.CombinationsOfTotal(total).ToList();

                combinations.Should().HaveCount(2);

                foreach (var combination in combinations)
                {
                    combination.Sum().Should().Be(total);
                    combination.All(item => list.Contains(item)).Should().BeTrue();
                }
            }

            [Test]
            public void When_told_to_generate_combinations_of_total_with_no_valid_combination()
            {
                var list = new List<long> { 1, 2, 3 };
                var total = 10;
                var combinations = list.CombinationsOfTotal(total);
                combinations.Should().BeEmpty();
            }

            [Test]
            public void When_told_to_generate_combinations_of_total_with_an_empty_collection()
            {
                var list = new List<long>();
                var total = 5;
                var combinations = list.CombinationsOfTotal(total);
                combinations.Should().BeEmpty();
            }
        }

        public class Combinations : ListExtensionsTestBase
        {
            [Test]
            public void When_told_to_generate_combinations_for_a_given_length()
            {
                var list = new List<int> { 1, 2, 3, 4 };
                var length = 2;
                var combinations = list.Combinations(length);
                var count = 0;

                foreach(var item in combinations)
                {
                    count++;
                }

                count.Should().Be(6);
            }

            [Test]
            public void When_told_to_generate_combinations_for_a_given_length_for_a_zero_length()
            {
                var list = new List<string> { "A", "B", "C" };
                var length = 4;
                var combinations = list.Combinations(length);
                var count = 0;
                foreach(var item in combinations)
                {
                    count++;
                }

                count.Should().Be(0);
            }

            [Test]
            public void When_told_to_generate_combinations_for_a_given_length_with_an_empty_collection()
            {
                var list = new List<int>();
                var length = 2;
                var combinations = list.Combinations(length);
                var count = 0;
                foreach (var item in combinations)
                {
                    count++;
                }

                count.Should().Be(0);
            }

            [Test]
            public void When_told_to_generate_a_set_of_combinations_with_a_given_collection()
            {
                var list = new List<int> { 1, 2, 3 };
                var combinations = list.Combinations().ToList();

                combinations.Count.Should().Be(8);
            }

            [Test]
            public void When_told_to_generate_a_set_of_combinations_with_an_empty_collection()
            {
                var list = new List<string>();
                var combinations = list.Combinations();
                combinations.Count.Should().Be(0);
            }
        }

        public class ProductInt : ListExtensionsTestBase
        {
            [Test]
            public void When_told_to_get_the_product_of_a_list_of_int()
            {
                var list = new List<int> { 2, 3, 4 };
                var product = list.Product();
                product.Should().Be(24);
            }

            [Test]
            public void When_told_to_get_the_product_of_a_list_of_int_when_an_empty_collection()
            {
                var list = new List<int>();
                var product = list.Product();
                product.Should().Be(0);
            }

            [Test]
            public void When_told_to_get_the_product_of_a_list_of_int_with_a_single_value()
            {
                var list = new List<int> { 5 };
                var product = list.Product();
                product.Should().Be(5);
            }
        }

        public class ProductLong : ListExtensionsTestBase
        {
            [Test]
            public void When_told_to_get_the_product_of_a_list_of_int()
            {
                var list = new List<long> { 2, 3, 4 };
                var product = list.Product();
                product.Should().Be(24);
            }

            [Test]
            public void When_told_to_get_the_product_of_a_list_of_int_when_an_empty_collection()
            {
                var list = new List<long>();
                var product = list.Product();
                product.Should().Be(0);
            }

            [Test]
            public void When_told_to_get_the_product_of_a_list_of_int_with_a_single_value()
            {
                var list = new List<long> { 5 };
                var product = list.Product();
                product.Should().Be(5);
            }
        }

        public class RemoveRange : ListExtensionsTestBase
        {
            [Test]
            public void When_told_to_remove_a_range_of_items_from_a_list()
            {
                var list = new List<int> { 1, 2, 3, 4, 5 };
                var rangeToRemove = new List<int> { 2, 4 };

                list.RemoveRange(rangeToRemove);

                list.Should().BeEquivalentTo(new List<int> { 1, 3, 5 });
            }

            [Test]
            public void When_told_to_remove_a_range_of_items_from_a_list_with_an_empty_range()
            {
                var list = new List<string> { "apple", "banana", "cherry" };
                var emptyRange = new List<string>();

                list.RemoveRange(emptyRange);

                list.Should().BeEquivalentTo(new List<string> { "apple", "banana", "cherry" });
            }

            [Test]
            public void When_told_to_remove_a_range_of_items_from_a_list_with_all_items_in_the_range()
            {
                var list = new List<char> { 'a', 'b', 'c' };
                var rangeToRemove = new List<char> { 'a', 'b', 'c' };

                list.RemoveRange(rangeToRemove);

                list.Should().BeEmpty();
            }
        }
    }
}
