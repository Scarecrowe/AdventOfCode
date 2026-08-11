namespace AdventOfCode.Core.ConsoleMenu
{
    using AdventOfCode.Core;

    public class YearMenu : ConsoleMenu, IConsoleMenu
    {
        public YearMenu(string title, string backText)
            : base(title)
        {
            this.Year = -1;
            this.BackText = backText;
        }

        public YearMenu(string title, string subTitle, string backText)
           : base(title)
        {
            this.Year = -1;
            this.BackText = backText;
            this.SetSubTitle(subTitle);
        }

        public int Year { get; private set; }

        public string BackText { get; private set; }

        public bool GoBack { get; private set; }

        public async Task<IConsoleMenu> Execute()
        {
            while (true)
            {
                this.ResetYear();
                int selection = await this.PromptYear();

                if (this.GoBack
                    || this.MainMenu)
                {
                    return this;
                }

                if (selection > 0)
                {
                    this.Year = selection;
                    return this;
                }
            }
        }

        public void ResetYear()
        {
            this.Year = -1;
        }

        private async Task<int> PromptYear()
        {
            while (true)
            {
                this.Items.Clear();
                this.Reset();

                for (int i = 2015; i <= 2025; i++)
                {
                    this.Items.Add(i, $"{i}", string.Empty);
                }

                this.AddBackMenuItem(this.BackText);
                this.AddExitMenuItem();

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
            }
        }
    }
}
