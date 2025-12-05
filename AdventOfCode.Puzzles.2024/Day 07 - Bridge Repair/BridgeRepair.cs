namespace AdventOfCode.Puzzles._2024.Day_07___Bridge_Repair
{
    public class BridgeRepair
    {
        public List<Equation> Equations { get; private set; }

        public BridgeRepair(string[] input) => this.Equations = Parse(input);

        private static List<Equation> Parse(string[] input)
        {
            List<Equation> result = [];

            foreach (string line in input)
            {
                string[] tokens = line.Split(": ");

                result.Add(new (long.Parse(tokens[0]), tokens[1].Split(" ").Select(x => long.Parse(x)).ToList()));
            }

            return result;
        }

        private static void Enqueue(Queue<EquationState> queue, long testValue, int index, bool concat) 
        {
            queue.Enqueue(new(testValue, OperatorEnum.Add, index));
            queue.Enqueue(new(testValue, OperatorEnum.Multiply, index));

            if (concat)
            {
                queue.Enqueue(new(testValue, OperatorEnum.Concat, index));
            }
        }

        public long Calibrate(bool concat = false)
        {
            long result = 0;
            Queue<EquationState> queue = new();

            foreach (Equation equation in this.Equations)
            {
                Enqueue(queue, equation.Values[0], 1, concat);

                while (queue.Count > 0)
                {
                    EquationState state = queue.Dequeue();

                    switch (state.Operator)
                    {
                        case OperatorEnum.Add:
                            state.Result += equation.Values[state.Index];
                            break;
                        case OperatorEnum.Multiply:
                            state.Result *= equation.Values[state.Index];
                            break;
                        case OperatorEnum.Concat:
                            state.Result = long.Parse($"{state.Result}{equation.Values[state.Index]}");
                            break;
                    }

                    if (state.Result == equation.TestValue)
                    {
                        result += state.Result;
                        queue.Clear();
                        break;
                    }
                    else if(state.Index < equation.Values.Count - 1)
                    {
                        Enqueue(queue, state.Result, state.Index + 1, concat);
                    }
                }
            }

            return result;
        }
    }
}
