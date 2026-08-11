namespace AdventOfCode.Animation.Renderers.Configuration
{
    using System.Drawing;

    public interface ICharacterConfiguration
    {
        char Character { get; }

        Font? Font { get; }

        Color? Color { get; }

        bool Visible { get; }

        string Replace { get; }

        ICharacterConfiguration SetFont(Font font);

        ICharacterConfiguration SetColor(Color color);

        ICharacterConfiguration SetVisible(bool visible);

        ICharacterConfiguration SetReplace(string replace);

        string ToString();
    }
}
