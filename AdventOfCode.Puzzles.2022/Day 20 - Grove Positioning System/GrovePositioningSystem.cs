namespace AdventOfCode.Puzzles._2022.Day_20___Grove_Positioning_System
{
    using AdventOfCode.Core.Extensions;

    public class GrovePositioningSystem
    {
        public GrovePositioningSystem(string[] input) => this.Coordinates = BuildCoordinates(input);

        public List<GroveCoordinate> Coordinates { get; private set; }

        public long DecryptionKey { get; private set; } = 811589153;

        public GrovePositioningSystem Decrypt()
        {
            foreach (var coordinate in this.Coordinates)
            {
                coordinate.Decrypt(this.DecryptionKey);
            }

            for (int i = 1; i <= 10; i++)
            {
                this.Cycle();
            }

            return this;
        }

        public GrovePositioningSystem Sort()
        {
            GroveCoordinate first = this.Coordinates.First(x => x.Value == 0);
            List<GroveCoordinate> result = new() { first };
            GroveCoordinate? current = first;

            while (current?.Next != first)
            {
                result.Add(current?.Next ?? new(0));
                current = current?.Next;
            }

            this.Coordinates = result;

            return this;
        }

        public long Sum()
        {
            this.Sort();

            long result = 0;

            for (int i = 1000; i <= 3000; i += 1000)
            {
                result += this.Coordinates[i % this.Coordinates.Count].Value;
            }

            return result;
        }

        public GrovePositioningSystem Cycle()
        {
            foreach (GroveCoordinate coordinate in this.Coordinates)
            {
                coordinate?.Previous?.SetNext(coordinate?.Next ?? new(0));
                coordinate?.Next?.SetPrevious(coordinate?.Previous ?? new(0));

                GroveCoordinate? previous = coordinate?.Previous;
                GroveCoordinate? next = coordinate?.Next;

                for (int i = 0; i < coordinate?.Value.Abs() % (this.Coordinates.Count - 1); i++)
                {
                    if (coordinate?.Value < 0)
                    {
                        previous = previous?.Previous;
                        next = next?.Previous;
                    }
                    else
                    {
                        previous = previous?.Next;
                        next = next?.Next;
                    }
                }

                previous?.SetNext(coordinate ?? new(0));
                coordinate?.SetPrevious(previous ?? new(0));
                next?.SetPrevious(coordinate ?? new(0));
                coordinate?.SetNext(next ?? new(0));
            }

            return this;
        }

        private static List<GroveCoordinate> BuildCoordinates(string[] input)
        {
            List<GroveCoordinate> result = input
                .Select(x => new GroveCoordinate(x.ToLong()))
                .ToList();

            for (int i = 0; i < result.Count; i++)
            {
                if (i > 0)
                {
                    result[i].SetPrevious(result[i - 1]);
                }

                if (i < result.Count - 1)
                {
                    result[i].SetNext(result[i + 1]);
                }
            }

            var first = result.First();
            var last = result.Last();
            first.SetPrevious(last);
            last.SetNext(first);

            return result;
        }
    }
}
