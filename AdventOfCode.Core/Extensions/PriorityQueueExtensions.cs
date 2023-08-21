namespace AdventOfCode.Core.Extensions
{
    public static class PriorityQueueExtensions
    {
        public static bool Any<TElement, TPriority>(this PriorityQueue<TElement, TPriority> queue) => queue.Count > 0;
    }
}
