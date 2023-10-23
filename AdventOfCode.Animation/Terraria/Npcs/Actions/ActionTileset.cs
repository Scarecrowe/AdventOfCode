namespace AdventOfCode.Animation.Terraria.Npcs.Actions
{
    using System.Drawing;
    using AdventOfCode.Animation;
    using AdventOfCode.Animation.Terraria.Renderers;
    using AdventOfCode.Core;

    public class ActionTileset : Tileset
    {
        public ActionTileset()
            : base()
        {
            this.Loop = true;
        }

        public ActionTileset(
            int tileWidth,
            int tileHeight)
           : base()
        {
            this.TileWidth = tileWidth;
            this.TileHeight = tileHeight;
            this.Loop = true;
        }

        public ActionTileset(
            Image image,
            int tileWidth,
            int tileHeight,
            int indexStart,
            int indexEnd)
            : base(image, tileWidth, tileHeight)
        {
            this.Index = indexStart;
            this.IndexStart = indexStart;
            this.IndexEnd = indexEnd;
            this.Fps = 0;
            this.Loop = true;
        }

        public ActionTileset(
            Image image,
            int tileWidth,
            int tileHeight,
            Vector<int> offset,
            int indexStart,
            int indexEnd)
            : base(image, tileWidth, tileHeight, offset)
        {
            this.Index = indexStart;
            this.IndexStart = indexStart;
            this.IndexEnd = indexEnd;
            this.Fps = 0;
            this.Loop = true;
        }

        protected ActionTileset(
            int tileWidth,
            int tileHeight,
            Vector<int> offset,
            int indexStart,
            int indexEnd,
            VectorDictionary<int, Image> tiles,
            int fps)
            : base()
        {
            this.TileWidth = tileWidth;
            this.TileHeight = tileHeight;
            this.Offset = offset;
            this.Index = indexStart;
            this.IndexStart = indexStart;
            this.IndexEnd = indexEnd;
            this.Tiles = tiles;
            this.Fps = fps;
            this.Loop = true;
        }

        public int Index { get; private set; }

        public int IndexStart { get; private set; }

        public int IndexEnd { get; private set; }

        public int Fps { get; private set;  }

        public Cardinal Direction { get; private set; } = Cardinal.South;

        public Vector<int> Point { get; private set; } = new();

        public bool Loop { get; private set; }

        public bool Paused { get; private set; }

        public void Pause() => this.Paused = true;

        public void Unpause() => this.Paused = true;

        public ActionTileset Reset()
        {
            this.Index = this.IndexStart;
            this.Paused = false;

            return this;
        }

        public ActionTileset SetLoop(bool value)
        {
            this.Loop = value;

            return this;
        }

        public ActionTileset SetIndex(int index)
        {
            this.Index = index;

            return this;
        }

        public ActionTileset SetIndexStart(int index)
        {
            this.IndexStart = index;
            this.Index = index;

            return this;
        }

        public ActionTileset SetIndexEnd(int index)
        {
            this.IndexEnd = index;

            return this;
        }

        public new Image GetImage(int frame)
        {
            Image result = (Image)base.GetImage(this.Index).Clone();

            if (!this.Paused && frame % (WaterFallRenderer.Fps / this.Fps) == 0)
            {
                this.Index++;

                if (this.Index > this.IndexEnd)
                {
                    if (!this.Loop)
                    {
                        this.Index = this.IndexEnd;
                        return result;
                    }

                    this.Index = this.IndexStart;
                }
            }

            return result;
        }

        public ActionTileset SetFps(int fps)
        {
            this.Fps = fps;

            return this;
        }

        public new ActionTileset Clone()
        {
            ActionTileset result = new(
                this.TileWidth,
                this.TileHeight,
                this.Offset,
                this.IndexStart,
                this.IndexEnd,
                this.Tiles,
                this.Fps);

            result.SetLoop(this.Loop);

            return result;
        }
    }
}
