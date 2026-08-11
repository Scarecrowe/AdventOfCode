namespace AdventOfCode.Puzzles._2024.Day_16___Reindeer_Maze
{
    using AdventOfCode.Animation.Renderers;
    using AdventOfCode.Core;
    using System.Text;

    public class ReindeerMaze
    {
        public VectorArray<int, char> Map { get; private set; }

        public Vector<int> Reindeer { get; private set; }

        public Vector<int> Finish { get; private set; }

        public IFrameRenderer? Renderer { get; }

        private readonly record struct PathState(Vector<int> Point, Cardinal Direction);

        private sealed record RenderNode(Vector<int> Point, Cardinal Direction, int Score);

        public ReindeerMaze(string[] input)
        {
            this.Map = new VectorArray<int, char>(input, c => c);

            foreach (var cell in this.Map.AxisEnumerator())
            {
                if (cell.Value == 'S')
                {
                    this.Reindeer = cell.Point.Clone();
                    continue;
                }

                if (cell.Value == 'E')
                {
                    this.Finish = cell.Point.Clone();
                }
            }
        }

        public ReindeerMaze(string[] input, IFrameRenderer renderer)
            : this(input)
        {
            this.Renderer = renderer;
        }

        public int BestScore() => this.ShortestPath().Score;

        public int Seats()
        {
            var shortestPath = this.ShortestPath();
            HashSet<Vector<int>> seats = [];

            HashSet<(Vector<int>, Cardinal)> visited = [];
            PriorityQueue<(Vector<int> Point, Cardinal Direction, ReindeerPath Path), int> queue = new();

            queue.Enqueue((this.Reindeer, Cardinal.East, new(1)), 1);
            queue.Enqueue((this.Reindeer, CardinalHelper.Clockwise[Cardinal.East], new(1000)), 1000);
            queue.Enqueue((this.Reindeer, CardinalHelper.AntiClockwise[Cardinal.East], new(1000)), 1000);

            while (queue.Count > 0)
            {
                var state = queue.Dequeue();

                if (visited.Contains((state.Point, state.Direction)))
                {
                    continue;
                }

                visited.Add((state.Point, state.Direction));

                foreach (var cell in this.Map.AdjacentCardinal(state.Point))
                {
                    if (cell.Value == '.')
                    {
                        var path = state.Path.Clone();

                        if (cell.Direction == state.Direction)
                        {
                            path.Score += 1;
                            path.Points.Add((cell.Point, path.Score));
                            queue.Enqueue((cell.Point, state.Direction, path), path.Score);
                        }
                        else if (cell.Direction == CardinalHelper.Clockwise[state.Direction])
                        {
                            path.Score += 1001;
                            path.Points.Add((cell.Point, path.Score));
                            queue.Enqueue((cell.Point, CardinalHelper.Clockwise[state.Direction], path), path.Score);
                        }
                        else if (cell.Direction == CardinalHelper.AntiClockwise[state.Direction])
                        {
                            path.Score += 1001;
                            path.Points.Add((cell.Point, path.Score));
                            queue.Enqueue((cell.Point, CardinalHelper.AntiClockwise[state.Direction], path), path.Score);
                        }
                    }
                    else if (cell.Value == 'E')
                    {
                        state.Path.Score++;
                    }
                }
            }

            PuzzleConsole.WriteLine(this.Map.Print((c, p) =>
            {
                if (seats.Contains(p))
                {
                    return 'O';
                }

                return c;
            }));

            PuzzleConsole.Flush();

            return seats.Count;
        }

        public ReindeerPath ShortestPath()
        {
            int score = 0;
            ReindeerPath result = null;
            HashSet<(Vector<int>, Cardinal)> visited = [];
            PriorityQueue<(Vector<int> Point, Cardinal Direction, ReindeerPath Path), int> queue = new();

            queue.Enqueue((this.Reindeer, Cardinal.East, new(1)), 1);
            queue.Enqueue((this.Reindeer, CardinalHelper.Clockwise[Cardinal.East], new(1000)), 1000);
            queue.Enqueue((this.Reindeer, CardinalHelper.AntiClockwise[Cardinal.East], new(1000)), 1000);

            while (queue.Count > 0)
            {
                var state = queue.Dequeue();

                if (visited.Contains((state.Point, state.Direction)))
                {
                    continue;
                }

                visited.Add((state.Point, state.Direction));

                foreach (var cell in this.Map.AdjacentCardinal(state.Point))
                {
                    if (cell.Value == '.')
                    {
                        var path = state.Path.Clone();

                        if (cell.Direction == state.Direction)
                        {
                            path.Score += 1;
                            path.Points.Add((cell.Point, path.Score));
                            queue.Enqueue((cell.Point, state.Direction, path), path.Score);
                        }
                        else if (cell.Direction == CardinalHelper.Clockwise[state.Direction])
                        {
                            path.Score += 1001;
                            path.Points.Add((cell.Point, path.Score));
                            queue.Enqueue((cell.Point, CardinalHelper.Clockwise[state.Direction], path), path.Score);
                        }
                        else if (cell.Direction == CardinalHelper.AntiClockwise[state.Direction])
                        {
                            path.Score += 1001;
                            path.Points.Add((cell.Point, path.Score));
                            queue.Enqueue((cell.Point, CardinalHelper.AntiClockwise[state.Direction], path), path.Score);
                        }
                    }
                    else if (cell.Value == 'E')
                    {
                        state.Path.Score++;

                        if (score == 0)
                        {
                            score = state.Path.Score;
                            result = state.Path;
                        }
                        else if (state.Path.Score < score)
                        {
                            score = state.Path.Score;
                            result = state.Path;
                        }
                    }
                }
            }

            return result;
        }

        public ReindeerMaze RenderSilver(int renderEvery = 4)
        {
            if (this.Renderer == null)
            {
                return this;
            }

            List<RenderNode> path = this.FindSingleBestRenderPath();
            List<string[]> frames = [];
            HashSet<Vector<int>> trail = [];

            for (int i = 0; i < path.Count; i++)
            {
                RenderNode node = path[i];
                trail.Add(node.Point);

                if (i % renderEvery == 0 || i == path.Count - 1)
                {
                    frames.Add(this.BuildFrame(
                        title: $"REINDEER MAZE // BEST SCORE {node.Score:000000}",
                        current: node.Point,
                        direction: node.Direction,
                        trail: trail,
                        seats: [],
                        showSeats: false));
                }
            }

            RenderNode last = path.Last();

            for (int i = 0; i < 24; i++)
            {
                frames.Add(this.BuildFrame(
                    title: $"FINISH REACHED // SCORE {last.Score:000000}",
                    current: last.Point,
                    direction: last.Direction,
                    trail: trail,
                    seats: [],
                    showSeats: false));
            }

            this.RenderPaddedFrames(frames);

            return this;
        }

        public ReindeerMaze RenderGold(int revealEvery = 6)
        {
            if (this.Renderer == null)
            {
                return this;
            }

            int bestScore = this.CalculateBestScoreForRender();
            HashSet<Vector<int>> seats = this.FindBestSeatTiles(bestScore);
            List<Vector<int>> orderedSeats = seats
                .OrderBy(p => p.Y)
                .ThenBy(p => p.X)
                .ToList();

            List<string[]> frames = [];
            HashSet<Vector<int>> revealed = [];

            for (int i = 0; i < orderedSeats.Count; i++)
            {
                revealed.Add(orderedSeats[i]);

                if (i % revealEvery == 0 || i == orderedSeats.Count - 1)
                {
                    frames.Add(this.BuildFrame(
                        title: $"BEST SEATS // {revealed.Count:000}/{seats.Count:000} // SCORE {bestScore:000000}",
                        current: null,
                        direction: Cardinal.East,
                        trail: [],
                        seats: revealed,
                        showSeats: true));
                }
            }

            for (int i = 0; i < 24; i++)
            {
                frames.Add(this.BuildFrame(
                    title: $"ALL BEST SEATS FOUND // {seats.Count:000} TILES",
                    current: null,
                    direction: Cardinal.East,
                    trail: [],
                    seats: seats,
                    showSeats: true));
            }

            this.RenderPaddedFrames(frames);

            return this;
        }

        private List<RenderNode> FindSingleBestRenderPath()
        {
            PathState start = new(this.Reindeer, Cardinal.East);
            PriorityQueue<PathState, int> queue = new();
            Dictionary<PathState, int> distance = [];
            Dictionary<PathState, PathState> cameFrom = [];

            queue.Enqueue(start, 0);
            distance[start] = 0;

            while (queue.Count > 0)
            {
                PathState current = queue.Dequeue();
                int currentScore = distance[current];

                if (current.Point == this.Finish)
                {
                    return this.ReconstructRenderPath(cameFrom, distance, current);
                }

                foreach ((PathState Next, int Cost) in this.GetRenderNeighbours(current))
                {
                    int nextScore = currentScore + Cost;

                    if (!distance.TryGetValue(Next, out int existingScore) || nextScore < existingScore)
                    {
                        distance[Next] = nextScore;
                        cameFrom[Next] = current;
                        queue.Enqueue(Next, nextScore);
                    }
                }
            }

            return [];
        }

        private int CalculateBestScoreForRender()
        {
            Dictionary<PathState, int> fromStart = this.CalculateDistancesFrom(new(this.Reindeer, Cardinal.East), reverse: false);

            return fromStart
                .Where(x => x.Key.Point == this.Finish)
                .Min(x => x.Value);
        }

        private HashSet<Vector<int>> FindBestSeatTiles(int bestScore)
        {
            Dictionary<PathState, int> fromStart = this.CalculateDistancesFrom(new(this.Reindeer, Cardinal.East), reverse: false);
            Dictionary<PathState, int> toEnd = this.CalculateReverseDistancesFromEnd();
            HashSet<Vector<int>> seats = [];

            foreach (KeyValuePair<PathState, int> item in fromStart)
            {
                if (!toEnd.TryGetValue(item.Key, out int endScore))
                {
                    continue;
                }

                if (item.Value + endScore == bestScore)
                {
                    seats.Add(item.Key.Point);
                }
            }

            return seats;
        }

        private Dictionary<PathState, int> CalculateDistancesFrom(PathState start, bool reverse)
        {
            PriorityQueue<PathState, int> queue = new();
            Dictionary<PathState, int> distance = [];

            queue.Enqueue(start, 0);
            distance[start] = 0;

            while (queue.Count > 0)
            {
                PathState current = queue.Dequeue();
                int currentScore = distance[current];

                IEnumerable<(PathState Next, int Cost)> neighbours = reverse
                    ? this.GetReverseRenderNeighbours(current)
                    : this.GetRenderNeighbours(current);

                foreach ((PathState Next, int Cost) in neighbours)
                {
                    int nextScore = currentScore + Cost;

                    if (!distance.TryGetValue(Next, out int existingScore) || nextScore < existingScore)
                    {
                        distance[Next] = nextScore;
                        queue.Enqueue(Next, nextScore);
                    }
                }
            }

            return distance;
        }

        private Dictionary<PathState, int> CalculateReverseDistancesFromEnd()
        {
            PriorityQueue<PathState, int> queue = new();
            Dictionary<PathState, int> distance = [];

            foreach (Cardinal direction in Enum.GetValues<Cardinal>())
            {
                PathState end = new(this.Finish, direction);
                queue.Enqueue(end, 0);
                distance[end] = 0;
            }

            while (queue.Count > 0)
            {
                PathState current = queue.Dequeue();
                int currentScore = distance[current];

                foreach ((PathState Next, int Cost) in this.GetReverseRenderNeighbours(current))
                {
                    int nextScore = currentScore + Cost;

                    if (!distance.TryGetValue(Next, out int existingScore) || nextScore < existingScore)
                    {
                        distance[Next] = nextScore;
                        queue.Enqueue(Next, nextScore);
                    }
                }
            }

            return distance;
        }

        private static Vector<int> Move(Vector<int> point, Cardinal direction)
            => CardinalHelper.Transform(point, direction);

        private IEnumerable<(PathState Next, int Cost)> GetRenderNeighbours(PathState state)
        {
            Vector<int> forward = Move(state.Point, state.Direction);
            char forwardValue = this.Map[forward.Y, forward.X];

            if (forwardValue != '#')
            {
                yield return (new PathState(forward, state.Direction), 1);
            }

            yield return (new PathState(state.Point, CardinalHelper.Clockwise[state.Direction]), 1000);
            yield return (new PathState(state.Point, CardinalHelper.AntiClockwise[state.Direction]), 1000);
        }

        private IEnumerable<(PathState Next, int Cost)> GetReverseRenderNeighbours(PathState state)
        {
            Vector<int> previous = Move(state.Point, CardinalHelper.Flip(state.Direction));
            char previousValue = this.Map[previous.Y, previous.X];

            if (previousValue != '#')
            {
                yield return (new PathState(previous, state.Direction), 1);
            }

            yield return (new PathState(state.Point, CardinalHelper.Clockwise[state.Direction]), 1000);
            yield return (new PathState(state.Point, CardinalHelper.AntiClockwise[state.Direction]), 1000);
        }

        private List<RenderNode> ReconstructRenderPath(
            Dictionary<PathState, PathState> cameFrom,
            Dictionary<PathState, int> distance,
            PathState target)
        {
            List<RenderNode> path = [];
            PathState current = target;

            path.Add(new(current.Point, current.Direction, distance[current]));

            while (cameFrom.ContainsKey(current))
            {
                current = cameFrom[current];
                path.Add(new(current.Point, current.Direction, distance[current]));
            }

            path.Reverse();

            return path;
        }

        private string[] BuildFrame(
            string title,
            Vector<int>? current,
            Cardinal direction,
            HashSet<Vector<int>> trail,
            HashSet<Vector<int>> seats,
            bool showSeats)
        {
            List<string> result = [];

            result.Add(title);
            result.Add(string.Empty);

            for (int y = 0; y < this.Map.Height; y++)
            {
                StringBuilder sb = new();

                for (int x = 0; x < this.Map.Width; x++)
                {
                    Vector<int> point = new(x, y);
                    char value = this.Map[y, x];

                    if (current != null && point == current)
                    {
                        sb.Append(GetDirectionCharacter(direction));
                    }
                    else if (showSeats && seats.Contains(point))
                    {
                        sb.Append('O');
                    }
                    else if (!showSeats && trail.Contains(point))
                    {
                        sb.Append('~');
                    }
                    else
                    {
                        sb.Append(value);
                    }
                }

                result.Add(sb.ToString());
            }

            return [.. result];
        }

        private static char GetDirectionCharacter(Cardinal direction)
        {
            return direction switch
            {
                Cardinal.North => '^',
                Cardinal.East => '>',
                Cardinal.South => 'v',
                Cardinal.West => '<',
                _ => '@'
            };
        }

        private void RenderPaddedFrames(List<string[]> frames)
        {
            int width = frames.SelectMany(frame => frame).Max(row => row.Length);
            int height = frames.Max(frame => frame.Length);

            foreach (string[] frame in frames)
            {
                this.Renderer?.RenderFrame(
                    new Frame(PadFrame(frame, width, height)));
            }
        }

        private static string[] PadFrame(string[] frame, int width, int height)
        {
            List<string> result = [];

            foreach (string row in frame)
            {
                result.Add(row.PadRight(width, ' '));
            }

            while (result.Count < height)
            {
                result.Add(new string(' ', width));
            }

            return [.. result];
        }
    }
}
