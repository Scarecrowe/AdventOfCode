namespace AdventOfCode.Animation
{
    using AdventOfCode.Animation.Renderers.ConsoleRenderer;

    public interface IConsoleAnimation : IAnimation
    {
        public IConsoleRendererConfiguration ConsoleConfiguration();
    }
}
