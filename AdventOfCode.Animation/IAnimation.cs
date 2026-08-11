namespace AdventOfCode.Animation
{
    using AdventOfCode.Animation.Renderers;

    public interface IAnimation
    {
        public string DayTitle { get; }

        public int Year { get; }

        public int Day { get; }

        public void SilverFrame(IFrameRenderer renderer);

        public void GoldFrame(IFrameRenderer renderer);
    }
}
