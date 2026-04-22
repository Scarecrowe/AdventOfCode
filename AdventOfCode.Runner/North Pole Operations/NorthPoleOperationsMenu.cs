namespace AdventOfCode.Runner.North_Pole_Operations
{
    using System.Threading.Tasks;
    using AdventOfCode.Core;
    using AdventOfCode.Runner.Frostys_Melting_Room;
    using AdventOfCode.Runner.Menus;
    using AdventOfCode.Runner.Mrs_Claus_Workbench;
    using AdventOfCode.Runner.Rudolphs_Test_Track;
    using AdventOfCode.Runner.Santas_Guanlet;
    using AdventOfCode.Runner.The_Archivists_Library;
    using AdventOfCode.Runner.The_Elves_Workshop_Floor;
    using AdventOfCode.Runner.The_Ghost_Theatre;
    using AdventOfCode.Runner.The_Grinchs_Ledger;
    using AdventOfCode.Runner.The_Head_Elfs_Office;
    using AdventOfCode.Runner.The_Toymakers_Workshop;

    public class NorthPoleOperationsMenu : Menu, IMenu
    {
        public NorthPoleOperationsMenu()
            : base("North Pole Operations")
        {
        }

        public async Task<IMenu> Execute()
        {
            this.Reset();

            PuzzleConsole.WriteLine("1.  The Toymaker's Workshop        - Run a puzzle");
            PuzzleConsole.WriteLine("2.  The Ghost Theatre              - Build an animation");
            PuzzleConsole.WriteLine("3.  Rudolph's Test Track           - Benchmark puzzles");
            PuzzleConsole.WriteLine("4.  The Archivist's Library        - View past runs");
            PuzzleConsole.WriteLine("5.  Mrs Claus' Workbench           - Test with custom input");
            PuzzleConsole.WriteLine("6.  Santa's Gauntlet               - Run all puzzles");
            PuzzleConsole.WriteLine("7.  The Head Elf's Office          - Debug a puzzle");
            PuzzleConsole.WriteLine("8.  The Grinch's Ledger            - View stats");
            PuzzleConsole.WriteLine("9.  Frosty's Melting Room          - Clean up");
            PuzzleConsole.WriteLine("10. The Elves' Workshop Floor      - View system stats");
            PuzzleConsole.WriteLine("11. Santa's Calling It a Day       - Exit");

            PuzzleConsole.WriteLine();
            PuzzleConsole.Flush();

            int option = PromptInt("Select an option: ", 1);

            switch ((NorthPoleOperations)option)
            {
                case NorthPoleOperations.TheToyMakersWorkshop:
                    await new TheToyMakersWorkshopSelectorMenu().Execute();
                    break;
                case NorthPoleOperations.TheGhostTheatre:
                    await new TheGhostTheatreMenu().Execute();
                    break;
                case NorthPoleOperations.RudolphsTestTrack:
                    await new RudolphsTestTrackMenu().Execute();
                    break;
                case NorthPoleOperations.TheArchivistsLibrary:
                    await new TheArchivistsLibraryMenu().Execute();
                    break;
                case NorthPoleOperations.MrsClausWorkbench:
                    await new MrsClausWorkbenchMenu().Execute();
                    break;
                case NorthPoleOperations.SantasGauntlet:
                    await new SantasGauntletMenu().Execute();
                    break;
                case NorthPoleOperations.TheHeadElfsOffice:
                    await new TheHeadElfsOfficeMenu().Execute();
                    break;
                case NorthPoleOperations.TheGrinchsLedger:
                    await new TheGrinchsLedgerMenu().Execute();
                    break;
                case NorthPoleOperations.FrostysMeltingRoom:
                    await new FrostysMeltingRoomMenu().Execute();
                    break;
                case NorthPoleOperations.TheElvesWorkshopFloor:
                    await new TheElvesWorkshopFloorMenu().Execute();
                    break;
                case NorthPoleOperations.SantasCallingItaDay:
                    return await new ExitMenu().Execute();
            }

            return new NorthPoleOperationsMenu();
        }
    }
}
