namespace AdventOfCode.Puzzles._2022.Day_11___Monkey_in_the_Middle
{
    public class Monkey
    {
        public Monkey(
            int id,
            List<long> startingItems,
            string operationA,
            string operationB,
            string operation,
            long test,
            long testTrue,
            long testFalse)
        {
            this.Id = id;
            this.StartingItems = startingItems;
            this.OperationA = operationA;
            this.OperationB = operationB;
            this.Operation = operation;
            this.Test = test;
            this.TestTrue = testTrue;
            this.TestFalse = testFalse;
        }

        public int Id { get; private set; }

        public List<long> StartingItems { get; private set; }

        public string OperationA { get; private set; }

        public string OperationB { get; private set; }

        public string Operation { get; private set; }

        public long Test { get; private set; }

        public long TestTrue { get; private set; }

        public long TestFalse { get; private set; }

        public long Inspections { get; private set; }

        public new string ToString() => $"{this.Inspections}";

        public void IncrementInspection() => this.Inspections++;
    }
}
