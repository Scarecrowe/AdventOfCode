namespace AdventOfCode.Animation.Terraria.Renderers
{
    using System.Drawing;
    using AdventOfCode.Animation.Terraria.Basins;
    using AdventOfCode.Animation.Terraria.Random;
    using AdventOfCode.Puzzles._2018.Day_17___Reservoir_Research;

    public class OutroRenderer
    {
        public OutroRenderer(RenderParameters parameters)
        {
            this.Parameters = parameters;
            this.AnswerBasin = new();
        }

        public RenderParameters Parameters { get; }

        public Basin AnswerBasin { get; private set; }

        public long MaxY { get; private set; }

        public void SetAnswerBasin(Basin basin)
        {
            this.AnswerBasin = basin;
            this.MaxY = (this.AnswerBasin.Top.Left.Y - 58) * 16;
        }

        public void RenderAnswerStatues(Graphics graphics)
        {
            Console.WriteLine("Loading Outro");

            string answer = new ReservoirResearch(Animation.GetInput(2018, 17)).Settle(false).ToString();

            Console.WriteLine($"Puzzle answer: {answer}");

            CharacterStatueAsset asset = new("Random\\Tiles_337.png");

            long width = answer.Length * 2;
            int center = (int)(this.AnswerBasin.Width / 2);
            long left = (center - (width / 2)) + this.AnswerBasin.Top.Left.X;

            for (int i = 0; i < answer.Length; i++)
            {
                asset.Render(graphics, new(left, this.AnswerBasin.Top.Left.Y - 1), answer[i]);
                left += 2;
            }
        }
    }
}
