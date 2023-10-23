namespace AdventOfCode.Animation.Terraria.Audio
{
    public class BiomeAudio
    {
        public BiomeAudio(string path, int start, int end)
        {
            this.Path = path;
            this.Start = start;
            this.End = end;
        }

        public string Path { get; }

        public int Start { get; private set; }

        public int End { get; private set; }

        public int Duration => this.End - this.Start;

        public void SetStart(int value)
        {
            this.Start = value;
        }

        public void SetEnd(int value)
        {
            this.End = value;
        }
    }
}
