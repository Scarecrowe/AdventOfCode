namespace AdventOfCode.Core.ConsoleMenu.Configuration
{
    using System.Threading.Tasks;
    using AdventOfCode.Core.ConsoleMenu;

    public class ListConfigurationMenu : ConsoleMenu, IConsoleMenu
    {
        public ListConfigurationMenu(
            IConsoleMenu parent,
            string title,
            IListConfiguration configuration)
            : base(parent, title)
        {
            this.Configuration = configuration;
            this.PushHistory(title);
        }

        private IListConfiguration Configuration { get; }

        public async Task<IConsoleMenu> Execute()
        {
            while (true)
            {
                this.Reset();
                this.AddMenuItems();
                IConsoleMenuItem? selection = await this.WriteMenu();

                switch (selection.GetKey<int>())
                {
                    case GenericMenu.Back:
                        return this.Parent;
                    case GenericMenu.MainMenu:
                        this.Parent.GotoMainMenu();
                        return this.Parent;
                }

                object? value = this.Configuration[selection.Index - 1];

                if (value == null)
                {
                    break;
                }

                IConsoleMenu menu = await new ConfigurationMenu(this, $"{value}", (IConfiguration)value).Execute();

                if (menu.MainMenu)
                {
                    this.Parent.GotoMainMenu();
                    return this.Parent;
                }
            }

            throw new InvalidOperationException();
        }

        private void AddMenuItems()
        {
            this.Items.Clear();

            if (this.Configuration is System.Collections.IList list)
            {
                foreach (var value in list)
                {
                    this.Items.Add($"{value}", string.Empty);
                }
            }

            this.AddGenericMenuItems("Return to this puzzles animation menu");
        }
    }
}
