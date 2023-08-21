namespace AdventOfCode.Puzzles._2017.Day_25___The_Halting_Problem
{
    public class State
    {
        public State(char letter, int[] values, Direction[] directions, char[] states)
        {
            this.Letter = letter;
            this.Values = values;
            this.Directions = directions;
            this.States = states;
        }

        public char Letter { get; }

        public int[] Values { get; }

        public Direction[] Directions { get; }

        public char[] States { get; }
    }
}
