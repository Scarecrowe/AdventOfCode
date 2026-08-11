namespace AdventOfCode.Core.ConsoleMenu.Configuration
{
    using System.Reflection;
    using System.Threading.Tasks;
    using AdventOfCode.Core.ConsoleMenu;
    using AdventOfCode.Core.Extensions;

    public class BoolConfigurationMenu : ConsoleMenu, IConsoleMenu
    {
        public BoolConfigurationMenu(
            IConsoleMenu parent,
            string title,
            PropertyInfo property,
            IConfiguration configuration)
            : base(parent, title)
        {
            this.Property = property;
            this.Configuration = configuration;
            this.PushHistory($"{property.Name.SplitCamelCase()}");
        }

        private PropertyInfo Property { get; }

        private IConfiguration Configuration { get; }

        public async Task<IConsoleMenu> Execute()
        {
            this.Reset();
            this.Items.Clear();
            this.Items.Add(true, "Yes", string.Empty);
            this.Items.Add(false, "No", string.Empty);
            this.AddGenericMenuItems("Return to the configuration menu");

            IConsoleMenuItem selection = await this.WriteMenu();

            switch(selection.Key)
            {
                case GenericMenu.Back:
                    break;
                case GenericMenu.MainMenu:
                    this.Parent.GotoMainMenu();
                    break;
                default:
                    Reflector.SetConfigurationProperty(this.Property, this.Configuration, selection.Key);
                    break;
            }

            this.PopHistory();

            return this.Parent;
        }
    }
}
