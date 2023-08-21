namespace AdventOfCode.Test.Core
{
    using AdventOfCode.Core;
    using AdventOfCode.Core.Extensions;
    using FluentAssertions;
    using NUnit.Framework;
    using System.Reflection;

    public class PuzzleConsoleTestBase
    {
        [SetUp]
        public void Init()
        {
            PuzzleConsole.Clear();
        }
    }

    [TestFixture]
    public class PuzzleConsoleTest : PuzzleConsoleTestBase
    {
        public class WriteString : PuzzleConsoleTestBase
        {
            [Test]
            public void When_told_to_write_a_string()
            {
                PuzzleConsole.Write("Writing a string...");

                PuzzleConsole.Buffer.Count.Should().Be(1);

                PuzzleConsole.Buffer[0].Should().NotContain(Environment.NewLine);
                PuzzleConsole.Buffer[0].Should().Be(" Writing a string...");

                PuzzleConsole.Write("Writing another string...");

                PuzzleConsole.Buffer.Count.Should().Be(2);

                PuzzleConsole.Buffer[1].Should().NotContain(Environment.NewLine);
                PuzzleConsole.Buffer[1].Should().Be("Writing another string...");
            }
        }

        public class WriteChar : PuzzleConsoleTestBase
        {
            [Test]
            public void When_told_to_write_a_string()
            {
                PuzzleConsole.Write('A');

                PuzzleConsole.Buffer.Count.Should().Be(1);

                PuzzleConsole.Buffer[0].Should().NotContain(Environment.NewLine);
                PuzzleConsole.Buffer[0].Should().Be(" A");

                PuzzleConsole.Write('B');

                PuzzleConsole.Buffer.Count.Should().Be(2);

                PuzzleConsole.Buffer[1].Should().NotContain(Environment.NewLine);
                PuzzleConsole.Buffer[1].Should().Be("B");
            }
        }

        public class WriteLineString : PuzzleConsoleTestBase
        {
            [Test]
            public void When_told_to_write_a_string()
            {
                PuzzleConsole.WriteLine("Writing a string...");

                PuzzleConsole.Buffer.Count.Should().Be(1);

                PuzzleConsole.Buffer[0].Should().Contain(Environment.NewLine);
                PuzzleConsole.Buffer[0].Should().Be($" Writing a string...{Environment.NewLine}");

                PuzzleConsole.WriteLine("Writing another string...");

                PuzzleConsole.Buffer.Count.Should().Be(2);

                PuzzleConsole.Buffer[1].Should().Contain(Environment.NewLine);
                PuzzleConsole.Buffer[1].Should().Be($" Writing another string...{Environment.NewLine}");
            }
        }

        public class WriteLineInt : PuzzleConsoleTestBase
        {
            [Test]
            public void When_told_to_write_a_string()
            {
                PuzzleConsole.WriteLine(100);

                PuzzleConsole.Buffer.Count.Should().Be(1);

                PuzzleConsole.Buffer[0].Should().Contain(Environment.NewLine);
                PuzzleConsole.Buffer[0].Should().Be($" 100{Environment.NewLine}");

                PuzzleConsole.WriteLine(93546);

                PuzzleConsole.Buffer.Count.Should().Be(2);

                PuzzleConsole.Buffer[1].Should().Contain(Environment.NewLine);
                PuzzleConsole.Buffer[1].Should().Be($" 93546{Environment.NewLine}");
            }
        }

        public class Flush : PuzzleConsoleTestBase
        {
            [Test]
            public void When_told_to_flush_with_no_buffer()
            {
                PuzzleConsole.Flush();

                PuzzleConsole.Buffer.Count.Should().Be(0);
                PuzzleConsole.Position.Should().Be(0);
            }

            [Test]
            public void When_told_to_flush()
            {
                PuzzleConsole.WriteLine("Line 1");
                PuzzleConsole.WriteLine("Line 2");
                PuzzleConsole.WriteLine("Line 3");
                PuzzleConsole.WriteLine("Line 4");
                PuzzleConsole.Write("Line 5");

                PuzzleConsole.Buffer.Count.Should().Be(5);
                PuzzleConsole.Position.Should().Be(1);

                PuzzleConsole.Flush();

                PuzzleConsole.Buffer.Count.Should().Be(0);
                PuzzleConsole.Position.Should().Be(0);
            }
        }

        public class FlushToFile: PuzzleConsoleTestBase
        {
            [Test]
            public void When_told_to_flush_to_file_with_no_buffer()
            {
                PuzzleConsole.Flush($"{Assembly.GetCallingAssembly().ExecutingDirectory()}\\flushed.txt");
                PuzzleConsole.Buffer.Count.Should().Be(0);
                PuzzleConsole.Position.Should().Be(0);
            }

            [Test]
            public void When_told_to_flush()
            {
                PuzzleConsole.WriteLine("Line 1");
                PuzzleConsole.WriteLine("Line 2");
                PuzzleConsole.WriteLine("Line 3");
                PuzzleConsole.WriteLine("Line 4");
                PuzzleConsole.Write("Line 5");

                PuzzleConsole.Buffer.Count.Should().Be(5);
                PuzzleConsole.Position.Should().Be(1);

                PuzzleConsole.Flush($"{Assembly.GetCallingAssembly().ExecutingDirectory()}\\flushed.txt");

                PuzzleConsole.Buffer.Count.Should().Be(0);
                PuzzleConsole.Position.Should().Be(0);
            }
        }

        public class Clear : PuzzleConsoleTestBase
        {
            [Test]
            public void When_told_to_clear()
            {
                PuzzleConsole.WriteLine("Line 1");
                PuzzleConsole.WriteLine("Line 2");
                PuzzleConsole.WriteLine("Line 3");
                PuzzleConsole.WriteLine("Line 4");
                PuzzleConsole.Write("Line 5");

                PuzzleConsole.Buffer.Count.Should().Be(5);
                PuzzleConsole.Position.Should().Be(1);

                PuzzleConsole.Clear();

                PuzzleConsole.Buffer.Count.Should().Be(0);
                PuzzleConsole.Position.Should().Be(0);
            }
        }
    }
}
