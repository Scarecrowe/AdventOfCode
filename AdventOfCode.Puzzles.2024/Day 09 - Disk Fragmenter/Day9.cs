namespace AdventOfCode.Puzzles._2024.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2024.Day_09___Disk_Fragmenter;

    public class Day9 : Puzzle, IPuzzle
    {
        public Day9()
        {
            this.DayTitle = "Disk Fragmenter";
            this.GetPuzzleData(9, this.DayTitle);
        }

        public string Silver() => $"{new DiskFragmenter(this.Input).FilesystemChecksum()}";

        public string Gold() => $"{new DiskFragmenter(this.Input).WholeFilesystemChecksum()}";
    }
}
