namespace AdventOfCode.Animation.Terraria.Npcs
{
    using System.Drawing;
    using AdventOfCode.Animation.Extensions;
    using AdventOfCode.Animation.Terraria.Npcs.Actions;
    using AdventOfCode.Animation.Terraria.Renderers;
    using AdventOfCode.Core;
    using AdventOfCode.Core.Extensions;
    using AdventOfCode.Puzzles._2018.Day_17___Reservoir_Research;

    public abstract class Npc
    {
        public Npc(
            NpcType type,
            Frequency spawnFrequency)
        {
            this.Type = type;
            this.SpawnFrequency = spawnFrequency;
            this.Point = new();
            this.Actions = new();
            this.Direction = Cardinal.None;
        }

        public Frequency SpawnFrequency { get; }

        public NpcType Type { get; }

        public Vector<double> Point { get; private set; }

        public int TileWidth => this.Actions[this.ActionType].Tileset.TileWidth;

        public int TileHeight => this.Actions[this.ActionType].Tileset.TileHeight;

        public Cardinal Direction { get; protected set; }

        public ActionType ActionType { get; protected set; }

        public Dictionary<ActionType, IAction> Actions { get; }

        public static bool IsValidPoint(
            Vector<long> point,
            VectorArray<long, EntityType> map)
        {
            if (point.Y >= map.Height
                || point.X >= map.Width
                || point.X < 0
                || point.Y < 0)
            {
                return false;
            }

            return true;
        }

        public void SetDirection(Cardinal direction)
        {
            this.Direction = direction;
        }

        public void SetPoint(Vector<double> point)
        {
            this.Point = point;
        }

        public Vector<long> TilePoint() => new((int)this.Point.X / 16, (int)this.Point.Y / 16);

        public List<Vector<long>> TilePoints()
        {
            List<Vector<long>> result = new();

            long x = ((int)this.Point.X + this.Actions[this.ActionType].Tileset.TileWidth) / 16;
            long y = ((int)this.Point.Y + this.Actions[this.ActionType].Tileset.TileHeight) / 16;

            Vector<long> topLeft = this.TilePoint();
            Vector<long> topRight = new(x, topLeft.Y);
            Vector<long> bottomLeft = new(topLeft.X, y);
            Vector<long> bottomRight = new(x, y);

            result.Add(topLeft);
            result.Add(topRight);
            result.Add(bottomLeft);
            result.Add(bottomRight);

            long diff = topRight.X - topLeft.X;

            if (diff > 1)
            {
                for (x = topLeft.X + 1; x < topRight.X; x++)
                {
                    result.Add(new(x, topLeft.Y));
                }
            }

            diff = bottomLeft.Y - topLeft.Y;

            if (diff > 1)
            {
                for (y = topLeft.Y + 1; y < bottomLeft.Y; y++)
                {
                    result.Add(new(bottomLeft.X, y));
                }
            }

            return result;
        }

        public List<IAction> GetInActiveActions(double frequency)
          => this.Actions
            .Where(x => x.Value.Frequency != null && !x.Value.IsActive)
            .Where(x => frequency >= (x.Value.Frequency?.Min ?? 0.0) && frequency < (x.Value.Frequency?.Max ?? 1.0))
            .Select(x => x.Value)
            .ToList();

        public void SetRandomDirection(
            HashSet<Vector<long>> collisions,
            VectorArray<long, EntityType> map,
            Func<HashSet<Vector<long>>, VectorArray<long, EntityType>, CollisionType> isCollision)
        {
            List<Cardinal> availableMoves = this.AvailableMoves(collisions, map, isCollision);

            if (availableMoves.Count == 0)
            {
                this.SetDirection(Cardinal.None);

                return;
            }

            this.SetDirection(availableMoves[RandomGenerator.Next(0, availableMoves.Count)]);
        }

        public void SetRandomDirection(List<Cardinal> directions)
        {
            this.Direction = directions[RandomGenerator.Next(0, directions.Count)];
        }

        public CollisionType IsCollision(
            HashSet<Vector<long>> collisions,
            VectorArray<long, EntityType> map)
        {
            Vector<long> tilePoint = this.TilePoint();

            if (!IsValidPoint(tilePoint, map))
            {
                return CollisionType.Invalid;
            }

            long x = (int)((this.Point.X + (double)this.Actions[this.ActionType].Tileset.TileWidth) / 16);
            long y = (int)((this.Point.Y + (double)this.Actions[this.ActionType].Tileset.TileHeight) / 16);

            Vector<long> topLeft = this.TilePoint();
            Vector<long> topRight = new(x, topLeft.Y);
            Vector<long> bottomLeft = new(topLeft.X, y);

            bool containsTopLeft = collisions.Contains(topLeft);
            bool containsTopRight = collisions.Contains(topRight);
            bool containsBottomLeft = collisions.Contains(bottomLeft);
            bool containsBottomRight = collisions.Contains(new(x, y));

            if (containsTopRight
                && containsBottomRight
                && !containsTopLeft
                && !containsBottomLeft)
            {
                return CollisionType.Right;
            }

            if (containsTopLeft
                && containsBottomLeft
                && !containsTopRight
                && !containsBottomRight)
            {
                return CollisionType.Left;
            }

            if (containsTopLeft
                 && containsTopRight
                 && !containsBottomLeft
                 && !containsBottomRight)
            {
                return CollisionType.Top;
            }

            if (containsBottomLeft
                 && containsBottomRight
                 && !containsTopLeft
                 && !containsTopRight)
            {
                return CollisionType.Bottom;
            }

            if ((containsTopRight
                    && !containsBottomRight)
                || (containsBottomRight
                    && !containsTopRight))
            {
                return CollisionType.Right;
            }

            if ((containsTopLeft
                    && !containsBottomLeft)
                || (containsBottomLeft
                    && !containsTopLeft))
            {
                return CollisionType.Left;
            }

            long diff = topRight.X - topLeft.X;

            if (diff > 1)
            {
                for (x = topLeft.X + 1; x < topRight.X; x++)
                {
                    if (collisions.Contains(new(x, topLeft.Y)))
                    {
                        return CollisionType.Middle;
                    }
                }
            }

            diff = bottomLeft.Y - topLeft.Y;

            if (diff > 1)
            {
                for (y = topLeft.Y + 1; y < bottomLeft.Y; y++)
                {
                    if (collisions.Contains(new(bottomLeft.X, y)))
                    {
                        return CollisionType.Middle;
                    }
                }
            }

            foreach (KeyValuePair<ActionType, IAction> action in this.Actions)
            {
                CollisionType collision = action.Value.IsCollision(collisions, map);

                if (collision != CollisionType.None)
                {
                    return collision;
                }
            }

            return CollisionType.None;
        }

        public void Update(
            HashSet<Vector<long>> collisions,
            VectorArray<long, EntityType> map,
            int frame)
        {
            foreach (KeyValuePair<ActionType, IAction> action in this.Actions.Where(x => x.Value.IsActive))
            {
                action.Value.Update(collisions, map, frame);
            }

            if (!this.Actions[this.ActionType].CanDeactivate)
            {
                return;
            }

            List<IAction> actions = this.GetInActiveActions(RandomGenerator.NextDouble());

            if (actions.Any())
            {
                IAction action = actions.Count == 1 ? actions[0] : actions[RandomGenerator.Next(0, actions.Count)];

                this.Actions.ForEach(x => x.Value.Deactivate());
                action.Activate();
                this.ActionType = action.ActionType;
            }
        }

        public void Render(
            Graphics graphics,
            Vector<long> point,
            int frame)
        {
            if (this.Type == NpcType.YellowFish)
            {
            }

            Image image = this.Actions[this.ActionType].Tileset.GetImage(frame);

            if (this.Direction == Cardinal.East
                || this.Direction == Cardinal.NorthEast
                || this.Direction == Cardinal.SouthEast)
            {
                image = image.FlipHorizontally();
            }

            graphics.DrawImage(image, (int)this.Point.X - point.X, (int)this.Point.Y - point.Y);
        }

        protected List<Cardinal> AvailableMoves(
            HashSet<Vector<long>> collisions,
            VectorArray<long, EntityType> map,
            Func<HashSet<Vector<long>>, VectorArray<long, EntityType>, CollisionType> isCollision)
        {
            List<Cardinal> result = new();

            foreach (Cardinal cardinal in Enum.GetValues(typeof(Cardinal)))
            {
                if (cardinal == Cardinal.None)
                {
                    continue;
                }

                Vector<double> point = this.Point.Clone();
                this.Point.Transform(cardinal);

                if (isCollision(collisions, map) == CollisionType.None)
                {
                    result.Add(cardinal);
                }

                this.Point.X = point.X;
                this.Point.Y = point.Y;
            }

            return result;
        }

        protected void WithWalk(ActionTileset tileset, double speed)
        {
            this.Actions.TryAddValue(ActionType.Walk, new WalkAction(this, tileset, speed));
        }

        protected void WithWalk(ActionTileset tileset, double speed, Frequency frequency)
        {
            this.WithWalk(tileset, speed);
            this.Actions[ActionType.Walk].SetFrequency(frequency);
        }

        protected void WithSwim(ActionTileset tileset, double speed)
        {
            this.Actions.TryAddValue(ActionType.Swim, new SwimAction(this, tileset, speed));
        }

        protected void WithSwim(ActionTileset tileset, double speed, Frequency frequency)
        {
            this.WithSwim(tileset, speed);
            this.Actions[ActionType.Swim].SetFrequency(frequency);
        }

        protected void WithFly(ActionTileset tileset, double speed)
        {
            this.Actions.TryAddValue(ActionType.Fly, new FlyAction(this, tileset, speed));
        }

        protected void WithFly(ActionTileset tileset, double speed, Frequency frequency)
        {
            this.WithFly(tileset, speed);
            this.Actions[ActionType.Fly].SetFrequency(frequency);
        }

        protected void WithJump(ActionTileset tileset, double speed, double height)
        {
            this.Actions.TryAdd(ActionType.Jump, new JumpAction(this, tileset, speed, height));
        }

        protected void WithJump(ActionTileset tileset, double speed, double height, Frequency frequency)
        {
            this.WithJump(tileset, speed, height);
            this.Actions[ActionType.Jump].SetFrequency(frequency);
        }

        protected void WithIdle(ActionTileset tileset)
        {
            this.Actions.TryAdd(ActionType.Idle, new IdleAction(this, tileset));
        }

        protected void WithIdle(ActionTileset tileset, Frequency frequency)
        {
            this.WithIdle(tileset);
            this.Actions[ActionType.Idle].SetFrequency(frequency);
        }
    }
}
