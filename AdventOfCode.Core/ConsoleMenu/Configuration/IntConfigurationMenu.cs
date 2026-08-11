namespace AdventOfCode.Core.ConsoleMenu.Configuration
{
    using System.Reflection;
    using System.Threading.Tasks;
    using AdventOfCode.Core.ConsoleMenu;
    using AdventOfCode.Core.Extensions;

    public class IntConfigurationMenu : ConsoleMenu, IConsoleMenu
    {
        public IntConfigurationMenu(
            IConsoleMenu parent,
            string title,
            PropertyInfo property,
            IConfiguration configuration)
            : base(parent, title)
        {
            this.Property = property;
            this.Configuration = configuration;
            this.PushHistory(this.Property.Name.SplitCamelCase());
        }

        private PropertyInfo Property { get; }

        private IConfiguration Configuration { get; }

        public Task<IConsoleMenu> Execute()
        {
            this.Reset();

            int value = PromptInt($"Enter a new value for {this.Property.Name.SplitCamelCase()}", (int)(this.Property.GetValue(this.Configuration) ?? 0));

            Reflector.SetConfigurationProperty(this.Property, this.Configuration, value);

            return Task.FromResult(this.Parent);
        }
    }
}
