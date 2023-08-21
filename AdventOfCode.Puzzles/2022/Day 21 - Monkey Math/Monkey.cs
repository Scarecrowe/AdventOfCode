namespace AdventOfCode.Puzzles._2022.Day_21___Monkey_Math
{
    public class Monkey
    {
        public long Value { get; set; }

        public Monkey? Left { get; set; }

        public Monkey? Right { get; set; }

        public MonkeyOperator Operation { get; set; }

        public long Eval()
        {
            return this.Operation switch
            {
                MonkeyOperator.ADD => (this.Left?.Eval() + this.Right?.Eval()) ?? 0,
                MonkeyOperator.SUBTRACT => (this.Left?.Eval() - this.Right?.Eval()) ?? 0,
                MonkeyOperator.MULTIPLY => (this.Left?.Eval() * this.Right?.Eval()) ?? 0,
                MonkeyOperator.DIVIDE => (this.Left?.Eval() / this.Right?.Eval()) ?? 0,
                MonkeyOperator.EQUALS => (this.Left?.Eval() - this.Right?.Eval()) ?? -1,
                _ => this.Value
            };
        }
    }
}
