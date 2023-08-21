namespace AdventOfCode.Puzzles._2015.Day_01___Not_Quite_Lisp
{
    public class NotQuiteLisp
    {
        public NotQuiteLisp(string[] input) => this.Instructions = input[0];

        private string Instructions { get; }

        public int FinalPosition() => this.Instructions.Sum(x => Floor(x));

        public int BasementPosition()
        {
            int floor = 0;

            for (int i = 0; i < this.Instructions.Length; i++)
            {
                floor += Floor(this.Instructions[i]);

                if (floor < 0)
                {
                    return ++i;
                }
            }

            throw new InvalidOperationException();
        }

        private static int Floor(char @char) => @char == '(' ? 1 : -1;
    }
}
