namespace AdventOfCode.Runner.North_Pole_Operations
{
    using System.Threading.Tasks;
    using AdventOfCode.Core.ConsoleMenu;
    using AdventOfCode.Runner.Frostys_Melting_Room;
    using AdventOfCode.Runner.Menus;
    using AdventOfCode.Runner.Rudolphs_Test_Track;
    using AdventOfCode.Runner.Santas_Gauntlet;
    using AdventOfCode.Runner.The_Archivists_Library;
    using AdventOfCode.Runner.The_Ghost_Theatre;
    using AdventOfCode.Runner.The_Grinchs_Ledger;
    using AdventOfCode.Runner.The_Head_Elfs_Office;
    using AdventOfCode.Runner.The_Toymakers_Workshop;

    public class NorthPoleOperationsMenu : ConsoleMenu, IConsoleMenu
    {
        public NorthPoleOperationsMenu()
            : base("North Pole Operations")
        {
            this.Items.Add(NorthPoleOperationsMenuType.TheToyMakersWorkshop, "The Toymaker's Workshop", "Run a puzzle");
            this.Items.Add(NorthPoleOperationsMenuType.TheGhostTheatre, "The Ghost Theatre", "Build an animation");
            this.Items.Add(NorthPoleOperationsMenuType.RudolphsTestTrack, "Rudolph's Test Track", "Benchmark puzzles");
            this.Items.Add(NorthPoleOperationsMenuType.TheArchivistsLibrary, "The Archivist's Library", "View past runs");
            this.Items.Add(NorthPoleOperationsMenuType.SantasGauntlet, "Santa's Gauntlet", "Run all puzzles");
            this.Items.Add(NorthPoleOperationsMenuType.TheHeadElfsOffice, "The Head Elf's Office", "View system stats");
            this.Items.Add(NorthPoleOperationsMenuType.TheGrinchsLedger, "The Grinch's Ledger", "View stats");
            this.Items.Add(NorthPoleOperationsMenuType.FrostysMeltingRoom, "Frosty's Melting Room", "Clean up");
            this.AddExitMenuItem();
        }

        public async Task<IConsoleMenu> Execute()
        {
            this.Reset();
            IConsoleMenuItem? item = await this.WriteMenu();

            switch ((NorthPoleOperationsMenuType)(item?.Index ?? 0))
            {
                case NorthPoleOperationsMenuType.TheToyMakersWorkshop:
                    await new TheToyMakersWorkshopSelectorMenu().Execute();
                    break;
                case NorthPoleOperationsMenuType.TheGhostTheatre:
                    await new TheGhostTheatreMenu().Execute();
                    break;
                case NorthPoleOperationsMenuType.RudolphsTestTrack:
                    await new RudolphsTestTrackMenu().Execute();
                    break;
                case NorthPoleOperationsMenuType.TheArchivistsLibrary:
                    await new TheArchivistsLibraryMenu().Execute();
                    break;
                case NorthPoleOperationsMenuType.SantasGauntlet:
                    await new SantasGauntletMenu().Execute();
                    break;
                case NorthPoleOperationsMenuType.TheHeadElfsOffice:
                    await new TheHeadElfsOfficeMenu().Execute();
                    break;
                case NorthPoleOperationsMenuType.TheGrinchsLedger:
                    await new TheGrinchsLedgerMenu().Execute();
                    break;
                case NorthPoleOperationsMenuType.FrostysMeltingRoom:
                    await new FrostysMeltingRoomMenu().Execute();
                    break;
                default:
                    break;
            }

            return new NorthPoleOperationsMenu();
        }
    }
}
