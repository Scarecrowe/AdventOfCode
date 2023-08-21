namespace AdventOfCode.Test.Extensions
{
    using System;
    using System.Reflection;

    public static class ExceptionExtensions
    {
        public static void Rethrow(this Exception ex)
        {
            MethodInfo? methodInfo = typeof(Exception)
                .GetMethod(
                "PrepForRemoting",
                BindingFlags.NonPublic | BindingFlags.Instance);

            if (methodInfo == null)
            {
                throw new InvalidOperationException();
            }

            methodInfo?.Invoke(ex, Array.Empty<object>());

            throw ex;
        }
    }
}
