namespace AdventOfCode.Animation.Renderers
{
    public interface IFrameRenderer
    {
        IFrames Frames { get; }

        void RenderFrame(IFrame frame);

        void Finish();
    }
}
