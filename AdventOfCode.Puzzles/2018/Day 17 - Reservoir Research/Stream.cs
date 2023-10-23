namespace AdventOfCode.Puzzles._2018.Day_17___Reservoir_Research
{
    using AdventOfCode.Core;

    public class Stream
    {
        public Stream(Vector<long> point) => this.Point = point;

        public Vector<long> Point { get; private set; }

        public void Move(VectorArray<long, EntityType> map, ref Queue<Stream> queue)
        {
            List<EntityType> blocked = new() { EntityType.Clay, EntityType.Settled };
            EntityType below = map[this.Point.Y + 1, this.Point.X];
            EntityType above = map[this.Point.Y - 1, this.Point.X];
            Dictionary<Cardinal, VectorCell<long, EntityType>> adjacent = map.AdjacentCardinal(this.Point)
                .ToDictionary(c => c.Direction, c => c);

            if (map[this.Point] == EntityType.Settled && above == EntityType.Water)
            {
                queue.Enqueue(new Stream(new(this.Point.X, this.Point.Y - 1)));
                return;
            }

            if (adjacent[Cardinal.South].Value == EntityType.Air)
            {
                queue.Enqueue(new Stream(this.MoveSouth(ref map)));
            }

            if (adjacent[Cardinal.South].Value == EntityType.Water && above == EntityType.Air)
            {
                queue.Enqueue(new Stream(this.MoveSouth(ref map)));
            }

            Vector<long>? point = default;
            long westClayIndex = adjacent[Cardinal.West].Value == EntityType.Clay ? this.Point.X : -1;

            if (!adjacent.ContainsKey(Cardinal.East))
            {
                return;
            }

            long eastClayIndex = adjacent[Cardinal.East].Value == EntityType.Clay ? this.Point.X : -1;

            if ((adjacent[Cardinal.West].Value == EntityType.Air || adjacent[Cardinal.West].Value == EntityType.Water)
              && (below == EntityType.Clay || below == EntityType.Settled))
            {
                point = this.MoveWest(ref map, ref westClayIndex);

                if (westClayIndex == -1)
                {
                    queue.Enqueue(new(point));
                }
            }

            if ((adjacent[Cardinal.East].Value == EntityType.Air || adjacent[Cardinal.East].Value == EntityType.Water)
              && (below == EntityType.Clay || below == EntityType.Settled))
            {
                point = this.MoveEast(ref map, ref eastClayIndex);

                if (eastClayIndex == -1)
                {
                    queue.Enqueue(new(point));
                }
            }

            if (westClayIndex > -1 && eastClayIndex > -1)
            {
                for (long i = westClayIndex; i <= eastClayIndex; i++)
                {
                    map[this.Point.Y, i] = EntityType.Settled;
                }

                queue.Enqueue(new(new(this.Point.X, this.Point.Y - 1)));
            }

            if (westClayIndex == -1 && eastClayIndex == -1)
            {
                return;
            }

            adjacent = map.AdjacentCardinal(this.Point).ToDictionary(c => c.Direction, c => c);

            if (adjacent[Cardinal.West].Value == EntityType.Water && adjacent[Cardinal.East].Value == EntityType.Water)
            {
                long streamX = -1;
                long x = -1;

                if (westClayIndex > -1)
                {
                    x = this.Point.X + 1;

                    while (true)
                    {
                        EntityType value = map[this.Point.Y, x];

                        if (value == EntityType.Clay)
                        {
                            for (long i = westClayIndex; i < x; i++)
                            {
                                above = map[this.Point.Y - 1, i];
                                if (above == EntityType.Water)
                                {
                                    streamX = i;
                                }

                                map[this.Point.Y, i] = EntityType.Settled;
                            }

                            break;
                        }

                        if (value == EntityType.Air)
                        {
                            break;
                        }

                        x++;
                    }
                }
                else
                {
                    x = this.Point.X - 1;

                    while (true)
                    {
                        EntityType value = map[this.Point.Y, x];

                        if (value == EntityType.Clay)
                        {
                            for (long i = x + 1; i < eastClayIndex; i++)
                            {
                                above = map[this.Point.Y - 1, i];

                                if (above == EntityType.Water)
                                {
                                    streamX = i;
                                }

                                map[this.Point.Y, i] = EntityType.Settled;
                            }

                            break;
                        }

                        if (value == EntityType.Air)
                        {
                            break;
                        }

                        x--;
                    }
                }

                if (streamX != -1)
                {
                    queue.Enqueue(new(new(streamX, this.Point.Y - 1)));
                }
            }
        }

        private Vector<long> MoveSouth(ref VectorArray<long, EntityType> map)
        {
            map[this.Point.Y, this.Point.X] = EntityType.Water;
            bool drop = true;
            long y = this.Point.Y + 1;
            long x = this.Point.X;

            while (drop)
            {
                if (y >= map.Height)
                {
                    break;
                }

                EntityType value = map[y, x];

                if (value == EntityType.Clay || value == EntityType.Settled)
                {
                    y--;
                    break;
                }
                else
                {
                    map[y, x] = EntityType.Water;
                    y++;
                }
            }

            return new(x, y);
        }

        private Vector<long> MoveWest(ref VectorArray<long, EntityType> map, ref long clayIndex)
        {
            bool slide = true;
            long x = this.Point.X;

            while (slide)
            {
                EntityType value = map[this.Point.Y, x];
                EntityType below = map[this.Point.Y + 1, x];

                if (value == EntityType.Clay)
                {
                    clayIndex = x + 1;
                    break;
                }

                if (below == EntityType.Clay || below == EntityType.Settled)
                {
                    map[this.Point.Y, x] = EntityType.Water;
                    x--;
                }
                else
                {
                    map[this.Point.Y, x] = EntityType.Water;
                    break;
                }
            }

            return new(x, this.Point.Y);
        }

        private Vector<long> MoveEast(ref VectorArray<long, EntityType> map, ref long clayIndex)
        {
            bool slide = true;
            long x = this.Point.X;

            while (slide)
            {
                EntityType value = map[this.Point.Y, x];
                EntityType below = map[this.Point.Y + 1, x];

                if (value == EntityType.Clay)
                {
                    clayIndex = x - 1;
                    break;
                }

                if (below == EntityType.Clay || below == EntityType.Settled)
                {
                    map[this.Point.Y, x] = EntityType.Water;
                    x++;
                }
                else
                {
                    map[this.Point.Y, x] = EntityType.Water;
                    break;
                }
            }

            return new(x, this.Point.Y);
        }
    }
}
