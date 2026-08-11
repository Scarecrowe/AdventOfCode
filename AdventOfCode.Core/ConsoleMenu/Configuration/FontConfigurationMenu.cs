namespace AdventOfCode.Core.ConsoleMenu.Configuration
{
    using System.Drawing.Text;
    using System.Reflection;
    using System.Threading.Tasks;
    using AdventOfCode.Core.ConsoleMenu;
    using AdventOfCode.Core.Extensions;

    public class FontConfigurationMenu : ConsoleMenu, IConsoleMenu
    {
        public FontConfigurationMenu(
            IConsoleMenu parent,
            string title,
            PropertyInfo property,
            IConfiguration configuration)
            : base(parent, title)
        {
            this.Property = property;
            this.Configuration = configuration;
            this.PushHistory($"{property.Name.SplitCamelCase()}");
            this.Font = this.Property.GetValue(this.Configuration) as Font;
        }

        private PropertyInfo Property { get; }

        private IConfiguration Configuration { get; }

        private Font? Font { get; set; }

        public async Task<IConsoleMenu> Execute()
        {
            if (this.Font == null)
            {
                return this.Parent;
            }

            while(true)
            {
                if (this.MainMenu)
                {
                    this.Parent.GotoMainMenu();
                    return this.Parent;
                }

                this.Reset();

                this.Items.Clear();
                this.Items.Add(FontConfigurationMenuType.Family, $"Family ({this.Font.FontFamily.Name})", "Set the font family");
                this.Items.Add(FontConfigurationMenuType.Size, $"Size ({this.Font.Size})", "Set the size of the font");
                this.AddGenericMenuItems("Return to the configuration menu");

                IConsoleMenuItem selection = await this.WriteMenu();

                if (selection.GetKey<int>() == GenericMenu.Back)
                {
                    break;
                }

                switch (selection.Key)
                {
                    case FontConfigurationMenuType.Family:
                        this.FamilyMenu();

                        break;
                    case FontConfigurationMenuType.Size:
                        this.SizeMenu();

                        break;
                    case GenericMenu.MainMenu:
                        this.Parent.GotoMainMenu();
                        return this.Parent;
                }
            }

            this.PopHistory();

            return this.Parent;
        }

        private async void FamilyMenu()
        {
            this.Reset();
            this.Items.Clear();

            InstalledFontCollection fonts = new();

            foreach (FontFamily family in fonts.Families)
            {
                this.Items.Add(family, family.Name, string.Empty);
            }

            this.AddGenericMenuItems("Return to the configuration menu");

            IConsoleMenuItem selection = await this.WriteMenu();

            switch (selection.Key)
            {
                case GenericMenu.Back:
                    break;
                case GenericMenu.MainMenu:
                    this.GotoMainMenu();
                    break;
                default:
                    if (this.Font == null)
                    {
                        break;
                    }

                    this.Font = new Font(
                        selection.GetKey<FontFamily>() ?? this.Font.FontFamily,
                        this.Font.Size,
                        this.Font.Style,
                        this.Font.Unit,
                        this.Font.GdiCharSet,
                        this.Font.GdiVerticalFont);

                    Reflector.SetConfigurationProperty(this.Property, this.Configuration, this.Font);
                    break;
            }
        }

        private void SizeMenu()
        {
            this.Reset();
            this.Items.Clear();

            int size = PromptInt("Enter a new font size:", (int)(this.Font?.Size ?? 10));

            if (this.Font == null)
            {
                return;
            }

            this.Font = new Font(
                this.Font.FontFamily,
                size,
                this.Font.Style,
                this.Font.Unit,
                this.Font.GdiCharSet,
                this.Font.GdiVerticalFont);

            Reflector.SetConfigurationProperty(this.Property, this.Configuration, this.Font);
        }
    }
}
