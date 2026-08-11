namespace AdventOfCode.Puzzles._2023.Day_12___Hot_Springs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using AdventOfCode.Animation.Renderers;
    using AdventOfCode.Core;
    using AdventOfCode.Core.Extensions;

    public class HotSprings
    {
        private readonly string[] input;

        public HotSprings(string[] input)
        {
            this.input = input;
        }

        public HotSprings(string[] input, IFrameRenderer renderer)
            : this(input)
        {
            this.Renderer = renderer;
        }

        public IFrameRenderer? Renderer { get; }

        private readonly record struct RenderState(
            int Index,
            int GroupIndex,
            int RunLength,
            long Count,
            int Depth,
            char Choice);

        public long Arrangements()
        {
            long result = 0;

            foreach (string line in this.input)
            {
                (string pattern, int[] groups) = this.Parse(line, false);
                result += this.CountArrangements(pattern, groups);
            }

            return result;
        }

        public long UnfoldedArrangements()
        {
            long result = 0;

            foreach (string line in this.input)
            {
                (string pattern, int[] groups) = this.Parse(line, true);
                result += this.CountArrangements(pattern, groups);
            }

            return result;
        }

        public HotSprings RenderSilver(int renderEvery = 1, int maxRows = 24)
        {
            return this.Render(false, renderEvery, maxRows);
        }

        public HotSprings RenderGold(int renderEvery = 8, int maxRows = 16)
        {
            return this.Render(true, renderEvery, maxRows);
        }

        private (string Pattern, int[] Groups) Parse(string line, bool unfold)
        {
            string[] tokens = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            string pattern = tokens[0];
            int[] groups = tokens[1].Split(',').ToInt();

            if (!unfold)
            {
                return (pattern, groups);
            }

            string expandedPattern = string.Join("?", Enumerable.Repeat(pattern, 5));
            int[] expandedGroups = Enumerable.Range(0, 5)
                .SelectMany(_ => groups)
                .ToArray();

            return (expandedPattern, expandedGroups);
        }

        private long CountArrangements(string pattern, int[] groups)
        {
            Dictionary<(int Index, int GroupIndex, int RunLength), long> cache = new();

            return this.Count(pattern, groups, 0, 0, 0, cache);
        }

        private long Count(
            string pattern,
            int[] groups,
            int index,
            int groupIndex,
            int runLength,
            Dictionary<(int Index, int GroupIndex, int RunLength), long> cache)
        {
            (int Index, int GroupIndex, int RunLength) key = (index, groupIndex, runLength);

            if (cache.TryGetValue(key, out long cached))
            {
                return cached;
            }

            if (index == pattern.Length)
            {
                if (runLength > 0)
                {
                    if (groupIndex >= groups.Length || runLength != groups[groupIndex])
                    {
                        return cache[key] = 0;
                    }

                    groupIndex++;
                }

                return cache[key] = groupIndex == groups.Length ? 1 : 0;
            }

            long result = 0;
            char current = pattern[index];

            if (current == '.' || current == '?')
            {
                if (runLength == 0)
                {
                    result += this.Count(pattern, groups, index + 1, groupIndex, 0, cache);
                }
                else if (groupIndex < groups.Length && runLength == groups[groupIndex])
                {
                    result += this.Count(pattern, groups, index + 1, groupIndex + 1, 0, cache);
                }
            }

            if (current == '#' || current == '?')
            {
                if (groupIndex < groups.Length && runLength < groups[groupIndex])
                {
                    result += this.Count(pattern, groups, index + 1, groupIndex, runLength + 1, cache);
                }
            }

            cache[key] = result;
            return result;
        }

        private HotSprings Render(bool unfold, int renderEvery, int maxRows)
        {
            if (this.Renderer == null)
            {
                return this;
            }

            List<(string Original, string Pattern, int[] Groups, long Count)> rows = this.input
                .Select(line =>
                {
                    (string pattern, int[] groups) = this.Parse(line, unfold);
                    long count = this.CountArrangements(pattern, groups);
                    return (line, pattern, groups, count);
                })
                .ToList();

            long total = rows.Sum(row => row.Count);
            int visibleRows = Math.Min(maxRows, rows.Count);
            int start = 0;
            int frame = 0;
            int width = this.FrameWidth(rows, visibleRows, unfold);
            int height = visibleRows + 10;

            foreach ((string Original, string Pattern, int[] Groups, long Count) row in rows)
            {
                List<RenderState> states = this.Trace(row.Pattern, row.Groups, renderEvery).ToList();

                if (row.Pattern.Length > width - 4)
                {
                    foreach (string[] scrollingFrame in this.RenderScrollingRow(row.Pattern, row.Groups, row.Count, total, unfold, frame, width, height))
                    {
                        this.Renderer.RenderFrame(new Frame(scrollingFrame));
                        frame++;
                    }
                }

                foreach (RenderState state in states)
                {
                    if (frame % renderEvery == 0 || state.Index == row.Pattern.Length)
                    {
                        if (frame > 0 && frame % Math.Max(1, visibleRows - 3) == 0)
                        {
                            start = Math.Min(Math.Max(0, rows.Count - visibleRows), start + 1);
                        }

                        string[] rendered = this.BuildFrame(
                            rows,
                            row.Original,
                            row.Pattern,
                            row.Groups,
                            state,
                            total,
                            start,
                            visibleRows,
                            unfold,
                            width,
                            height);

                        this.Renderer.RenderFrame(new Frame(rendered));
                    }

                    frame++;
                }
            }

            string[] finalFrame = this.BuildSummaryFrame(rows, total, unfold, width, height);

            for (int i = 0; i < 24; i++)
            {
                this.Renderer.RenderFrame(new Frame(finalFrame));
            }

            return this;
        }

        private IEnumerable<RenderState> Trace(string pattern, int[] groups, int renderEvery)
        {
            Dictionary<(int Index, int GroupIndex, int RunLength), long> cache = new();
            Stack<RenderState> stack = new();

            stack.Push(new(0, 0, 0, 0, 0, 'S'));

            int step = 0;

            while (stack.Count > 0)
            {
                RenderState state = stack.Pop();
                long count = this.Count(pattern, groups, state.Index, state.GroupIndex, state.RunLength, cache);

                if (step % renderEvery == 0 || state.Index == pattern.Length)
                {
                    yield return state with { Count = count };
                }

                step++;

                if (state.Index >= pattern.Length)
                {
                    continue;
                }

                char current = pattern[state.Index];

                if (current == '#' || current == '?')
                {
                    if (state.GroupIndex < groups.Length && state.RunLength < groups[state.GroupIndex])
                    {
                        stack.Push(new(
                            state.Index + 1,
                            state.GroupIndex,
                            state.RunLength + 1,
                            0,
                            state.Depth + 1,
                            '#'));
                    }
                }

                if (current == '.' || current == '?')
                {
                    if (state.RunLength == 0)
                    {
                        stack.Push(new(
                            state.Index + 1,
                            state.GroupIndex,
                            0,
                            0,
                            state.Depth + 1,
                            '.'));
                    }
                    else if (state.GroupIndex < groups.Length && state.RunLength == groups[state.GroupIndex])
                    {
                        stack.Push(new(
                            state.Index + 1,
                            state.GroupIndex + 1,
                            0,
                            0,
                            state.Depth + 1,
                            '.'));
                    }
                }
            }
        }

        private string[] BuildFrame(
            List<(string Original, string Pattern, int[] Groups, long Count)> rows,
            string original,
            string pattern,
            int[] groups,
            RenderState state,
            long total,
            int start,
            int visibleRows,
            bool unfold,
            int width,
            int height)
        {
            List<string> result = [];
            string title = unfold ? "HOT SPRINGS // UNFOLDED RECORDS" : "HOT SPRINGS // DAMAGED RECORDS";

            result.Add(title.PadRight(width));
            result.Add($"TOTAL {total} // ROW COUNT {rows.Count} // MODE {(unfold ? "GOLD" : "SILVER")}".PadRight(width));
            result.Add(string.Empty.PadRight(width));
            result.Add(this.FormatPattern(pattern, state.Index, width).PadRight(width));
            result.Add(this.FormatGroups(groups, state.GroupIndex, width).PadRight(width));
            result.Add($"INDEX {state.Index:0000} // GROUP {state.GroupIndex:000} // RUN {state.RunLength:000} // BRANCH {state.Choice} // WAYS {state.Count}".PadRight(width));
            result.Add(string.Empty.PadRight(width));

            for (int i = 0; i < visibleRows; i++)
            {
                int rowIndex = start + i;

                if (rowIndex >= rows.Count)
                {
                    result.Add(string.Empty.PadRight(width));
                    continue;
                }

                (string Original, string Pattern, int[] Groups, long Count) row = rows[rowIndex];
                bool active = row.Original == original;
                string prefix = active ? ">" : " ";
                string line = $"{prefix} {rowIndex + 1:000} {this.Compact(row.Pattern, 32),-32} {string.Join(',', row.Groups),-24} {row.Count}";

                result.Add(line[..Math.Min(line.Length, width)].PadRight(width));
            }

            result.Add(string.Empty.PadRight(width));
            result.Add("? UNKNOWN  # DAMAGED  . OPERATIONAL  | CURRENT".PadRight(width));

            return this.PadFrame(result, width, height);
        }

        private string[] BuildSummaryFrame(
            List<(string Original, string Pattern, int[] Groups, long Count)> rows,
            long total,
            bool unfold,
            int width,
            int height)
        {
            List<string> result = [];

            result.Add((unfold ? "HOT SPRINGS // GOLD COMPLETE" : "HOT SPRINGS // SILVER COMPLETE").PadRight(width));
            result.Add(string.Empty.PadRight(width));
            result.Add($"TOTAL ARRANGEMENTS {total}".PadRight(width));
            result.Add($"ROWS PROCESSED {rows.Count}".PadRight(width));
            result.Add(string.Empty.PadRight(width));

            foreach ((string Original, string Pattern, int[] Groups, long Count) row in rows.Take(height - 7))
            {
                string line = $"{this.Compact(row.Pattern, 36),-36} {string.Join(',', row.Groups),-24} {row.Count}";
                result.Add(line[..Math.Min(line.Length, width)].PadRight(width));
            }

            return this.PadFrame(result, width, height);
        }

        private IEnumerable<string[]> RenderScrollingRow(
            string pattern,
            int[] groups,
            long count,
            long total,
            bool unfold,
            int frame,
            int width,
            int height)
        {
            int window = Math.Max(16, width - 6);
            int maxOffset = Math.Max(0, pattern.Length - window);
            int step = Math.Max(1, window / 4);

            for (int offset = 0; offset <= maxOffset; offset += step)
            {
                List<string> result = [];
                string slice = pattern.Substring(offset, Math.Min(window, pattern.Length - offset));

                result.Add((unfold ? "HOT SPRINGS // UNFOLDED SCAN" : "HOT SPRINGS // RECORD SCAN").PadRight(width));
                result.Add($"TOTAL {total} // ROW WAYS {count} // FRAME {frame}".PadRight(width));
                result.Add(string.Empty.PadRight(width));
                result.Add($"[{offset:0000}-{offset + slice.Length:0000}]".PadRight(width));
                result.Add(slice.PadRight(width));
                result.Add(new string(' ', Math.Min(width, 6 + Math.Min(window - 1, slice.Length - 1))) + "^");
                result.Add(string.Empty.PadRight(width));
                result.Add(this.FormatGroups(groups, 0, width).PadRight(width));
                result.Add(string.Empty.PadRight(width));
                result.Add("SCROLLING LONG UNFOLDED RECORD TO AVOID TINY SCALED RENDER".PadRight(width));

                yield return this.PadFrame(result, width, height);
            }
        }

        private string FormatPattern(string pattern, int index, int width)
        {
            int available = Math.Max(10, width - 2);
            int start = Math.Max(0, Math.Min(index - available / 2, Math.Max(0, pattern.Length - available)));
            string visible = pattern.Substring(start, Math.Min(available, pattern.Length - start));
            StringBuilder marker = new();

            marker.Append(visible);

            if (index >= start && index < start + visible.Length)
            {
                int local = index - start;
                marker.Insert(local, '|');
            }

            return marker.ToString();
        }

        private string FormatGroups(int[] groups, int groupIndex, int width)
        {
            string[] values = groups
                .Select((group, index) => index == groupIndex ? $"[{group}]" : group.ToString())
                .ToArray();

            return this.Compact(string.Join(',', values), Math.Max(10, width));
        }

        private int FrameWidth(
            List<(string Original, string Pattern, int[] Groups, long Count)> rows,
            int visibleRows,
            bool unfold)
        {
            int longestRow = rows
                .Take(Math.Max(1, visibleRows))
                .Select(row => Math.Min(row.Pattern.Length, 96) + string.Join(',', row.Groups).Length + 24)
                .DefaultIfEmpty(80)
                .Max();

            return Math.Clamp(Math.Max(80, longestRow), 80, unfold ? 120 : 140);
        }

        private string Compact(string value, int width)
        {
            if (value.Length <= width)
            {
                return value;
            }

            if (width <= 3)
            {
                return value[..width];
            }

            return value[..(width - 3)] + "...";
        }

        private string[] PadFrame(List<string> frame, int width, int height)
        {
            List<string> result = [];

            foreach (string row in frame.Take(height))
            {
                result.Add(row[..Math.Min(row.Length, width)].PadRight(width));
            }

            while (result.Count < height)
            {
                result.Add(new string(' ', width));
            }

            return [.. result];
        }
    }
}
