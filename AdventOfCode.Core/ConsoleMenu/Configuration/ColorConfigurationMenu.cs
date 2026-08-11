namespace AdventOfCode.Core.ConsoleMenu.Configuration
{
    using System.Reflection;
    using System.Threading.Tasks;
    using AdventOfCode.Core.ConsoleMenu;
    using AdventOfCode.Core.Extensions;

    public class ColorConfigurationMenu : ConsoleMenu, IConsoleMenu
    {
        public ColorConfigurationMenu(
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

            this.Items.Add(ColorConfigurationMenuType.SystemColor, "System Color", "Choose from a list of Windows System Colors");
            this.Items.Add(ColorConfigurationMenuType.CustomColor, "Custom Color", "Set a colour using RGB");
            this.AddGenericMenuItems("Return to the configuration menu");

            IConsoleMenuItem selection = await this.WriteMenu();

            switch (selection.Key)
            {
                case ColorConfigurationMenuType.SystemColor:
                    this.SystemColorMenu();

                    break;
                case ColorConfigurationMenuType.CustomColor:
                    this.RgbColorMenu();

                    break;
                case GenericMenu.Back:
                    break;
                case GenericMenu.MainMenu:
                    this.Parent.GotoMainMenu();
                    break;
            }

            this.PopHistory();

            return this.Parent;
        }

        private void RgbColorMenu()
        {
            this.Items.Clear();

            this.Reset();
            int red = PromptInt("Enter a value for Red:", 255);

            this.Reset();
            int green = PromptInt("Enter a value for Green:", 255);

            this.Reset();
            int blue = PromptInt("Enter a value for Blue:", 255);

            this.Reset();
            int alpha = PromptInt("Enter a value for Alpha:", 255);

            Color color = Color.FromArgb(alpha, red, green, blue);

            Reflector.SetConfigurationProperty(this.Property, this.Configuration, color);
        }

        private async void SystemColorMenu()
        {
            this.Reset();
            this.Items.Clear();

            foreach (KnownColor knownColor in Enum.GetValues<KnownColor>())
            {
                this.Items.Add(Color.FromKnownColor(knownColor), $"{knownColor}", string.Empty);
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
                    Reflector.SetConfigurationProperty(this.Property, this.Configuration, selection.Key);
                    break;
            }
        }
    }
}
