namespace AdventOfCode.Test.Core
{
    using AdventOfCode.Core.Extensions;
    using FluentAssertions;
    using NUnit.Framework;

    public class DictionaryExtensionsTestBase
    {
        [SetUp]
        public void Init()
        {
        }
    }

    [TestFixture]
    public class DictionaryExtensionsTest : DictionaryExtensionsTestBase
    {
        public class TryAddValue : DictionaryExtensionsTestBase
        {
            [Test]
            public void When_told_to_add_a_key_value_pair_and_the_key_does_not_exist()
            {
                var dictionary = new Dictionary<string, int>();

                dictionary.TryAddValue("key1", 42);

                dictionary.Should().ContainKey("key1");
                dictionary["key1"].Should().Be(42);
            }

            [Test]
            public void When_told_to_add_a_key_value_pair_that_already_exists()
            {
                var dictionary = new Dictionary<string, int>
                {
                    { "key1", 42 }
                };

                dictionary.TryAddValue("key1", 100);

                dictionary.Should().HaveCount(1);
                dictionary["key1"].Should().Be(100);
            }
        }

        public class AddRange : DictionaryExtensionsTestBase
        {
            [Test]
            public void AddRange_WhenAddingEmptyDictionary_ShouldNotChangeOriginalDictionary()
            {
                var originalDictionary = new Dictionary<string, int>
                {
                    { "key1", 42 }
                };

                var emptyDictionary = new Dictionary<string, int>();

                originalDictionary.AddRange(emptyDictionary);

                originalDictionary.Should().HaveCount(1);
                originalDictionary.Should().ContainKey("key1").And.ContainValue(42);
            }

            [Test]
            public void AddRange_WhenAddingNonEmptyDictionary_ShouldAddKeyValuesToOriginalDictionary()
            {
                var originalDictionary = new Dictionary<string, int>
                {
                    { "key1", 42 }
                };
                        var valuesToAdd = new Dictionary<string, int>
                {
                    { "key2", 100 },
                    { "key3", 200 }
                };

                originalDictionary.AddRange(valuesToAdd);

                originalDictionary.Should().HaveCount(3);
                originalDictionary.Should().ContainKey("key1").And.ContainValue(42);
                originalDictionary.Should().ContainKey("key2").And.ContainValue(100);
                originalDictionary.Should().ContainKey("key3").And.ContainValue(200);
            }

            [Test]
            public void AddRange_WhenAddingDictionaryWithExistingKeys_ShouldOverrideValuesInOriginalDictionary()
            {
                var originalDictionary = new Dictionary<string, int>
                {
                    { "key1", 42 }
                };

                var valuesToAdd = new Dictionary<string, int>
                {
                    { "key1", 100 },
                    { "key2", 200 }
                };

                originalDictionary.AddRange(valuesToAdd);

                originalDictionary.Should().HaveCount(2);
                originalDictionary.Should().ContainKey("key1").And.ContainValue(100);
                originalDictionary.Should().ContainKey("key2").And.ContainValue(200);
            }
        }

        public class GetKeyIndex : DictionaryExtensionsTestBase
        {
            [Test]
            public void When_told_to_get_the_index_of_a_key_that_exists()
            {
                var dictionary = new Dictionary<string, int>
                {
                    { "key1", 42 },
                    { "key2", 100 },
                    { "key3", 200 }
                };

                var index = dictionary.GetKeyIndex("key2");

                index.Should().Be(1);
            }

            [Test]
            public void When_told_to_get_the_index_of_a_key_that_does_not_exist()
            {
                var dictionary = new Dictionary<string, int>
                {
                    { "key1", 42 },
                    { "key2", 100 },
                    { "key3", 200 }
                };

                var index = dictionary.GetKeyIndex("key4");

                index.Should().Be(-1);
            }
        }

        public class GetValueIndex : DictionaryExtensionsTestBase
        {
            [Test]
            public void When_told_to_get_the_index_of_a_value_that_exists()
            {
                var dictionary = new Dictionary<string, int>
                {
                    { "key1", 42 },
                    { "key2", 100 },
                    { "key3", 200 }
                };

                var index = dictionary.GetValueIndex(100);

                index.Should().Be(1);
            }

            [Test]
            public void When_told_to_get_the_index_of_a_value_that_does_not_exist()
            {
                var dictionary = new Dictionary<string, int>
                {
                    { "key1", 42 },
                    { "key2", 100 },
                    { "key3", 200 }
                };

                var index = dictionary.GetValueIndex(150);

                index.Should().Be(-1);
            }
        }

        public class Pop : DictionaryExtensionsTestBase
        {
            [Test]
            public void When_told_to_pop_a_key_value_pair_and_the_dictionary_is_not_empty()
            {
                var dictionary = new Dictionary<string, int>
                {
                    { "key1", 42 },
                    { "key2", 100 },
                    { "key3", 200 }
                };

                var poppedPair = dictionary.Pop();

                poppedPair.Key.Should().Be("key3");
                poppedPair.Value.Should().Be(200);
                dictionary.Should().NotContainKey("key3");
            }

            [Test]
            public void When_told_to_pop_a_key_value_pair_and_the_dictionary_is_empty()
            {
                var dictionary = new Dictionary<string, int>();

                Action action = () => dictionary.Pop();

                action.Should().Throw<InvalidOperationException>();
            }
        }

        public class ForEach : DictionaryExtensionsTestBase
        {
            [Test]
            public void When_told_to_enumerate_a_key_value_pair_collection()
            {
                var dictionary = new Dictionary<string, int>
                {
                    { "key1", 42 },
                    { "key2", 100 },
                    { "key3", 200 }
                };

                var keysProcessed = new List<string>();
                var valuesProcessed = new List<int>();

                dictionary.ForEach(pair =>
                {
                    keysProcessed.Add(pair.Key);
                    valuesProcessed.Add(pair.Value);
                });

                keysProcessed.Should().Contain("key1", "key2", "key3");
                valuesProcessed.Should().Contain(42);
                valuesProcessed.Should().Contain(100);
                valuesProcessed.Should().Contain(200);
            }
        }
    }
}
