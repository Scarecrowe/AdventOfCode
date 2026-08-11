namespace AdventOfCode.Animation.Renderers.ConsoleRenderer
{
    using AdventOfCode.Animation.Renderers.Configuration;
    using AdventOfCode.Core.ConsoleMenu.Configuration;

    public interface IConsoleRendererConfiguration : IConfiguration
    {
        string Title { get; }

        ICharactersConfiguration? Characters { get; }

        IConsoleRendererConfiguration WithCharacter(char character, char replace);
    }
}
