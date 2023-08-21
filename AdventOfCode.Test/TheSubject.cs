namespace AdventOfCode.Test
{
    using System.Reflection;
    using AdventOfCode.Core.Extensions;
    using FluentAssertions;
    using FluentAssertions.Events;

    public abstract class TheSubject<T>
    {
        public TheSubject()
        {
        }

        private static T? subject;

        public static T Subject
        {
            get
            {
                if (subject == null)
                {
                    throw new InvalidOperationException();
                }

                return subject;
            }
                
            set
            {
                subject = value;
                Events?.Dispose();
                Type type = typeof(T);
                if (((TypeInfo)type).DeclaredEvents.Any())
                {
                    Events = Subject.Monitor();
                }
            }
        }

        public static string TestDirectory => Assembly.GetCallingAssembly().ExecutingDirectory();

        public static IMonitor<T>? Events { get; set; }
    }
}
