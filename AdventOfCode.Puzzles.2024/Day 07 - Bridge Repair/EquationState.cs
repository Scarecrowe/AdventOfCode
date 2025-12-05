namespace AdventOfCode.Puzzles._2024.Day_07___Bridge_Repair
{
    public class EquationState
    {
        public long Result { get; set; }

        public OperatorEnum Operator { get; private set; }

        public int Index { get; set; }

        public EquationState(long result, OperatorEnum @operator, int index)
        {
            this.Result = result;
            this.Operator = @operator;
            this.Index = index;
        }
    }
}
