namespace AdventOfCode.Puzzles._2023.Day_16___The_Floor_Will_Be_Lava
{
    using AdventOfCode.Animation.Renderers;
    using AdventOfCode.Core;
    using AdventOfCode.Core.Extensions;
    using System.Text;

    public class TheFloorWillBeLava
    {
        public TheFloorWillBeLava(string[] input)
        {
            this.Map = new(input, (c) => c);
        }

        public TheFloorWillBeLava(string[] input, IFrameRenderer renderer)
            : this(input)
        {
            this.Renderer = renderer;
        }

        private VectorArray<int, char> Map { get; }

        private IFrameRenderer? Renderer { get; }

        private readonly record struct BeamState(Vector<int> Point, Cardinal Direction);

        private readonly record struct BeamStart(Vector<int> Point, Cardinal Direction, int Energized);

        public int Shine()
        {
            List<int> results = new();

            foreach (var cell in this.Map.EdgeEnumerator())
            {
                if (cell.Point == new Vector<int>(0, 0))
                {
                    results.Add(this.Beam(cell.Point, Cardinal.East));
                    results.Add(this.Beam(cell.Point, Cardinal.South));
                    continue;
                }

                if (cell.Point == new Vector<int>(0, this.Map.Width - 1))
                {
                    results.Add(this.Beam(cell.Point, Cardinal.West));
                    results.Add(this.Beam(cell.Point, Cardinal.South));
                    continue;
                }

                if (cell.Point == new Vector<int>(this.Map.Height - 1, 0))
                {
                    results.Add(this.Beam(cell.Point, Cardinal.West));
                    results.Add(this.Beam(cell.Point, Cardinal.North));
                    continue;
                }

                if (cell.Point == new Vector<int>(this.Map.Height - 1, this.Map.Width - 1))
                {
                    results.Add(this.Beam(cell.Point, Cardinal.East));
                    results.Add(this.Beam(cell.Point, Cardinal.North));
                    continue;
                }

                if (cell.Point.Y == 0)
                {
                    results.Add(this.Beam(cell.Point, Cardinal.South));
                    continue;
                }

                if (cell.Point.Y == this.Map.Height - 1)
                {
                    results.Add(this.Beam(cell.Point, Cardinal.North));
                    continue;
                }

                if (cell.Point.X == 0)
                {
                    results.Add(this.Beam(cell.Point, Cardinal.East));
                    continue;
                }

                if (cell.Point.X == this.Map.Width - 1)
                {
                    results.Add(this.Beam(cell.Point, Cardinal.West));
                    continue;
                }
            }

            return results.Max();
        }

        public int Beam(Vector<int> start, Cardinal direction)
        {
            Queue<(Vector<int> Point, Cardinal Direction)> queue = new();
            queue.Enqueue((start.Clone(), direction));

            HashSet<(Vector<int>, Cardinal)> visited = new();

            while (queue.Any())
            {
                var current = queue.Dequeue();

                if (visited.Contains((current.Point, current.Direction))
                    || (current.Point.X < 0 || current.Point.X >= this.Map.Width)
                    || (current.Point.Y < 0 || current.Point.Y >= this.Map.Height))
                {
                    continue;
                }

                visited.Add((current.Point, current.Direction));

                switch (this.Map[current.Point])
                {
                    case '.':
                        queue.Enqueue((current.Point.Clone().Transform(current.Direction), current.Direction));
                        break;
                    case '/':
                        switch (current.Direction)
                        {
                            case Cardinal.North:
                                queue.Enqueue((current.Point.Clone().Transform(Cardinal.East), Cardinal.East));
                                break;
                            case Cardinal.South:
                                queue.Enqueue((current.Point.Clone().Transform(Cardinal.West), Cardinal.West));
                                break;
                            case Cardinal.East:
                                queue.Enqueue((current.Point.Clone().Transform(Cardinal.North), Cardinal.North));
                                break;
                            case Cardinal.West:
                                queue.Enqueue((current.Point.Clone().Transform(Cardinal.South), Cardinal.South));
                                break;
                        }

                        break;
                    case '\\':
                        switch (current.Direction)
                        {
                            case Cardinal.North:
                                queue.Enqueue((current.Point.Clone().Transform(Cardinal.West), Cardinal.West));
                                break;
                            case Cardinal.South:
                                queue.Enqueue((current.Point.Clone().Transform(Cardinal.East), Cardinal.East));
                                break;
                            case Cardinal.East:
                                queue.Enqueue((current.Point.Clone().Transform(Cardinal.South), Cardinal.South));
                                break;
                            case Cardinal.West:
                                queue.Enqueue((current.Point.Clone().Transform(Cardinal.North), Cardinal.North));
                                break;
                        }

                        break;
                    case '|':
                        switch (current.Direction)
                        {
                            case Cardinal.North:
                            case Cardinal.South:
                                queue.Enqueue((current.Point.Clone().Transform(current.Direction), current.Direction));
                                break;
                            case Cardinal.East:
                            case Cardinal.West:
                                queue.Enqueue((current.Point.Clone().Transform(Cardinal.North), Cardinal.North));
                                queue.Enqueue((current.Point.Clone().Transform(Cardinal.South), Cardinal.South));
                                break;
                        }

                        break;
                    case '-':
                        switch (current.Direction)
                        {
                            case Cardinal.North:
                            case Cardinal.South:
                                queue.Enqueue((current.Point.Clone().Transform(Cardinal.East), Cardinal.East));
                                queue.Enqueue((current.Point.Clone().Transform(Cardinal.West), Cardinal.West));
                                break;
                            case Cardinal.East:
                            case Cardinal.West:
                                queue.Enqueue((current.Point.Clone().Transform(current.Direction), current.Direction));
                                break;
                        }

                        break;
                }
            }

            return visited.Select(x => x.Item1).Distinct().Count();
        }

        public TheFloorWillBeLava RenderSilver(int renderEvery = 4)
        {
            return this.RenderBeam(new(0, 0), Cardinal.East, "THE FLOOR WILL BE LAVA", renderEvery);
        }

        public TheFloorWillBeLava RenderGold(int renderEvery = 4)
        {
            BeamStart best = this.FindBestStart();

            return this.RenderBeam(
                best.Point,
                best.Direction,
                $"BEST LAVA CONFIGURATION // MAX {best.Energized:00000}",
                renderEvery);
        }

        private TheFloorWillBeLava RenderBeam(
            Vector<int> start,
            Cardinal direction,
            string title,
            int renderEvery)
        {
            if (this.Renderer == null)
            {
                return this;
            }

            Queue<BeamState> queue = new();
            queue.Enqueue(new(start.Clone(), direction));

            HashSet<BeamState> visited = [];
            HashSet<Vector<int>> energized = [];
            List<BeamState> active = [];

            int step = 0;

            this.RenderFrame(title, step, energized, active);

            while (queue.Any())
            {
                BeamState current = queue.Dequeue();

                if (visited.Contains(current)
                    || current.Point.X < 0
                    || current.Point.X >= this.Map.Width
                    || current.Point.Y < 0
                    || current.Point.Y >= this.Map.Height)
                {
                    continue;
                }

                visited.Add(current);
                energized.Add(current.Point);
                active.Add(current);

                foreach (BeamState next in this.GetNextStates(current))
                {
                    queue.Enqueue(next);
                }

                step++;

                if (step % renderEvery == 0)
                {
                    this.RenderFrame(title, step, energized, active);
                    active.Clear();
                }
            }

            this.RenderFrame(title, step, energized, active);

            for (int i = 0; i < 24; i++)
            {
                this.RenderFrame(
                    $"{title} // ENERGIZED {energized.Count:00000}",
                    step,
                    energized,
                    []);
            }

            return this;
        }

        private IEnumerable<BeamState> GetNextStates(BeamState current)
        {
            switch (this.Map[current.Point])
            {
                case '.':
                    yield return new(current.Point.Clone().Transform(current.Direction), current.Direction);
                    break;

                case '/':
                    switch (current.Direction)
                    {
                        case Cardinal.North:
                            yield return new(current.Point.Clone().Transform(Cardinal.East), Cardinal.East);
                            break;
                        case Cardinal.South:
                            yield return new(current.Point.Clone().Transform(Cardinal.West), Cardinal.West);
                            break;
                        case Cardinal.East:
                            yield return new(current.Point.Clone().Transform(Cardinal.North), Cardinal.North);
                            break;
                        case Cardinal.West:
                            yield return new(current.Point.Clone().Transform(Cardinal.South), Cardinal.South);
                            break;
                    }

                    break;

                case '\\':
                    switch (current.Direction)
                    {
                        case Cardinal.North:
                            yield return new(current.Point.Clone().Transform(Cardinal.West), Cardinal.West);
                            break;
                        case Cardinal.South:
                            yield return new(current.Point.Clone().Transform(Cardinal.East), Cardinal.East);
                            break;
                        case Cardinal.East:
                            yield return new(current.Point.Clone().Transform(Cardinal.South), Cardinal.South);
                            break;
                        case Cardinal.West:
                            yield return new(current.Point.Clone().Transform(Cardinal.North), Cardinal.North);
                            break;
                    }

                    break;

                case '|':
                    switch (current.Direction)
                    {
                        case Cardinal.North:
                        case Cardinal.South:
                            yield return new(current.Point.Clone().Transform(current.Direction), current.Direction);
                            break;
                        case Cardinal.East:
                        case Cardinal.West:
                            yield return new(current.Point.Clone().Transform(Cardinal.North), Cardinal.North);
                            yield return new(current.Point.Clone().Transform(Cardinal.South), Cardinal.South);
                            break;
                    }

                    break;

                case '-':
                    switch (current.Direction)
                    {
                        case Cardinal.North:
                        case Cardinal.South:
                            yield return new(current.Point.Clone().Transform(Cardinal.East), Cardinal.East);
                            yield return new(current.Point.Clone().Transform(Cardinal.West), Cardinal.West);
                            break;
                        case Cardinal.East:
                        case Cardinal.West:
                            yield return new(current.Point.Clone().Transform(current.Direction), current.Direction);
                            break;
                    }

                    break;
            }
        }

        private BeamStart FindBestStart()
        {
            List<BeamStart> starts = [];

            foreach (var cell in this.Map.EdgeEnumerator())
            {
                foreach (Cardinal direction in this.GetEntryDirections(cell.Point))
                {
                    starts.Add(new(cell.Point.Clone(), direction, this.Beam(cell.Point, direction)));
                }
            }

            return starts.OrderByDescending(x => x.Energized).First();
        }

        private IEnumerable<Cardinal> GetEntryDirections(Vector<int> point)
        {
            if (point.Y == 0)
            {
                yield return Cardinal.South;
            }

            if (point.Y == this.Map.Height - 1)
            {
                yield return Cardinal.North;
            }

            if (point.X == 0)
            {
                yield return Cardinal.East;
            }

            if (point.X == this.Map.Width - 1)
            {
                yield return Cardinal.West;
            }
        }

        private void RenderFrame(
            string title,
            int step,
            HashSet<Vector<int>> energized,
            List<BeamState> active)
        {
            List<string> frame = [];

            frame.Add($"{title} // STEP {step:00000} // ENERGIZED {energized.Count:00000}");
            frame.Add(string.Empty);

            for (int y = 0; y < this.Map.Height; y++)
            {
                StringBuilder sb = new();

                for (int x = 0; x < this.Map.Width; x++)
                {
                    Vector<int> point = new(x, y);
                    char map = this.Map[y, x];
                    List<Cardinal> directions = active
                        .Where(x => x.Point == point)
                        .Select(x => x.Direction)
                        .Distinct()
                        .ToList();

                    if (directions.Count > 1)
                    {
                        sb.Append(directions.Count);
                    }
                    else if (directions.Count == 1 && map == '.')
                    {
                        sb.Append(this.DirectionCharacter(directions[0]));
                    }
                    else if (energized.Contains(point) && map == '.')
                    {
                        sb.Append('#');
                    }
                    else if (energized.Contains(point))
                    {
                        sb.Append(this.EnergizedContraptionCharacter(map));
                    }
                    else
                    {
                        sb.Append(map);
                    }
                }

                frame.Add(sb.ToString());
            }

            this.Renderer?.RenderFrame(new Frame([.. frame]));
        }

        private char DirectionCharacter(Cardinal direction)
        {
            return direction switch
            {
                Cardinal.North => '^',
                Cardinal.South => 'v',
                Cardinal.East => '>',
                Cardinal.West => '<',
                _ => '?'
            };
        }

        private char EnergizedContraptionCharacter(char c)
        {
            return c switch
            {
                '/' => '╱',
                '\\' => '╲',
                '|' => '┃',
                '-' => '━',
                _ => c
            };
        }
    }
}
