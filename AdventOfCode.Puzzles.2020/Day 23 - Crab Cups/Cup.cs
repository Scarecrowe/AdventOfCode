namespace AdventOfCode.Puzzles._2020.Day_23___Crab_Cups
{
    using System.Text;

    public class Cup
    {
        public Cup? Next { get; private set; }

        public long Value { get; private set; }

        public Cup SetNext(Cup cup)
        {
            this.Next = cup;

            return cup;
        }

        public Cup SetValue(long value)
        {
            this.Value = value;

            return this;
        }

        public new string ToString()
        {
            StringBuilder sb = new();
            Cup? current = this.Next;

            do
            {
                sb.Append(current?.Value);
                current = current?.Next;
            }
            while (current != this);

            return sb.ToString();
        }
    }
}
