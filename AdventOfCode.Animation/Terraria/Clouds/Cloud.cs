namespace AdventOfCode.Animation.Terraria.Clouds
{
    using System.Drawing;
    using AdventOfCode.Core;

    public class Cloud
    {
        public Cloud(string path, Vector<int> direction)
        {
            this.Point = new(RandomGenerator.Next(0, 1920), RandomGenerator.Next(0, 1080));
            this.Direction = direction;
            this.Image = Terraria.GetImage(path);
        }

        public Vector<int> Point { get; private set; }

        public Vector<int> Direction { get; }

        public Image Image { get; }

        public void Move()
        {
            this.Point += this.Direction;
        }
    }
}
