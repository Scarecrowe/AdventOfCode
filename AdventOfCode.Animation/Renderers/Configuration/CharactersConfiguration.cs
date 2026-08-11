namespace AdventOfCode.Animation.Renderers.Configuration
{
    using AdventOfCode.Core.ConsoleMenu.Configuration;

    public class CharactersConfiguration : List<ICharacterConfiguration>, ICharactersConfiguration
    {
        public ICharacterConfiguration? GetByCharacter(char character)
            => this.FirstOrDefault(x => x.Character == character);
    }
}
