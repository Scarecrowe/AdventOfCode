namespace AdventOfCode.Core.ConsoleMenu.Configuration
{
    using System.Reflection;
    using System.Threading.Tasks;
    using AdventOfCode.Core.ConsoleMenu;
    using AdventOfCode.Core.Extensions;

    public class StringConfigurationMenu : ConsoleMenu, IConsoleMenu
    {
        public StringConfigurationMenu(
            IConsoleMenu parent,
            string title,
            PropertyInfo property,
            IConfiguration configuration)
            : base(parent, title)
        {
            this.Property = property;
            this.Configuration = configuration;
        }

        private PropertyInfo Property { get; }

        private IConfiguration Configuration { get; }

        public Task<IConsoleMenu> Execute()
        {
            this.Reset();
            this.SetSubTitle($"Edit {this.Property.Name.SplitCamelCase()} ({this.Property.GetValue(this.Configuration)})");

            string value = PromptString($"Enter a new value for {this.Property.Name.SplitCamelCase()}", (string)(this.Property.GetValue(this.Configuration) ?? string.Empty));

            Reflector.SetConfigurationProperty(this.Property, this.Configuration, value);

            return Task.FromResult(this.Parent);
        }
    }
}
