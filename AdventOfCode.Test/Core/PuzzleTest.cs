namespace AdventOfCode.Test.Core
{
    using System.Reflection;
    using AdventOfCode.Core;
    using AdventOfCode.Core.Extensions;
    using AdventOfCode.Puzzles._2015.Days;
    using FluentAssertions;
    using NUnit.Framework;

    public class PuzzleTestBase : TheSubject<Day4>
    {
        public static string Input { get; } = "ckczppom";

        [SetUp]
        public void Init()
        {
            CreateSplitInput(Input);
            Subject = new Day4(InputPath(2015, 4, "The Ideal Stocking Stuffer"));
        }

        protected static string InputPath(int year, int day, string dayTitle)
            => $"{Assembly.GetCallingAssembly().ExecutingDirectory()}\\{year}\\Day 0{day} - {dayTitle}\\input.txt";

        protected static void CreateSplitInput(string input)
        {
            string yearPath = $"{Assembly.GetCallingAssembly().ExecutingDirectory()}\\2015";
            string dayPath = $"Day 04 - The Ideal Stocking Stuffer";

            if (!Directory.Exists(yearPath))
            {
                Directory.CreateDirectory(yearPath);
            }

            if (!Directory.Exists($"{yearPath}\\{dayPath}"))
            {
                Directory.CreateDirectory($"{yearPath}\\{dayPath}");
            }

            File.WriteAllText($"{yearPath}\\{dayPath}\\input.txt", input);
        }

        protected static void CreateNonSplitInput(string input)
        {
            string yearPath = $"{Assembly.GetCallingAssembly().ExecutingDirectory()}\\2015";
            string dayPath = $"Day 08 - Matchsticks";

            if (!Directory.Exists(yearPath))
            {
                Directory.CreateDirectory(yearPath);
            }

            if (!Directory.Exists($"{yearPath}\\{dayPath}"))
            {
                Directory.CreateDirectory($"{yearPath}\\{dayPath}");
            }

            File.WriteAllText($"{yearPath}\\{dayPath}\\input.txt", input);
        }
    }

    [TestFixture]
    public class PuzzleTest : PuzzleTestBase
    {
        public class Constructor : PuzzleTestBase
        {
            [Test]
            public void When_created()
            {
                Subject.Should().NotBeNull();
                Subject.Input.Should().Equal(new string[] { Input });
                Subject.FilePath.Should().Be(InputPath(2015, 4, "The Ideal Stocking Stuffer"));
                Subject.DayTitle.Should().Be("The Ideal Stocking Stuffer");
            }
        }

        public class GetPuzzle : PuzzleTestBase
        {
            [Test]
            public void When_told_to_get_a_puzzle_with_an_invalid_year()
            {
                Action action = () => Puzzle.GetPuzzle(2014, 1);

                action.Should().Throw<ArgumentException>();
            }

            [Test]
            public void When_told_to_get_a_puzzle_with_an_invalid_day()
            {
                Action action = () => Puzzle.GetPuzzle(2015, 26);

                action.Should().Throw<ArgumentException>();
            }


            [Test]
            public void When_told_to_get_a_puzzle_with_split_and_trim()
            {
                IPuzzle? puzzle = Puzzle.GetPuzzle(2015, 4, $"value1\r\nvalue2", true, true);

                puzzle.Should().NotBeNull();
                puzzle?.Input.Length.Should().Be(2);
                puzzle?.Input.Should().Equal(new string[] { "value1", "value2" });
                puzzle?.FilePath.Should().Be(string.Empty);
                puzzle?.DayTitle.Should().Be(string.Empty);
            }

            [Test]
            public void When_told_to_get_a_puzzle_with_split_and_no_trim()
            {
                IPuzzle? puzzle = Puzzle.GetPuzzle(2015, 4, $"{Input}   ", true, false);

                puzzle.Should().NotBeNull();
                puzzle?.Input.Length.Should().Be(1);
                puzzle?.Input.Should().Equal(new string[] { $"{Input}   " });
                puzzle?.FilePath.Should().Be(string.Empty);
                puzzle?.DayTitle.Should().Be(string.Empty);
            }

            [Test]
            public void When_told_to_get_a_puzzle_with_no_split_and_no_trim()
            {
                IPuzzle? puzzle = Puzzle.GetPuzzle(2015, 4, $"{Input}\r\n   ", false, false);

                puzzle.Should().NotBeNull();
                puzzle?.Input.Length.Should().Be(1);
                puzzle?.Input.Should().Equal(new string[] { $"{Input}\r\n   " });
                puzzle?.FilePath.Should().Be(string.Empty);
                puzzle?.DayTitle.Should().Be(string.Empty);
            }

            [Test]
            public void When_told_to_get_a_puzzle_with_no_split_and_trim()
            {
                IPuzzle? puzzle = Puzzle.GetPuzzle(2015, 4, $"value1\r\nvalue2   ", false, true);

                puzzle.Should().NotBeNull();
                puzzle?.Input.Length.Should().Be(1);
                puzzle?.Input.Should().Equal(new string[] { $"value1\r\nvalue2" });
                puzzle?.FilePath.Should().Be(string.Empty);
                puzzle?.DayTitle.Should().Be(string.Empty);
            }

            [Test]
            public void When_told_to_get_a_puzzle_with_a_year_and_day()
            {
                IPuzzle? puzzle = Puzzle.GetPuzzle(2015, 4);

                puzzle.Should().NotBeNull();
                puzzle?.Input.Length.Should().Be(1);
                Subject.Input.Should().Equal(new string[] { Input });
                Subject.FilePath.Should().Be(InputPath(2015, 4, "The Ideal Stocking Stuffer"));
                puzzle?.DayTitle.Should().Be("The Ideal Stocking Stuffer");
            }
        }

        public class GetInput : PuzzleTestBase
        {
            [Test]
            public void When_told_to_get_a_puzzle_input()
            {
                string[] result = Puzzle.GetInput(2015, 4);

                result.Should().Equal(new string[] { Input });
            }

            [Test]
            public void When_told_to_get_a_puzzle_input_with_remove_none()
            {
                Puzzle.GetInput(2015, 4, StringSplitOptions.None);
            }

            [Test]
            public void When_told_to_get_a_puzzle_input_with_no_split()
            {
                string input = "qxfcsmh\r\nqxfcsmh\r\nqxfcsmh";
                CreateNonSplitInput(input);
                IPuzzle? puzzle = Puzzle.GetPuzzle(2015, 8);

                puzzle.Should().NotBeNull();
                puzzle?.Input.Should().Equal(new string[] { input });
                puzzle?.FilePath.Should().Be(InputPath(2015, 8, "Matchsticks"));
                puzzle?.DayTitle.Should().Be("Matchsticks");
            }
        }
    }
}
