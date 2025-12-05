namespace AdventOfCode.Puzzles._2023.Day_19___Aplenty
{
    using AdventOfCode.Core.Extensions;

    public class Aplenty
    {
        public Aplenty(string[] input)
        {
            this.AcceptedRanges = new();
            this.Workflows = new();
            this.Ratings = new();
            bool workflow = true;
            string[] tokens = Array.Empty<string>();

            foreach (string line in input)
            {
                if (string.IsNullOrEmpty(line))
                {
                    workflow = false;
                    continue;
                }

                if (workflow)
                {
                    tokens = line.Split("{");
                    string name = tokens[0];
                    List<(char In, char Symbol, int Value, string Out)> rules = new();

                    foreach(var rule in tokens[1].Replace("}").Split(",").ToArray())
                    {
                        tokens = rule.Split(":");

                        if (tokens[0].Contains(">")
                            || tokens[0].Contains("<"))
                        {
                            char @in = tokens[0][0];
                            char symbol = tokens[0][1];
                            int value = int.Parse(tokens[0][2..^0]);
                            string @out = tokens[1];
                            rules.Add((@in, symbol, value, @out));
                            continue;
                        }

                        rules.Add(('z', 'z', 0, tokens[0]));
                    }

                    this.Workflows.Add(name, rules);

                    continue;
                }

                tokens = line.Replace("{").Replace("}").Split(",");

                this.Ratings.Add((int.Parse(tokens[0][2..^0]), int.Parse(tokens[1][2..^0]), int.Parse(tokens[2][2..^0]), int.Parse(tokens[3][2..^0])));
            }
        }

        private Dictionary<string, List<(char In, char Symbol, int Value, string Out)>> Workflows { get; }

        private List<(int X, int M, int A, int S)> Ratings { get; }

        private List<(Dictionary<char, int[]> Range, long Size)> AcceptedRanges { get; }

        public int TotalRatings()
        {
            int result = 0;

            foreach(var rating in this.Ratings)
            {
                string current = "in";

                while(current != "A"
                    && current != "R")
                {
                    var workflow = this.Workflows[current];

                    foreach(var rule in workflow)
                    {
                        if (rule.In == 'z')
                        {
                            current = rule.Out;
                            break;
                        }
                        else if (rule.In == 'x')
                        {
                            if (rule.Symbol == '<')
                            {
                                if (rating.X < rule.Value)
                                {
                                    current = rule.Out;
                                    break;
                                }
                            }
                            else
                            {
                                if (rating.X > rule.Value)
                                {
                                    current = rule.Out;
                                    break;
                                }
                            }
                        }
                        else if (rule.In == 'm')
                        {
                            if (rule.Symbol == '<')
                            {
                                if (rating.M < rule.Value)
                                {
                                    current = rule.Out;
                                    break;
                                }
                            }
                            else
                            {
                                if (rating.M > rule.Value)
                                {
                                    current = rule.Out;
                                    break;
                                }
                            }
                        }
                        else if (rule.In == 'a')
                        {
                            if (rule.Symbol == '<')
                            {
                                if (rating.A < rule.Value)
                                {
                                    current = rule.Out;
                                    break;
                                }
                            }
                            else
                            {
                                if (rating.A > rule.Value)
                                {
                                    current = rule.Out;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            if (rule.Symbol == '<')
                            {
                                if (rating.S < rule.Value)
                                {
                                    current = rule.Out;
                                    break;
                                }
                            }
                            else
                            {
                                if (rating.S > rule.Value)
                                {
                                    current = rule.Out;
                                    break;
                                }
                            }
                        }
                    }
                }

                if (current == "A")
                {
                    result += rating.X + rating.M + rating.A + rating.S;
                }
            }

            return result;
        }

        public string Sort((int X, int M, int A, int S) rating)
        {
            string current = "in";

            while (current != "A"
                && current != "R")
            {
                var workflow = this.Workflows[current];

                foreach (var rule in workflow)
                {
                    if (rule.In == 'z')
                    {
                        current = rule.Out;
                        break;
                    }
                    else if (rule.In == 'x')
                    {
                        if (rule.Symbol == '<')
                        {
                            if (rating.X < rule.Value)
                            {
                                current = rule.Out;
                                break;
                            }
                        }
                        else
                        {
                            if (rating.X > rule.Value)
                            {
                                current = rule.Out;
                                break;
                            }
                        }
                    }
                    else if (rule.In == 'm')
                    {
                        if (rule.Symbol == '<')
                        {
                            if (rating.M < rule.Value)
                            {
                                current = rule.Out;
                                break;
                            }
                        }
                        else
                        {
                            if (rating.M > rule.Value)
                            {
                                current = rule.Out;
                                break;
                            }
                        }
                    }
                    else if (rule.In == 'a')
                    {
                        if (rule.Symbol == '<')
                        {
                            if (rating.A < rule.Value)
                            {
                                current = rule.Out;
                                break;
                            }
                        }
                        else
                        {
                            if (rating.A > rule.Value)
                            {
                                current = rule.Out;
                                break;
                            }
                        }
                    }
                    else
                    {
                        if (rule.Symbol == '<')
                        {
                            if (rating.S < rule.Value)
                            {
                                current = rule.Out;
                                break;
                            }
                        }
                        else
                        {
                            if (rating.S > rule.Value)
                            {
                                current = rule.Out;
                                break;
                            }
                        }
                    }
                }
            }

            return current;
        }

        public long RangeSize(int[] range) => range[1] - range[0];

        public long BatchSize(Dictionary<char, int[]> range)
            => this.RangeSize(range['x']) * this.RangeSize(range['m']) * this.RangeSize(range['a']) * this.RangeSize(range['s']);

        public long Accepted(Dictionary<char, int[]> range, string workflow)
        {
            if (workflow == "R")
            {
                return 0;
            }

            if (workflow == "A")
            {
                long size = this.BatchSize(range);
                this.AcceptedRanges.Add((range, size));
                return size;
            }

            long result = 0;

            foreach(var rule in this.Workflows[workflow])
            {
                if (rule.In == 'z')
                {
                    continue;
                }

                var current = range[rule.In];

                if (rule.Symbol == '<')
                {
                    if (current[1] <= rule.Value)
                    {
                        result += this.Accepted(range, rule.Out);
                        return result;
                    }
                    else if (current[0] < rule.Value)
                    {
                        var match = new Dictionary<char, int[]>();

                        foreach(var pair in range)
                        {
                            if (pair.Key == rule.In)
                            {
                                match.Add(pair.Key, new int[] { current[0], rule.Value });
                            }
                            else
                            {
                                match.Add(pair.Key, pair.Value);
                            }
                        }

                        result += this.Accepted(match, rule.Out);

                        range[rule.In] = new int[] { rule.Value, current[1] };
                        continue;
                    }

                    continue;
                }

                if (current[0] > rule.Value)
                {
                    result += this.Accepted(range, rule.Out);
                    return result;
                }
                else if (current[1] > rule.Value + 1)
                {
                    var match = new Dictionary<char, int[]>();

                    foreach (var pair in range)
                    {
                        if (pair.Key == rule.In)
                        {
                            match.Add(pair.Key, new int[] { rule.Value + 1, current[1] });
                        }
                        else
                        {
                            match.Add(pair.Key, pair.Value);
                        }
                    }

                    result += this.Accepted(match, rule.Out);

                    range[rule.In] = new int[] { current[0], rule.Value + 1 };
                    continue;
                }
            }

            result += this.Accepted(range, this.Workflows[workflow][^1].Out);

            return result;
        }

        public long CombinationRatings()
        {
            Dictionary<char, int[]> range = new()
            {
                { 'x', new int[] { 1, 4001 } },
                { 'm', new int[] { 1, 4001 } },
                { 'a', new int[] { 1, 4001 } },
                { 's', new int[] { 1, 4001 } }
            };

            return this.Accepted(range, "in");
        }
    }
}
