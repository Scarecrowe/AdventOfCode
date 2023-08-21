namespace AdventOfCode.Test.Core
{
    using AdventOfCode.Core;
    using AdventOfCode.Core.Extensions;
    using FluentAssertions;
    using NUnit.Framework;

    public class PriorityQueueExtensionsTestBase : TheSubject<PriorityQueue<int, int>>
    {
        [SetUp]
        public void Init()
        {
            Subject = new PriorityQueue<int, int>();
            Subject.Enqueue(100, 1);
        }
    }

    [TestFixture]
    public class PriorityQueueExtensionsTest : PriorityQueueExtensionsTestBase
    {
        [Test]
        public void When_asked_if_any_queue_items_exist()
        {
            Subject.Any().Should().BeTrue();
        }

        [Test]
        public void When_asked_if_any_queue_items_exist_when_empty()
        {
            Subject.Clear();
            Subject.Any().Should().BeFalse();
        }
    }
}
