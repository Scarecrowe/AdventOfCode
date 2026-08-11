namespace AdventOfCode.Runner.Rudolphs_Test_Track
{
    using AdventOfCode.Core.ConsoleMenu;

    public enum RudolphsTestTrackMenuType
    {
        BenchmarkSilver = 1,
        BenchmarkGold = 2,
        BenchmarkBoth = 3,
        SetExecutionCount = 4,
        Back = GenericMenu.Back,
        MainMenu = GenericMenu.MainMenu,
        Exit = GenericMenu.Exit,
    }
}
