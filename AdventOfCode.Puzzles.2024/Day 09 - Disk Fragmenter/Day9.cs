namespace AdventOfCode.Puzzles._2024.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2024.Day_09___Disk_Fragmenter;

    public class Day9 : Puzzle, IPuzzle
    {
        public Day9()
            : base(2024, 9, "Disk Fragmenter")
        {
        }

        public Day9(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver() => $"{new DiskFragmenter(this.Input).FilesystemChecksum()}";

        public string Gold() => $"{new DiskFragmenter(this.Input).WholeFilesystemChecksum()}";
    }
}
