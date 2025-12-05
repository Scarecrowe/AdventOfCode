namespace AdventOfCode.Puzzles._2021.Day_23___Amphipod
{
    using AdventOfCode.Core.Extensions;

    public class AmphipodMap
    {
        public static readonly Dictionary<AmphipodType, int> Energy = new() { { AmphipodType.Amber, 1 }, { AmphipodType.Bronze, 10 }, { AmphipodType.Copper, 100 }, { AmphipodType.Desert, 1000 } };

        public AmphipodMap(string[] input, bool advanced = false)
        {
            this.Corridor = Enumerable.Repeat(AmphipodType.Empty, 11).ToArray();
            this.Corridor[2] = this.Corridor[4] = this.Corridor[6] = this.Corridor[8] = AmphipodType.Forbidden;
            this.Rooms = !advanced ? SimpleRooms(input) : AdvancedRooms(input);
            this.RoomSize = advanced ? 4 : 2;
        }

        public AmphipodMap(int energy, AmphipodType[] corridor, Stack<AmphipodType>[] rooms, bool advanced = false)
        {
            this.TotalEnergy = energy;
            this.Corridor = corridor;
            this.Rooms = rooms;
            this.RoomSize = advanced ? 4 : 2;
        }

        public AmphipodMap(int energy, AmphipodType[] corridor, Stack<AmphipodType>[] rooms, int roomSize)
        {
            this.TotalEnergy = energy;
            this.Corridor = corridor;
            this.Rooms = rooms;
            this.RoomSize = roomSize;
        }

        public int TotalEnergy { get; private set; }

        public AmphipodType[] Corridor { get; private set; }

        public Stack<AmphipodType>[] Rooms { get; private set; }

        public int RoomSize { get; }

        public static int GetIndex(AmphipodType amphipodType) => (int)amphipodType - 'A';

        public (int RoomIndex, Stack<AmphipodType> Room) GetRoom(AmphipodType amphipodType)
        {
            return amphipodType switch
            {
                AmphipodType.Empty or AmphipodType.Forbidden => (-1, new()),
                _ => ((int)amphipodType - 'A', this.Rooms[(int)amphipodType - 'A']),
            };
        }

        public AmphipodMap Clone()
            => new(this.TotalEnergy, this.Corridor.ToArray(), this.Rooms.Select(x => new Stack<AmphipodType>(x.Reverse().ToArray())).ToArray(), this.RoomSize);

        public string ToKey()
            => $"{this.Corridor.Select(x => (char)x).Join()}~{this.Rooms.Select(x => x.Select(y => y).Join().PadLeft(4, '.')).Join("~")}";

        public bool IsComplete() =>
            this.IsRoomComplete(AmphipodType.Amber)
            && this.IsRoomComplete(AmphipodType.Bronze)
            && this.IsRoomComplete(AmphipodType.Copper)
            && this.IsRoomComplete(AmphipodType.Desert);

        public bool IsRoomComplete(AmphipodType amphipodType)
            => this.Rooms[GetIndex(amphipodType)].Count == this.RoomSize && this.Rooms[GetIndex(amphipodType)].All(x => x == amphipodType);

        public AmphipodMap Move(int steps, AmphipodType amphipodType)
        {
            this.TotalEnergy += steps * Energy[amphipodType];

            return this;
        }

        public AmphipodMap Empty(int corridor)
        {
            this.Corridor[corridor] = AmphipodType.Empty;

            return this;
        }

        public AmphipodMap Push(int room, int corridor)
        {
            this.Rooms[room].Push(this.Corridor[corridor]);

            return this;
        }

        public AmphipodMap Pop(int room)
        {
            this.Rooms[room].Pop();

            return this;
        }

        public AmphipodMap Set(int index, AmphipodType amphipodType)
        {
            this.Corridor[index] = amphipodType;

            return this;
        }

        public void ProcessRooms(PriorityQueue<AmphipodMap, int> queue)
        {
            for (var i = 0; i < this.Rooms.Length; i++)
            {
                Stack<AmphipodType> room = this.Rooms[i];

                AmphipodType amphipod = room.Any() ? room.Peek() : AmphipodType.Empty;

                if (amphipod == AmphipodType.Empty)
                {
                    continue;
                }

                this.ProcessCorridor(queue, true, i, amphipod);
                this.ProcessCorridor(queue, false, i, amphipod);
            }
        }

        public void ProcessCorridors(PriorityQueue<AmphipodMap, int> queue)
        {
            for (var i = 0; i < this.Corridor.Length; i++)
            {
                var (roomIndex, room) = this.GetRoom(this.Corridor[i]);

                if (this.Corridor[i] >= AmphipodType.Empty
                    || room.Any(x => x != this.Corridor[i])
                    || !this.IsPathEmpty(i, (roomIndex + 1) * 2))
                {
                    continue;
                }

                Enqueue(queue, this.Clone()
                    .Move(this.RoomSize - this.Rooms[roomIndex].Count + Math.Abs(i - ((roomIndex + 1) * 2)), this.Corridor[i])
                    .Push(roomIndex, i)
                    .Empty(i));
            }
        }

        private static Stack<AmphipodType>[] SimpleRooms(string[] input)
        {
            return new[]
            {
                new Stack<AmphipodType>(new[] { (AmphipodType)input[3][3], (AmphipodType)input[2][3] }),
                new Stack<AmphipodType>(new[] { (AmphipodType)input[3][5], (AmphipodType)input[2][5] }),
                new Stack<AmphipodType>(new[] { (AmphipodType)input[3][7], (AmphipodType)input[2][7] }),
                new Stack<AmphipodType>(new[] { (AmphipodType)input[3][9], (AmphipodType)input[2][9] })
            };
        }

        private static Stack<AmphipodType>[] AdvancedRooms(string[] input)
        {
            return new[]
            {
                new Stack<AmphipodType>(new[] { (AmphipodType)input[3][3], AmphipodType.Desert, AmphipodType.Desert, (AmphipodType)input[2][3] }),
                new Stack<AmphipodType>(new[] { (AmphipodType)input[3][5], AmphipodType.Bronze, AmphipodType.Copper, (AmphipodType)input[2][5] }),
                new Stack<AmphipodType>(new[] { (AmphipodType)input[3][7], AmphipodType.Amber, AmphipodType.Bronze, (AmphipodType)input[2][7] }),
                new Stack<AmphipodType>(new[] { (AmphipodType)input[3][9], AmphipodType.Copper, AmphipodType.Amber, (AmphipodType)input[2][9] })
            };
        }

        private static void Enqueue(PriorityQueue<AmphipodMap, int> queue, AmphipodMap map) => queue.Enqueue(map, map.TotalEnergy);

        private bool IsPathEmpty(int i, int index)
        {
            Func<int, int, bool> loop = new((i, index) => i < index ? i < index : i > index);
            Func<int, int> incrementor = new((i) => i < index ? i + 1 : i - 1);
            i = incrementor(i);

            for (; loop.Invoke(i, index); i = incrementor.Invoke(i))
            {
                if (this.Corridor[i] < AmphipodType.Empty)
                {
                    return false;
                }
            }

            return true;
        }

        private void ProcessCorridor(PriorityQueue<AmphipodMap, int> queue, bool left, int room, AmphipodType amphipodType)
        {
            Func<int, AmphipodType, bool> loop = new((index, corridor) => left
                ? index >= 0 && corridor >= AmphipodType.Empty
                : index < this.Corridor.Length && corridor >= AmphipodType.Empty);

            Func<int, int> incrementor = new((i) => left ? i - 1 : i + 1);
            Func<int, int, int> mover = new((index, corridorIndex) => left ? index - corridorIndex : corridorIndex - index);
            AmphipodType corridor;
            int index = (room * 2) + 2;
            int corridorIndex = incrementor(index);

            do
            {
                corridor = this.Corridor[corridorIndex];

                if (corridor == AmphipodType.Empty)
                {
                    Enqueue(queue, this.Clone()
                        .Move(this.RoomSize + 1 - this.Rooms[room].Count + mover(index, corridorIndex), amphipodType)
                        .Pop(room)
                        .Set(corridorIndex, amphipodType));
                }

                corridorIndex = incrementor(corridorIndex);
            }
            while (loop(corridorIndex, corridor));
        }
    }
}
