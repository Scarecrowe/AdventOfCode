namespace AdventOfCode.Animation.Renderers.Configuration
{
    using AdventOfCode.Core.ConsoleMenu.Configuration;

    public interface ICharactersConfiguration : IList<ICharacterConfiguration>, IListConfiguration
    {
        ICharacterConfiguration? GetByCharacter(char character);
    }
}
