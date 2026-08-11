namespace AdventOfCode.Core.ConsoleMenu.Configuration
{
    using System.Reflection;
    using System.Threading.Tasks;
    using AdventOfCode.Core.ConsoleMenu;

    public class EnumConfigurationMenu : ConsoleMenu, IConsoleMenu
    {
        public EnumConfigurationMenu(
            IConsoleMenu parent,
            string title,
            PropertyInfo property,
            IConfiguration configuration,
            Type type)
            : base(parent, title)
        {
            this.Property = property;
            this.Configuration = configuration;
            this.Type = type;
        }

        private PropertyInfo Property { get; }

        private IConfiguration Configuration { get; }

        private Type Type { get; }

        public async Task<IConsoleMenu> Execute()
        {
            this.Reset();
            this.Items.Clear();

            foreach (object? value in Enum.GetValues(this.Type))
            {
                this.Items.Add(value, $"{value}", string.Empty);
            }

            this.AddGenericMenuItems("Return to the configuration menu");

            IConsoleMenuItem selection = await this.WriteMenu();

            switch (selection.Key)
            {
                case GenericMenu.Back:
                    break;
                case GenericMenu.MainMenu:
                    this.Parent.GotoMainMenu();
                    break;
                default:
                    if (selection.Key == null)
                    {
                        break;
                    }

                    Reflector.SetConfigurationProperty(this.Property, this.Configuration, selection.Key);

                    break;
            }

            return this.Parent;
        }
    }
}
