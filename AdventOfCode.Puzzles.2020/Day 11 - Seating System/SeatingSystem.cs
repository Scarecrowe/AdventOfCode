namespace AdventOfCode.Puzzles._2020.Day_11___Seating_System
{
    using AdventOfCode.Core;
    using AdventOfCode.Core.Extensions;

    public class SeatingSystem
    {
        public SeatingSystem(string[] input) => this.Map = new(input, (chr) => (Entity)chr);

        private VectorArray<long, Entity> Map { get; set; }

        public long VisibleSeatCount()
        {
            Dictionary<Vector<long>, List<VectorCell<long, Entity>>> visible = this.VisibleSeats();

            while (true)
            {
                bool stateChanged = false;

                VectorArray<long, Entity> map = new(this.Map);

                this.Map.AxisEnumerator().Where(cell => cell.Value != Entity.Floor).ForEach(cell =>
                {
                    if (cell.Value == Entity.OccupiedSeat)
                    {
                        this.SetSeat(visible, cell, map, ref stateChanged, (value) => value >= 5, Entity.EmptySeat);
                    }
                    else
                    {
                        this.SetSeat(visible, cell, map, ref stateChanged, (value) => value == 0, Entity.OccupiedSeat);
                    }
                });

                if (!stateChanged)
                {
                    return this.Map.Count(Entity.OccupiedSeat);
                }

                this.Map = map;
            }
        }

        public long SeatCount()
        {
            while (true)
            {
                bool stateChanged = false;

                VectorArray<long, Entity> map = new(this.Map);

                foreach (VectorCell<long, Entity> cell in this.Map.AxisEnumerator())
                {
                    switch (cell.Value)
                    {
                        case Entity.OccupiedSeat:
                            List<VectorCell<long, Entity>> adjacent = this.Map.AdjacentInterCardinal(cell.Point).ToList();

                            if (adjacent.Count(x => x.Value == Entity.OccupiedSeat) >= 4)
                            {
                                map[cell.Point] = Entity.EmptySeat;
                                stateChanged = true;
                            }

                            break;
                        case Entity.EmptySeat:
                            adjacent = this.Map.AdjacentInterCardinal(cell.Point).ToList();

                            if (!adjacent.Any(x => x.Value == Entity.OccupiedSeat))
                            {
                                map[cell.Point] = Entity.OccupiedSeat;
                                stateChanged = true;
                            }

                            break;
                    }
                }

                if (!stateChanged)
                {
                    return this.Map.Count(Entity.OccupiedSeat);
                }

                this.Map = map;
            }
        }

        private void SetSeat(
            Dictionary<Vector<long>, List<VectorCell<long, Entity>>> visible,
            VectorCell<long, Entity> cell,
            VectorArray<long, Entity> map,
            ref bool stateChanged,
            Func<int, bool> equality,
            Entity value)
        {
            List<VectorCell<long, Entity>> adjacent = visible[cell.Point];

            int count = 0;

            foreach (var item in adjacent)
            {
                if (this.Map[item.Point] == Entity.OccupiedSeat)
                {
                    count++;
                }
            }

            if (equality(count))
            {
                map[cell.Point] = value;
                stateChanged = true;
            }
        }

        private Dictionary<Vector<long>, List<VectorCell<long, Entity>>> VisibleSeats()
        {
            Dictionary<Vector<long>, List<VectorCell<long, Entity>>> result = new();

            foreach (VectorCell<long, Entity> cell in this.Map.AxisEnumerator())
            {
                result.Add(cell.Point, new());

                foreach (VectorCell<long, Entity> cardinal in CardinalHelper.AllCells<long, Entity>())
                {
                    Vector<long> point = cell.Point.Clone();

                    while (true)
                    {
                        point += cardinal.Point;

                        if (this.Map.IsVectorInRange(point))
                        {
                            if (this.Map[point] != Entity.Floor)
                            {
                                result[cell.Point].Add(new(point, Entity.Floor, cardinal.Direction));
                                break;
                            }

                            continue;
                        }

                        break;
                    }
                }
            }

            return result;
        }
    }
}
