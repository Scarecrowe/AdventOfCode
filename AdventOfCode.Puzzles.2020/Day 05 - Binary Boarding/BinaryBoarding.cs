namespace AdventOfCode.Puzzles._2020.Day_05___Binary_Boarding
{
    public class BinaryBoarding
    {
        public BinaryBoarding(string[] input)
        {
            this.Input = input;
            this.SeatIds = new();
        }

        private string[] Input { get; }

        private List<int> SeatIds { get; set; }

        public BinaryBoarding Process()
        {
            this.SeatIds = new();

            foreach (string boardingPass in this.Input)
            {
                this.SeatIds.Add(SeatId(
                   Partition(boardingPass[..7], 64, 127, 'F'),
                   Partition(boardingPass[7..], 4, 7, 'L')));
            }

            return this;
        }

        public int MaxSeatId() => this.SeatIds.Max();

        public int MissingSeatId()
        {
            this.SeatIds.Sort();

            int last = this.SeatIds[0];

            for (int i = 1; i < this.SeatIds.Count; i++)
            {
                if (this.SeatIds[i] != last + 1)
                {
                    return last + 1;
                }

                last = this.SeatIds[i];
            }

            return 0;
        }

        private static int Partition(string input, int bits, int high, char instruction)
        {
            int low = 0;

            for (int i = 0; i < input.Length - 1; i++)
            {
                if (input[i] == instruction)
                {
                    high -= bits;
                }
                else
                {
                    low += bits;
                }

                bits /= 2;
            }

            return input[^1] == instruction ? low : high;
        }

        private static int SeatId(int row, int column) => (row * 8) + column;
    }
}
