namespace AdventOfCode.Animation.Animations._2018.Day_17___Reservoir_Research
{
    using AdventOfCode.Animation.Terraria.Renderers;

    public class Day17
    {
        public Day17()
        {
            this.WaterFall = new WaterFallRenderer();
            this.WaterFall.Render();
        }

        private WaterFallRenderer WaterFall { get; }
    }
}
