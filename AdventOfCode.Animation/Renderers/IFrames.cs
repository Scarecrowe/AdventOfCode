namespace AdventOfCode.Animation.Renderers
{
    using System.Drawing;

    public interface IFrames : IList<IFrame>
    {
        void Add(string[] data);
    }
}
