namespace AdventOfCode.Core.Exceptions
{
    public class AnimationTimeoutException : Exception
    {
        public AnimationTimeoutException(TimeSpan timeout)
            : base($"Animation exceeded timeout of {timeout:hh\\:mm\\:ss}")
        {
        }
    }
}
