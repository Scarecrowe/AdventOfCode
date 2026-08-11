namespace AdventOfCode.Animation
{
    using AdventOfCode.Animation.Renderers.AsciiRenderer;

    public interface IAsciiAnimation : IAnimation
    {
        public IAsciiRendererConfiguration AsciiConfiguration();
    }
}
