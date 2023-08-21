namespace AdventOfCode.Puzzles._2019.IntCode
{
    public enum IntcodeOpcodeType
    {
        None = 0,
        Add = 1,
        Mulitply = 2,
        Input = 3,
        Output = 4,
        JumpIfTrue = 5,
        JumpIfFalse = 6,
        LessThan = 7,
        Equal = 8,
        Relative = 9,
        Terminate = 99
    }
}
