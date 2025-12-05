namespace AdventOfCode.Puzzles._2022.Day_02___Rock_Paper_Scissors
{
    public interface IRound
    {
        PlayerAShape ValueA { get; }

        object ValueB { get; }

        int Score();
    }
}
