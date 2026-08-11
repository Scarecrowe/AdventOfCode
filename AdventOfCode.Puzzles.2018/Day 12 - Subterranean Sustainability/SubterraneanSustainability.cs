namespace AdventOfCode.Puzzles._2018.Day_12___Subterranean_Sustainability
{
    using AdventOfCode.Animation.Renderers;
    using AdventOfCode.Core;
    using AdventOfCode.Core.Extensions;
    using System.Text;

    public class SubterraneanSustainability
    {
        private const int SilverGenerations = 20;
        private const long GoldGenerations = 50000000000;
        private const int ViewportWidth = 121;
        private const int GoldHoldFrames = 24;

        public SubterraneanSustainability(string[] input)
        {
            this.Rules = new();
            string state = input[0].Replace("initial state: ");

            this.IntialState = new NegativeIndexArray<bool>(state.Length + 4, -2);

            for (int i = 0; i < state.Length; i++)
            {
                this.IntialState[i] = state[i] == '#';
            }

            for (int i = 2; i < input.Length; i++)
            {
                string[] tokens = input[i].Split(" => ");
                this.Rules.Add(tokens[0], tokens[1][0] == '#');
            }
        }

        public SubterraneanSustainability(string[] input, IFrameRenderer renderer)
            : this(input)
        {
            this.Renderer = renderer;
        }

        public IFrameRenderer? Renderer { get; }

        public NegativeIndexArray<bool> IntialState { get; private set; }

        public Dictionary<string, bool> Rules { get; }

        private sealed record PlantFrame(
            long Generation,
            long Sum,
            long Left,
            long Right,
            long Offset,
            string State,
            bool Stable,
            long ProjectedGenerations = 0,
            long ProjectedShift = 0);

        public long Grow(long generations)
        {
            int min = this.IntialState.Min;
            int max = this.IntialState.Max + 1;

            NegativeIndexArray<bool> last = this.IntialState;
            HashSet<string> states = new();

            for (int i = 1; i <= generations; i++)
            {
                NegativeIndexArray<bool> current = new(max, min);

                for (int j = min; j <= max; j++)
                {
                    string rule = PotToRule(j, ref last);

                    if (this.Rules.ContainsKey(rule))
                    {
                        if (this.Rules[rule])
                        {
                            current[j] = this.Rules[rule];
                        }
                    }
                    else
                    {
                        current[j] = true;
                    }
                }

                string value = current.Values.Select(x => x ? '#' : '.').Join().Strip('.');

                if (!states.Contains(value))
                {
                    states.Add(value);
                }
                else
                {
                    return SumOfPlants(ref current, generations, i);
                }

                if (current[current.Length - (min * -1) - 1])
                {
                    max += 2;
                }

                if (current[min])
                {
                    min -= 2;
                }

                last = current;
            }

            return SumOfPlants(ref last, 0, 0);
        }

        public SubterraneanSustainability RenderSilver(int renderEvery = 1)
        {
            return this.RenderGrowth(SilverGenerations, renderEvery, "SILVER // 20 GENERATIONS");
        }

        public SubterraneanSustainability RenderGold(int renderEvery = 1)
        {
            return this.RenderGrowth(GoldGenerations, renderEvery, "GOLD // 50 BILLION GENERATIONS");
        }

        private static string PotToRule(int index, ref NegativeIndexArray<bool> state)
        {
            char ll = !state.HasIndex(index - 2) ? '.' : state[index - 2] ? '#' : '.';
            char l = !state.HasIndex(index - 1) ? '.' : state[index - 1] ? '#' : '.';
            char c = !state.HasIndex(index) ? '.' : state[index] ? '#' : '.';
            char r = !state.HasIndex(index + 1) ? '.' : state[index + 1] ? '#' : '.';
            char rr = !state.HasIndex(index + 2) ? '.' : state[index + 2] ? '#' : '.';

            return $"{ll}{l}{c}{r}{rr}";
        }

        private static long SumOfPlants(ref NegativeIndexArray<bool> state, long generations, long stable)
        {
            long result = 0;

            for (int j = state.Min; j < state.Length; j++)
            {
                if (state.HasIndex(j) && state[j])
                {
                    result += j + (generations - stable);
                }
            }

            return result;
        }

        private SubterraneanSustainability RenderGrowth(long generations, int renderEvery, string title)
        {
            if (this.Renderer == null)
            {
                return this;
            }

            HashSet<long> plants = this.GetInitialPlants();
            Dictionary<string, (long Generation, long Left, long Sum)> seen = new();

            PlantFrame first = this.CreatePlantFrame(0, plants, false);
            this.Renderer.RenderFrame(new Frame(this.BuildFrame(first, title)));

            for (long generation = 1; generation <= generations; generation++)
            {
                plants = this.NextGeneration(plants);

                long left = plants.Count == 0 ? 0 : plants.Min();
                long sum = plants.Sum();
                string shape = this.ShapeKey(plants);

                if (seen.TryGetValue(shape, out (long Generation, long Left, long Sum) previous))
                {
                    long generationDelta = generation - previous.Generation;
                    long shiftDelta = left - previous.Left;
                    long remaining = generations - generation;
                    long cycles = remaining / generationDelta;
                    long remainder = remaining % generationDelta;

                    PlantFrame stable = this.CreatePlantFrame(
                        generation,
                        plants,
                        true,
                        generations,
                        cycles * shiftDelta);

                    this.Renderer.RenderFrame(new Frame(this.BuildFrame(stable, title)));

                    if (cycles > 0)
                    {
                        plants = plants.Select(x => x + (cycles * shiftDelta)).ToHashSet();
                        generation += cycles * generationDelta;
                    }

                    for (long i = 0; i < remainder; i++)
                    {
                        plants = this.NextGeneration(plants);
                        generation++;
                    }

                    PlantFrame projected = this.CreatePlantFrame(
                        generations,
                        plants,
                        true,
                        generations,
                        cycles * shiftDelta);

                    this.Renderer.RenderFrame(new Frame(this.BuildFrame(projected, title)));

                    for (int i = 0; i < GoldHoldFrames; i++)
                    {
                        this.Renderer.RenderFrame(new Frame(this.BuildFrame(projected, title)));
                    }

                    return this;
                }

                seen[shape] = (generation, left, sum);

                if (generation % renderEvery == 0 || generation == generations)
                {
                    PlantFrame frame = this.CreatePlantFrame(generation, plants, false);
                    this.Renderer.RenderFrame(new Frame(this.BuildFrame(frame, title)));
                }
            }

            PlantFrame last = this.CreatePlantFrame(generations, plants, false);

            for (int i = 0; i < GoldHoldFrames; i++)
            {
                this.Renderer.RenderFrame(new Frame(this.BuildFrame(last, title)));
            }

            return this;
        }

        private List<PlantFrame> BuildRenderFrames(long targetGeneration, int renderEvery)
        {
            List<PlantFrame> frames = [];
            HashSet<long> plants = this.GetInitialPlants();
            Dictionary<string, (long Generation, long Left, long Sum)> seen = new();

            frames.Add(this.CreatePlantFrame(0, plants, false));

            for (long generation = 1; generation <= targetGeneration; generation++)
            {
                plants = this.NextGeneration(plants);

                long left = plants.Count == 0 ? 0 : plants.Min();
                long sum = plants.Sum();
                string shape = this.ShapeKey(plants);

                if (seen.TryGetValue(shape, out (long Generation, long Left, long Sum) previous))
                {
                    long generationDelta = generation - previous.Generation;
                    long shiftDelta = left - previous.Left;
                    long sumDelta = sum - previous.Sum;
                    long remaining = targetGeneration - generation;
                    long cycles = remaining / generationDelta;
                    long remainder = remaining % generationDelta;

                    PlantFrame stable = this.CreatePlantFrame(
                        generation,
                        plants,
                        true,
                        targetGeneration,
                        cycles * shiftDelta);

                    if (frames.Count == 0 || frames[^1].Generation != stable.Generation)
                    {
                        frames.Add(stable);
                    }

                    if (cycles > 0)
                    {
                        plants = plants.Select(x => x + (cycles * shiftDelta)).ToHashSet();
                        generation += cycles * generationDelta;
                        sum += cycles * sumDelta;
                    }

                    for (long i = 0; i < remainder; i++)
                    {
                        plants = this.NextGeneration(plants);
                        generation++;
                    }

                    PlantFrame projected = this.CreatePlantFrame(
                        targetGeneration,
                        plants,
                        true,
                        targetGeneration,
                        cycles * shiftDelta);

                    if (frames[^1].Generation != projected.Generation)
                    {
                        frames.Add(projected);
                    }

                    return frames;
                }

                seen[shape] = (generation, left, sum);

                if (generation % renderEvery == 0 || generation == targetGeneration)
                {
                    frames.Add(this.CreatePlantFrame(generation, plants, false));
                }
            }

            return frames;
        }

        private HashSet<long> GetInitialPlants()
        {
            HashSet<long> result = [];

            for (int i = this.IntialState.Min; i < this.IntialState.Length; i++)
            {
                if (this.IntialState.HasIndex(i) && this.IntialState[i])
                {
                    result.Add(i);
                }
            }

            return result;
        }

        private HashSet<long> NextGeneration(HashSet<long> plants)
        {
            HashSet<long> result = [];

            if (plants.Count == 0)
            {
                return result;
            }

            long min = plants.Min() - 2;
            long max = plants.Max() + 2;

            for (long pot = min; pot <= max; pot++)
            {
                string rule = this.PotToRule(pot, plants);

                if (this.Rules.TryGetValue(rule, out bool grows) && grows)
                {
                    result.Add(pot);
                }
            }

            return result;
        }

        private string PotToRule(long index, HashSet<long> plants)
        {
            StringBuilder result = new();

            for (long i = index - 2; i <= index + 2; i++)
            {
                result.Append(plants.Contains(i) ? '#' : '.');
            }

            return result.ToString();
        }

        private string ShapeKey(HashSet<long> plants)
        {
            if (plants.Count == 0)
            {
                return string.Empty;
            }

            long left = plants.Min();
            long right = plants.Max();
            StringBuilder result = new();

            for (long i = left; i <= right; i++)
            {
                result.Append(plants.Contains(i) ? '#' : '.');
            }

            return result.ToString().Trim('.');
        }

        private PlantFrame CreatePlantFrame(
            long generation,
            HashSet<long> plants,
            bool stable,
            long projectedGenerations = 0,
            long projectedShift = 0)
        {
            long left = plants.Count == 0 ? 0 : plants.Min();
            long right = plants.Count == 0 ? 0 : plants.Max();
            long center = plants.Count == 0 ? 0 : (left + right) / 2;
            long offset = center - (ViewportWidth / 2);
            long sum = plants.Sum();
            StringBuilder state = new();

            for (long i = offset; i < offset + ViewportWidth; i++)
            {
                state.Append(plants.Contains(i) ? '#' : '.');
            }

            return new(
                generation,
                sum,
                left,
                right,
                offset,
                state.ToString(),
                stable,
                projectedGenerations,
                projectedShift);
        }

        private string[] BuildFrame(PlantFrame frame, string title)
        {
            List<string> result = [];
            long zeroColumn = -frame.Offset;

            result.Add(title);
            result.Add($"GENERATION {frame.Generation:000000000000} // SUM {frame.Sum:0000000000000} // WINDOW {frame.Offset}..{frame.Offset + ViewportWidth - 1}");

            if (frame.Stable)
            {
                result.Add($"STABLE SHAPE DETECTED // PROJECTING TO {frame.ProjectedGenerations:000000000000} // SHIFT +{frame.ProjectedShift}");
            }
            else
            {
                result.Add($"PLANTS {frame.Left}..{frame.Right} // SCROLLING VIEWPORT");
            }

            result.Add(string.Empty);
            result.Add(this.BuildRuler(frame.Offset));
            result.Add(this.BuildZeroMarker(zeroColumn));
            result.Add(frame.State);
            result.Add(this.BuildPlantGlow(frame.State));
            result.Add(string.Empty);
            result.Add("# PLANT  . EMPTY  | POT ZERO  ^ ACTIVE SPREAD");

            return [.. result];
        }

        private string BuildRuler(long offset)
        {
            StringBuilder result = new();

            for (long pot = offset; pot < offset + ViewportWidth; pot++)
            {
                if (pot == 0)
                {
                    result.Append('|');
                }
                else if (pot % 10 == 0)
                {
                    result.Append('+');
                }
                else if (pot % 5 == 0)
                {
                    result.Append(':');
                }
                else
                {
                    result.Append('-');
                }
            }

            return result.ToString();
        }

        private string BuildZeroMarker(long zeroColumn)
        {
            StringBuilder result = new();

            for (int i = 0; i < ViewportWidth; i++)
            {
                result.Append(i == zeroColumn ? '0' : ' ');
            }

            return result.ToString();
        }

        private string BuildPlantGlow(string state)
        {
            StringBuilder result = new();

            for (int i = 0; i < state.Length; i++)
            {
                bool left = i > 0 && state[i - 1] == '#';
                bool current = state[i] == '#';
                bool right = i < state.Length - 1 && state[i + 1] == '#';

                result.Append(current || left || right ? '^' : ' ');
            }

            return result.ToString();
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
