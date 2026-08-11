namespace AdventOfCode.Animation.Renderers.ConsoleRenderer
{
    using System.Drawing;
    using AdventOfCode.Animation.Renderers.Configuration;

    public class ConsoleRendererConfiguration : IConsoleRendererConfiguration
    {
        public ConsoleRendererConfiguration(string title)
        {
            this.Title = title;
            this.Characters = new CharactersConfiguration();
        }

        public string Title { get; private set; }

        public ICharactersConfiguration? Characters { get; private set; }

        public IConsoleRendererConfiguration SetTitle(string title)
        {
            this.Title = title;

            return this;
        }

        public IConsoleRendererConfiguration SetCharacters(ICharactersConfiguration characters)
        {
            this.Characters = characters;

            return this;
        }

        public IConsoleRendererConfiguration WithCharacter(char character, char replace)
        {
            this.Characters?.Add(new CharacterConfiguration(character, replace));

            return this;
        }
    }
}
