namespace AdventOfCode.Core.ConsoleMenu
{
    using AdventOfCode.Core;

    public class DayMenu : ConsoleMenu, IConsoleMenu
    {
        public DayMenu(string title, string subTitle, string bacKText, int year)
            : base(title)
        {
            this.Day = -1;
            this.Year = year;
            this.SetSubTitle(subTitle);
            this.BackText = bacKText;
        }

        public int Year { get; private set; }

        public int Day { get; private set; }

        public string BackText { get; private set; }

        public bool GoBack { get; private set; }

        public async Task<IConsoleMenu> Execute()
        {
            while(true)
            {
                this.ResetDay();
                int selection = await this.PromptDay();

                if (this.GoBack
                    || this.MainMenu)
                {
                    return this;
                }

                if (selection > 0)
                {
                    this.Day = selection;
                    return this;
                }
            }
        }

        public void ResetDay()
        {
            this.Day = -1;
        }

        private async Task<int> PromptDay()
        {
            this.Items.Clear();
            this.Reset();

            List<string> days = Puzzle.GetPuzzleTitles(this.Year);

            for (int i = 1; i <= (this.Year < 2025 ? 25 : 12); i++)
            {
                this.Items.Add(i, days[i - 1], string.Empty);
            }

            this.AddGenericMenuItems(this.BackText);

            IConsoleMenuItem selection = await this.WriteMenu();

            switch (selection.GetKey<int>())
            {
                case GenericMenu.Back:
                    this.GoBack = true;
                    return -1;
                case GenericMenu.MainMenu:
                    this.GotoMainMenu();
                    return -1;
                case GenericMenu.Exit:
                    await this.Exit();
                    break;
                default:
                    return selection.GetKey<int>();
            }

            return -1;
        }
    }
}
