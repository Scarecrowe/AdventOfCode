namespace AdventOfCode.Puzzles._2022.Day_11___Monkey_in_the_Middle
{
    using AdventOfCode.Core.Extensions;

    public class MonkeyInTheMiddle
    {
        public MonkeyInTheMiddle(List<string> input)
        {
            input.Add(string.Empty);
            this.Monkies = new();

            ParseStep step = ParseStep.Monkey;
            int id = -1;
            List<long> startingItems = new();
            string operationA = string.Empty;
            string operationB = string.Empty;
            string operation = string.Empty;
            int test = -1;
            int testTrue = -1;
            int testFalse = -1;

            foreach (string line in input)
            {
                if (string.IsNullOrEmpty(line))
                {
                    this.Monkies.Add(new(id, startingItems, operationA, operationB, operation, test, testTrue, testFalse));
                    this.Prime *= test;
                    step = ParseStep.Monkey;
                    id = -1;
                    startingItems = new();
                    operationA = string.Empty;
                    operationB = string.Empty;
                    operation = string.Empty;
                    test = -1;
                    testTrue = -1;
                    testFalse = -1;
                    continue;
                }

                switch (step)
                {
                    case ParseStep.Monkey:
                        id = line.SplitSpace()[1].Replace(":").ToInt();
                        step = ParseStep.StartingItems;
                        break;
                    case ParseStep.StartingItems:
                        string[] items = line.Trim().Split(":");

                        foreach (string item in items[1].Split(","))
                        {
                            startingItems.Add(item.Trim().ToInt());
                        }

                        step = ParseStep.Operation;
                        break;
                    case ParseStep.Operation:
                        string[] operations = line.Trim().Split(":")[1].Trim().Replace("new = ").SplitSpace();
                        operationA = operations[0];
                        operationB = operations[2];
                        operation = operations[1];

                        step = ParseStep.Test;
                        break;
                    case ParseStep.Test:
                        test = line.Trim().Replace("Test: divisible by ").ToInt();
                        step = ParseStep.True;
                        break;
                    case ParseStep.True:
                        testTrue = line.Trim().Replace("If true: throw to monkey ").ToInt();
                        step = ParseStep.False;
                        break;
                    case ParseStep.False:
                        testFalse = line.Trim().Replace("If false: throw to monkey ").ToInt();
                        step = ParseStep.False;
                        break;
                }
            }
        }

        public List<Monkey> Monkies { get; private set; }

        public long Prime { get; private set; }

        public long Play(int rounds)
        {
            long mod = this.Monkies.Select(x => x.Test).Aggregate(1, (a, b) => a * (int)b);

            for (int i = 0; i < rounds; i++)
            {
                foreach (Monkey monkey in this.Monkies)
                {
                    foreach (int item in monkey.StartingItems)
                    {
                        monkey.IncrementInspection();

                        long a = monkey.OperationA == "old" ? item : monkey.OperationA.ToInt();
                        long b = monkey.OperationB == "old" ? item : monkey.OperationB.ToInt();
                        long worryLevel = 0;

                        switch (monkey.Operation)
                        {
                            case "+":
                                worryLevel = a + b;
                                break;
                            case "*":
                                worryLevel = a * b;
                                break;
                        }

                        if (rounds == 20)
                        {
                            worryLevel /= 3;
                        }
                        else
                        {
                            worryLevel /= 1L;
                            worryLevel %= mod;
                        }

                        if (worryLevel % monkey.Test == 0)
                        {
                            this.Monkies.First(x => x.Id == monkey.TestTrue).StartingItems.Add(worryLevel);
                        }
                        else
                        {
                            this.Monkies.First(x => x.Id == monkey.TestFalse).StartingItems.Add(worryLevel);
                        }
                    }

                    monkey.StartingItems.Clear();
                }
            }

            List<long> inspections = this.Monkies.Select(x => x.Inspections).ToList();

            inspections.Sort();

            return inspections.Last() * inspections[^2];
        }
    }
}
