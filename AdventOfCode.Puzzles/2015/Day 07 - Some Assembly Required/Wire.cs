namespace AdventOfCode.Puzzles._2015.Day_07___Some_Assembly_Required
{
    public class Wire
    {
        public Wire(string value)
        {
            this.IsAddress = true;
            this.Address = string.Empty;

            if (ushort.TryParse(value, out ushort constant))
            {
                this.IsAddress = false;
                this.Constant = constant;

                return;
            }

            this.Address = value;
        }

        public bool IsAddress { get; }

        public ushort Constant { get; private set; }

        public string Address { get; }

        public new string ToString() => this.IsAddress ? this.Address : $"{this.Constant}";

        public void SetConstant(ushort value) => this.Constant = value;
    }
}
