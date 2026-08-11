namespace AdventOfCode.Core.ConsoleMenu.Configuration
{
    using System.Drawing;
    using System.Reflection;
    using System.Threading.Tasks;
    using AdventOfCode.Core.ConsoleMenu;
    using AdventOfCode.Core.Extensions;

    public class ConfigurationMenu : ConsoleMenu, IConsoleMenu
    {
        public ConfigurationMenu(
            IConsoleMenu parent,
            string title,
            IConfiguration configuration)
            : base(parent, title)
        {
            this.Configuration = configuration;
            this.PushHistory(title);
        }

        private IConfiguration Configuration { get; }

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

                PropertyInfo? property = selection.GetKey<PropertyInfo>();

                if (property != null)
                {
                    if (Reflector.IsGenericList(property.PropertyType))
                    {
                        object? value = property.GetValue(this.Configuration);

                        if (value == null)
                        {
                            break;
                        }

                        await new ListConfigurationMenu(this, property.Name.SplitCamelCase(), (IListConfiguration)value).Execute();
                    }
                    else
                    {
                        switch (property.PropertyType)
                        {
                            case Type t when t == typeof(int):
                                await new IntConfigurationMenu(this, this.Title, property, this.Configuration).Execute();

                                break;

                            case Type t when t == typeof(bool):
                                await new BoolConfigurationMenu(this, this.Title, property, this.Configuration).Execute();
                                break;

                            case Type t when t == typeof(string):
                                await new StringConfigurationMenu(this, this.Title, property, this.Configuration).Execute();
                                break;

                            case Type t when Nullable.GetUnderlyingType(t) == typeof(Font) || t == typeof(Font):
                                await new FontConfigurationMenu(this, this.Title, property, this.Configuration).Execute();
                                break;

                            case Type t when Nullable.GetUnderlyingType(t) == typeof(Color) || t == typeof(Color):
                                await new ColorConfigurationMenu(this, this.Title, property, this.Configuration).Execute();
                                break;

                            case Type t when t.IsEnum:
                                await new EnumConfigurationMenu(this, this.Title, property, this.Configuration, property.PropertyType).Execute();
                                break;
                            default:
                                Console.WriteLine($"Unhandled type: {property.PropertyType}");
                                break;
                        }

                        if (this.MainMenu)
                        {
                            this.Parent.GotoMainMenu();
                            return this.Parent;
                        }
                    }
                }
            }

            throw new InvalidOperationException();
        }

        private void AddMenuItems()
        {
            this.Items.Clear();

            foreach (PropertyInfo property in this.Configuration.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (Reflector.IsGenericList(property.PropertyType))
                {
                    this.Items.Add(property, property.Name.SplitCamelCase(), string.Empty);
                }
                else
                {
                    object? value = property.GetValue(this.Configuration);

                    if (value == null)
                    {
                        continue;
                    }

                    string display = $"({value})";

                    if (property.PropertyType == typeof(Font))
                    {
                        Font font = (Font)value;

                        display = $"([Font: Name={font.Name}, Size={font.Size}])";
                    }

                    this.Items.Add(property, property.Name.SplitCamelCase(), display);
                }
            }

            this.AddGenericMenuItems("Return to this puzzles animation menu");
        }
    }
}
