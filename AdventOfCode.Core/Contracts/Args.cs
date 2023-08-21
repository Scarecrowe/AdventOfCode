namespace AdventOfCode.Core.Contracts
{
    public static class Args
    {
        public static ArgsShould Should(this object subject) => new(subject);
    }
}
