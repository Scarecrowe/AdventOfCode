namespace AdventOfCode.Puzzles._2025.Day_12___Christmas_Tree_Farm
{
    using AdventOfCode.Animation.Renderers;
    using AdventOfCode.Core;
    using System.Text;

    public sealed class ChristmasTreeFarm
    {
        private readonly string[] input;

        public ChristmasTreeFarm(string[] input)
        {
            this.input = input;
        }

        public ChristmasTreeFarm(string[] input, IFrameRenderer renderer)
            : this(input)
        {
            this.Renderer = renderer;
        }

        public IFrameRenderer? Renderer { get; }

        private sealed record PresentShape(int Index, string[] Rows, int Area);

        private sealed record RegionSpec(
            int Index,
            int Width,
            int Height,
            int[] Counts,
            long RequiredArea,
            long BoardArea,
            bool Fits);

        public long RegionCount()
        {
            int[] presentAreas = ParsePresentAreas(input);

            long answer = 0;

            foreach (string rawLine in input)
            {
                if (string.IsNullOrWhiteSpace(rawLine))
                {
                    continue;
                }

                string line = rawLine.Trim();

                if (line.EndsWith(":", StringComparison.Ordinal))
                {
                    continue;
                }

                if (IsShapeRow(line))
                {
                    continue;
                }

                int colonIndex = line.IndexOf(':');

                if (colonIndex < 0)
                {
                    continue;
                }

                ReadOnlySpan<char> dimsSpan = line.AsSpan(0, colonIndex).Trim();
                ReadOnlySpan<char> countsSpan = line.AsSpan(colonIndex + 1).Trim();

                int xIndex = dimsSpan.IndexOf('x');

                if (xIndex < 0)
                {
                    continue;
                }

                int width = ParseInt(dimsSpan.Slice(0, xIndex));
                int height = ParseInt(dimsSpan.Slice(xIndex + 1));

                long boardArea = (long)width * height;
                long requiredArea = 0;

                int presentIndex = 0;
                int current = 0;
                bool inNumber = false;

                for (int i = 0; i < countsSpan.Length; i++)
                {
                    char c = countsSpan[i];

                    if (c >= '0' && c <= '9')
                    {
                        current = (current * 10) + (c - '0');
                        inNumber = true;
                    }
                    else if (inNumber)
                    {
                        if (presentIndex < presentAreas.Length)
                        {
                            requiredArea += (long)current * presentAreas[presentIndex];
                        }

                        presentIndex++;
                        current = 0;
                        inNumber = false;
                    }
                }

                if (inNumber && presentIndex < presentAreas.Length)
                {
                    requiredArea += (long)current * presentAreas[presentIndex];
                }

                if (requiredArea <= boardArea)
                {
                    answer++;
                }
            }

            return answer;
        }

        public ChristmasTreeFarm RenderSilver(int renderEvery = 1)
        {
            return this.RenderRegions("CHRISTMAS TREE FARM // AREA CHECK", renderEvery);
        }

        public ChristmasTreeFarm RenderGold(int renderEvery = 1)
        {
            return this.RenderRegions("CHRISTMAS TREE FARM // ALL REGIONS CHECKED", renderEvery);
        }

        private ChristmasTreeFarm RenderRegions(string title, int renderEvery)
        {
            if (this.Renderer == null)
            {
                return this;
            }

            List<PresentShape> shapes = ParsePresentShapes(this.input);
            List<RegionSpec> regions = ParseRegions(this.input, shapes.Select(s => s.Area).ToArray());
            List<string[]> frames = [];

            long fits = 0;

            frames.Add(this.BuildIntroFrame(title, shapes, regions.Count));

            for (int i = 0; i < regions.Count; i++)
            {
                RegionSpec region = regions[i];

                int fillFrames = region.Fits ? 7 : 9;

                for (int step = 0; step <= fillFrames; step++)
                {
                    if (step % renderEvery != 0 && step != fillFrames)
                    {
                        continue;
                    }

                    double progress = fillFrames == 0 ? 1 : (double)step / fillFrames;

                    frames.Add(this.BuildRegionFrame(
                        title,
                        region,
                        shapes,
                        fits,
                        progress,
                        complete: false));
                }

                if (region.Fits)
                {
                    fits++;
                }

                for (int hold = 0; hold < 3; hold++)
                {
                    frames.Add(this.BuildRegionFrame(
                        title,
                        region,
                        shapes,
                        fits,
                        progress: 1,
                        complete: true));
                }
            }

            for (int hold = 0; hold < 16; hold++)
            {
                frames.Add(this.BuildSummaryFrame(title, regions.Count, fits));
            }

            this.RenderPaddedFrames(frames);

            return this;
        }

        private string[] BuildIntroFrame(string title, List<PresentShape> shapes, int regionCount)
        {
            List<string> result = [];

            result.Add(title);
            result.Add(string.Empty);
            result.Add($"PRESENT SHAPES: {shapes.Count:000}");
            result.Add($"REGIONS:        {regionCount:0000}");
            result.Add(string.Empty);
            result.Add("EACH REGION IS TESTED BY THE SAME AREA RULE USED BY THE PUZZLE IMPLEMENTATION.");
            result.Add("THE BOARD PREVIEW IS SCALED, SO VERY WIDE REGIONS DO NOT CREATE TINY VIDEOS.");
            result.Add(string.Empty);

            foreach (PresentShape shape in shapes.Take(8))
            {
                result.Add($"SHAPE {shape.Index:00} // AREA {shape.Area:000}");

                foreach (string row in shape.Rows)
                {
                    result.Add($"  {row}");
                }

                result.Add(string.Empty);
            }

            return [.. result];
        }

        private string[] BuildRegionFrame(
            string title,
            RegionSpec region,
            List<PresentShape> shapes,
            long fitsSoFar,
            double progress,
            bool complete)
        {
            List<string> result = [];
            long animatedRequired = (long)Math.Round(region.RequiredArea * Math.Clamp(progress, 0, 1));
            bool overflowVisible = !region.Fits && progress >= 1;

            result.Add($"{title} // REGION {region.Index + 1:0000}");
            result.Add(string.Empty);
            result.Add($"SIZE {region.Width}x{region.Height} // BOARD AREA {region.BoardArea:000000} // REQUIRED AREA {animatedRequired:000000}/{region.RequiredArea:000000}");
            result.Add($"FITS SO FAR {fitsSoFar:0000} // STATUS {(complete ? (region.Fits ? "FIT" : "NO FIT") : "CHECKING")}");
            result.Add(string.Empty);

            result.AddRange(BuildScaledBoard(region, animatedRequired, overflowVisible));
            result.Add(string.Empty);
            result.Add(BuildAreaBar(region, animatedRequired));
            result.Add(string.Empty);
            result.Add("PRESENT COUNTS");

            foreach (string line in BuildCounts(region, shapes))
            {
                result.Add(line);
            }

            return [.. result];
        }

        private string[] BuildSummaryFrame(string title, int total, long fits)
        {
            List<string> result = [];

            result.Add(title);
            result.Add(string.Empty);
            result.Add("ALL REGIONS CHECKED");
            result.Add(string.Empty);
            result.Add($"REGIONS THAT FIT: {fits:0000}");
            result.Add($"TOTAL REGIONS:    {total:0000}");
            result.Add(string.Empty);
            result.Add($"ANSWER: {fits}");

            return [.. result];
        }

        private static IEnumerable<string> BuildScaledBoard(RegionSpec region, long animatedRequired, bool overflowVisible)
        {
            const int maxPreviewWidth = 72;
            const int maxPreviewHeight = 18;

            int previewWidth = Math.Min(region.Width, maxPreviewWidth);
            int previewHeight = Math.Min(region.Height, maxPreviewHeight);
            long visibleCells = (long)previewWidth * previewHeight;
            long boardArea = Math.Max(1, region.BoardArea);
            long requiredOnPreview = Math.Min(visibleCells, (animatedRequired * visibleCells) / boardArea);

            if (animatedRequired > 0 && requiredOnPreview == 0)
            {
                requiredOnPreview = 1;
            }

            long cell = 0;

            for (int y = 0; y < previewHeight; y++)
            {
                StringBuilder row = new();

                for (int x = 0; x < previewWidth; x++)
                {
                    row.Append(cell < requiredOnPreview ? '#' : '.');
                    cell++;
                }

                if (region.Width > previewWidth)
                {
                    row.Append('>');
                }

                yield return row.ToString();
            }

            if (region.Height > previewHeight)
            {
                yield return new string('v', previewWidth);
            }

            if (overflowVisible)
            {
                yield return $"OVERFLOW +{region.RequiredArea - region.BoardArea:000000} CELLS";
            }
        }

        private static string BuildAreaBar(RegionSpec region, long animatedRequired)
        {
            const int width = 60;

            long boardArea = Math.Max(1, region.BoardArea);
            int filled = (int)Math.Clamp((animatedRequired * width) / boardArea, 0, width);
            int overflow = region.RequiredArea > region.BoardArea && animatedRequired >= region.RequiredArea
                ? Math.Min(width, (int)Math.Clamp(((region.RequiredArea - region.BoardArea) * width) / boardArea, 1, width))
                : 0;

            StringBuilder bar = new();
            bar.Append('[');

            for (int i = 0; i < width; i++)
            {
                if (i < filled)
                {
                    bar.Append('#');
                }
                else
                {
                    bar.Append('.');
                }
            }

            bar.Append(']');

            if (overflow > 0)
            {
                bar.Append(' ');
                bar.Append(new string('+', overflow));
            }

            return bar.ToString();
        }

        private static IEnumerable<string> BuildCounts(RegionSpec region, List<PresentShape> shapes)
        {
            for (int i = 0; i < region.Counts.Length && i < shapes.Count; i++)
            {
                if (region.Counts[i] == 0)
                {
                    continue;
                }

                int area = shapes[i].Area;
                long contribution = (long)area * region.Counts[i];

                yield return $"  SHAPE {i:00} x {region.Counts[i]:000} // AREA {area:000} EACH // TOTAL {contribution:000000}";
            }
        }

        private static List<PresentShape> ParsePresentShapes(string[] input)
        {
            List<PresentShape> shapes = [];

            for (int i = 0; i < input.Length; i++)
            {
                string line = input[i].Trim();

                if (line.Length == 0 || !line.EndsWith(":", StringComparison.Ordinal))
                {
                    continue;
                }

                bool numericId = true;

                for (int j = 0; j < line.Length - 1; j++)
                {
                    if (line[j] < '0' || line[j] > '9')
                    {
                        numericId = false;
                        break;
                    }
                }

                if (!numericId)
                {
                    continue;
                }

                int index = ParseInt(line.AsSpan(0, line.Length - 1));
                List<string> rows = [];
                int area = 0;

                for (int r = i + 1; r < input.Length; r++)
                {
                    string shapeLine = input[r].Trim();

                    if (shapeLine.Length == 0 || !IsShapeRow(shapeLine))
                    {
                        break;
                    }

                    rows.Add(shapeLine);
                    area += shapeLine.Count(c => c == '#');
                }

                shapes.Add(new PresentShape(index, [.. rows], area));
            }

            return shapes.OrderBy(s => s.Index).ToList();
        }

        private static List<RegionSpec> ParseRegions(string[] input, int[] presentAreas)
        {
            List<RegionSpec> regions = [];

            foreach (string rawLine in input)
            {
                if (string.IsNullOrWhiteSpace(rawLine))
                {
                    continue;
                }

                string line = rawLine.Trim();

                if (line.EndsWith(":", StringComparison.Ordinal) || IsShapeRow(line))
                {
                    continue;
                }

                int colonIndex = line.IndexOf(':');

                if (colonIndex < 0)
                {
                    continue;
                }

                ReadOnlySpan<char> dimsSpan = line.AsSpan(0, colonIndex).Trim();
                ReadOnlySpan<char> countsSpan = line.AsSpan(colonIndex + 1).Trim();
                int xIndex = dimsSpan.IndexOf('x');

                if (xIndex < 0)
                {
                    continue;
                }

                int width = ParseInt(dimsSpan.Slice(0, xIndex));
                int height = ParseInt(dimsSpan.Slice(xIndex + 1));
                int[] counts = ParseCounts(countsSpan);
                long boardArea = (long)width * height;
                long requiredArea = 0;

                for (int i = 0; i < counts.Length && i < presentAreas.Length; i++)
                {
                    requiredArea += (long)counts[i] * presentAreas[i];
                }

                regions.Add(new RegionSpec(
                    regions.Count,
                    width,
                    height,
                    counts,
                    requiredArea,
                    boardArea,
                    requiredArea <= boardArea));
            }

            return regions;
        }

        private static int[] ParseCounts(ReadOnlySpan<char> countsSpan)
        {
            List<int> counts = [];
            int current = 0;
            bool inNumber = false;

            for (int i = 0; i < countsSpan.Length; i++)
            {
                char c = countsSpan[i];

                if (c >= '0' && c <= '9')
                {
                    current = (current * 10) + (c - '0');
                    inNumber = true;
                }
                else if (inNumber)
                {
                    counts.Add(current);
                    current = 0;
                    inNumber = false;
                }
            }

            if (inNumber)
            {
                counts.Add(current);
            }

            return [.. counts];
        }

        private static int[] ParsePresentAreas(string[] input)
        {
            List<int> areas = new(6);

            for (int i = 0; i < input.Length; i++)
            {
                string line = input[i].Trim();

                if (line.Length == 0)
                {
                    continue;
                }

                if (!line.EndsWith(":", StringComparison.Ordinal))
                {
                    continue;
                }

                bool numericId = true;

                for (int j = 0; j < line.Length - 1; j++)
                {
                    if (line[j] < '0' || line[j] > '9')
                    {
                        numericId = false;
                        break;
                    }
                }

                if (!numericId)
                {
                    continue;
                }

                int area = 0;

                for (int r = i + 1; r < input.Length; r++)
                {
                    string shapeLine = input[r].Trim();

                    if (shapeLine.Length == 0)
                    {
                        break;
                    }

                    if (!IsShapeRow(shapeLine))
                    {
                        break;
                    }

                    for (int c = 0; c < shapeLine.Length; c++)
                    {
                        if (shapeLine[c] == '#')
                        {
                            area++;
                        }
                    }
                }

                areas.Add(area);
            }

            return areas.ToArray();
        }

        private static bool IsShapeRow(string line)
        {
            if (line.Length == 0)
            {
                return false;
            }

            for (int i = 0; i < line.Length; i++)
            {
                char c = line[i];

                if (c != '#' && c != '.')
                {
                    return false;
                }
            }

            return true;
        }

        private static int ParseInt(ReadOnlySpan<char> span)
        {
            int value = 0;

            for (int i = 0; i < span.Length; i++)
            {
                char c = span[i];

                if (c >= '0' && c <= '9')
                {
                    value = (value * 10) + (c - '0');
                }
            }

            return value;
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
