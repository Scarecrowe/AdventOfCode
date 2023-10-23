namespace AdventOfCode.Animation.Terraria.Renderers
{
    using System.Drawing;
    using AdventOfCode.Animation.Terraria.Npcs;
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2018.Day_17___Reservoir_Research;

    public class NpcRenderer
    {
        public NpcRenderer(Screen screen, List<INpc> npcs)
        {
            this.Screen = screen;
            this.Npcs = npcs;
        }

        private Screen Screen { get; }

        private List<INpc> Npcs { get; }

        public void Render(
            Graphics graphics,
            int frame)
        {
            Edges edges = this.Screen.Edges();

            foreach (INpc npc in this.Npcs)
            {
                long x = (int)npc.Point.X / 16;
                long y = (int)npc.Point.Y / 16;

                if (x >= edges.TopLeft.X && x <= edges.TopRight.X
                    && y >= edges.TopLeft.Y && y <= edges.BottomLeft.Y)
                {
                    npc.Render(graphics, this.Screen.Point, frame);
                }
            }
        }
    }
}
