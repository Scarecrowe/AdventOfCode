namespace AdventOfCode.Animation.Renderers.Configuration
{
    using System.Drawing;
    using AdventOfCode.Core.ConsoleMenu.Configuration;
    using AdventOfCode.Core.Contracts;

    public class CharacterConfiguration : ICharacterConfiguration, IConfiguration
    {
        public CharacterConfiguration(
            char character,
            Font? font,
            Color color)
        {
            this.Character = character;
            this.Font = font;
            this.Color = color;
            this.Visible = true;
            this.Replace = string.Empty;
        }

        public CharacterConfiguration(
           char character,
           bool visible)
        {
            this.Character = character;
            this.Visible = visible;
            this.Replace = string.Empty;
        }

        public CharacterConfiguration(
           char character,
           char replace)
        {
            this.Character = character;
            this.Replace = replace.ToString();
        }

        public CharacterConfiguration(
            char chr,
            Font? font,
            Color color,
            string replace)
        {
            this.Character = chr;
            this.Font = font;
            this.Color = color;
            this.Visible = true;
            this.Replace = replace;
        }

        public char Character { get; private set; }

        public Font? Font { get; private set; }

        public Color? Color { get; private set; }

        public bool Visible { get; private set; }

        public string Replace { get; private set; }

        public ICharacterConfiguration SetFont(Font font)
        {
            font.Should().Not().BeNull();
            this.Font = font;

            return this;
        }

        public ICharacterConfiguration SetColor(Color color)
        {
            color.Should().Not().BeNull();
            this.Color = color;

            return this;
        }

        public ICharacterConfiguration SetVisible(bool visible)
        {
            this.Visible = visible;

            return this;
        }

        public ICharacterConfiguration SetReplace(string replace)
        {
            this.Replace = replace;

            return this;
        }

        public override string ToString()
        {
            return this.Character.ToString();
        }
    }
}
