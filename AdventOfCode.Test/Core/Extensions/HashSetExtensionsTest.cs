namespace AdventOfCode.Test.Core
{
    using AdventOfCode.Core.Extensions;
    using FluentAssertions;
    using NUnit.Framework;

    public class HashSetExtensionsTestBase
    {
        [SetUp]
        public void Init()
        {
        }
    }

    [TestFixture]
    public class HashSetExtensionsTest : HashSetExtensionsTestBase
    {
        public class AddRange : HashSetExtensionsTestBase
        {
            [Test]
            public void AddRange_ShouldAddAllValuesToHashSet()
            {
                HashSet<int> hashSet = new() { 1, 2, 3 };
                HashSet<int> valuesToAdd = new() { 3, 4, 5 };

                hashSet.AddRange(valuesToAdd);

                hashSet.ElementAt(0).Should().Be(1);
                hashSet.ElementAt(1).Should().Be(2);
                hashSet.ElementAt(2).Should().Be(3);
                hashSet.ElementAt(3).Should().Be(4);
                hashSet.ElementAt(4).Should().Be(5);
            }

            [Test]
            public void AddRange_WhenEmptyValuesToAdd_ShouldNotChangeHashSet()
            {
                HashSet<int> hashSet = new HashSet<int> { 1, 2, 3 };
                HashSet<int> valuesToAdd = new HashSet<int>();

                hashSet.AddRange(valuesToAdd);

                hashSet.ElementAt(0).Should().Be(1);
                hashSet.ElementAt(1).Should().Be(2);
                hashSet.ElementAt(2).Should().Be(3);
            }
        }
    }
}
