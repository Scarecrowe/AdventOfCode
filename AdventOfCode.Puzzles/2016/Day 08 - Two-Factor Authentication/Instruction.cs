namespace AdventOfCode.Puzzles._2016.Day_08___Two_Factor_Authentication
{
    using AdventOfCode.Core;
    using AdventOfCode.Core.Extensions;

    public class Instruction
    {
        public Instruction(string input)
        {
            this.Point = new(0, 0);
            string[] tokens = input.SplitSpace();
            string[] coordinates;

            if (tokens[0] == "rect")
            {
                coordinates = tokens[1].Split("x");
                this.Point = new(tokens[1].Split("x").ToInt());
                this.Type = InstructionType.Rect;

                return;
            }

            coordinates = tokens[2].Split("=");

            this.Count = tokens[4].ToInt();

            if (tokens[1] == "row")
            {
                this.Type = InstructionType.RotateRow;
                this.Point.Y = coordinates[1].ToInt();
                return;
            }

            this.Type = InstructionType.RotateColumn;
            this.Point.X = coordinates[1].ToInt();
        }

        public InstructionType Type { get; }

        public Vector<int> Point { get; }

        public int Count { get; }
    }
}
