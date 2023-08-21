namespace AdventOfCode.Puzzles._2016.Day_02___Bathroom_Security
{
    using System.Text;
    using AdventOfCode.Core;

    public class BathroomSecurity
    {
        public static readonly Dictionary<KeyPadMode, char[,]> KeyPads = new()
        {
            {
                KeyPadMode.Simple,
                new char[,]
                {
                    { '0', '0', '0', '0', '0' },
                    { '0', '1', '2', '3', '0' },
                    { '0', '4', '5', '6', '0' },
                    { '0', '7', '8', '9', '0' },
                    { '0', '0', '0', '0', '0' }
                }
            },
            {
                KeyPadMode.Advanced,
                new char[,]
                {
                    { '0', '0', '0', '0', '0', '0', '0' },
                    { '0', '0', '0', '1', '0', '0', '0' },
                    { '0', '0', '2', '3', '4', '0', '0' },
                    { '0', '5', '6', '7', '8', '9', '0' },
                    { '0', '0', 'A', 'B', 'C', '0', '0' },
                    { '0', '0', '0', 'D', '0', '0', '0' },
                    { '0', '0', '0', '0', '0', '0', '0' }
                }
            }
        };

        public BathroomSecurity(string[] input, KeyPadMode keyPadMode)
        {
            this.KeyPresses = input;
            this.KeyPadMode = keyPadMode;
            this.Point = this.KeyPadMode == KeyPadMode.Simple ? new Vector<int>(1, 1) : new Vector<int>(1, 3);
        }

        public string[] KeyPresses { get; }

        public KeyPadMode KeyPadMode { get; }

        public Vector<int> Point { get; private set; }

        public string KeyCode()
        {
            StringBuilder result = new();

            foreach (string directions in this.KeyPresses)
            {
                foreach (char direction in directions)
                {
                    this.Move(direction);
                }

                result.Append(this.GetKey());
            }

            return result.ToString();
        }

        private void Move(char direction)
        {
            Vector<int> point = this.Point + CardinalHelper.CardinalTransform<int>()[CardinalHelper.LetterToCardinalMap[direction]];

            if (KeyPads[this.KeyPadMode][point.Y, point.X] != '0')
            {
                this.Point = point;
            }
        }

        private char GetKey() => KeyPads[this.KeyPadMode][this.Point.Y, this.Point.X];
    }
}
