namespace AdventOfCode.Animation.Renderers
{
    using System.Drawing;

    public class Frames : List<IFrame>, IFrames
    {
        public void Add(string[] data)
        {
            this.Add(new Frame(data));
        }
    }
}
