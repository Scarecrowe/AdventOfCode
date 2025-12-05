namespace AdventOfCode.Puzzles._2024.Day_07___Bridge_Repair
{
    public class Equation
    {
        public long TestValue { get; private set; }
        
        public List<long> Values { get; private set; }

        public Equation(long testValue, List<long> values)
        {
            this.TestValue = testValue;
            this.Values = values;
        }
    }
}
