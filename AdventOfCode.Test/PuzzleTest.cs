namespace AdventOfCode.Test
{
    using AdventOfCode.Core;
    using FluentAssertions;
    using NUnit.Framework;

    public abstract class PuzzleTest
    {
        public int Year { get; private set; }

        public int Day { get; private set; }

        public string? SilverResult { get; private set; }

        public string? GoldResult { get; private set; }

        public void Setup(int year, int day, string silver, string gold)
        {
            this.Year = year;
            this.Day = day;
            this.SilverResult = silver;
            this.GoldResult = gold;
        }

        public void Silver(string input, string expected, bool split = true, bool trim = true, StringSplitOptions options = StringSplitOptions.RemoveEmptyEntries)
            => Puzzle.GetPuzzle(this.Year, this.Day, input, split, trim, options)?.Silver().Should().Be(expected);

        public void Gold(string input, string expected, bool split = true, bool trim = true, StringSplitOptions options = StringSplitOptions.RemoveEmptyEntries)
            => Puzzle.GetPuzzle(this.Year, this.Day, input, split, trim, options)?.Gold().Should().Be(expected);

        public bool IsSlow(IPuzzle? puzzle, string method) => puzzle?.GetType().GetMethods().Single(x => x.Name == method).CustomAttributes.Count() > 0;

        [Test]
        public void SilverAnswer()
        {
            IPuzzle? puzzle = Puzzle.GetPuzzle(this.Year, this.Day);

#if DEBUG

            if (!this.IsSlow(puzzle, "Silver"))
            {
                puzzle?.Silver().Should().Be(this.SilverResult);
            }
#else
            puzzle?.Silver().Should().Be(this.SilverResult);
#endif
        }

        [Test]
        public void GoldAnswer()
        {
            IPuzzle? puzzle = Puzzle.GetPuzzle(this.Year, this.Day);

#if DEBUG

            if (!this.IsSlow(puzzle, "Gold"))
            {
                puzzle?.Gold().Should().Be(this.GoldResult);
            }
#else
            puzzle?.Gold().Should().Be(this.GoldResult);
#endif
        }
    }
}
