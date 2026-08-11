namespace AdventOfCode.Animation.Renderers
{
    public class Frame : IFrame
    {
        public Frame(string[] data)
        {
            this.Data = data;
        }

        public string[] Data { get; }
    }
}
