namespace AdventOfCode.Puzzles._2024.Day_05___Print_Queue
{
    ///TODO - sort this
    public class PrintQueue
    {
        public PrintQueue(string[] input)
        {
            this.Input = input;
        }

        public string[] Input { get; private set; }

        public string Silver()
        {
            bool parsed = false;
            List<(int X, int Y)> ordering = new List<(int X, int Y)>();
            List<List<int>> updates = new List<List<int>>();

            foreach (string line in this.Input)
            {
                if (string.IsNullOrEmpty(line))
                {
                    parsed = true;
                    continue;
                }

                if (!parsed)
                {
                    var values = line.Split("|").Select(x => int.Parse(x)).ToArray();

                    ordering.Add((values[0], values[1]));
                    continue;
                }

                var valuesA = line.Split(",").Select(x => int.Parse(x)).ToList();

                updates.Add(valuesA);
            }

            List<List<int>> valid = new List<List<int>>();
            bool finished = false;

            foreach (List<int> update in updates)
            {
                finished = false;

                for (int i = 0; i < update.Count; i++)
                {
                    if (finished)
                    {
                        break;
                    }

                    for (int j = i + 1; j < update.Count; j++)
                    {
                        if (finished)
                        {
                            break;
                        }

                        if (!ordering.Contains((update[i], update[j])))
                        {
                            finished = true;
                            break;
                        }
                    }
                }

                if (!finished)
                {
                    valid.Add(update);
                }
            }

            int result = 0;

            foreach (var v in valid)
            {
                decimal x = Math.Ceiling((decimal)v.Count / 2);
                result += v[(int)x - 1];
            }

            return $"{result}";
        }

        public string Gold()
        {
            bool parsed = false;
            List<(int X, int Y)> ordering = new List<(int X, int Y)>();
            List<List<int>> updates = new List<List<int>>();

            foreach (string line in this.Input)
            {
                if (string.IsNullOrEmpty(line))
                {
                    parsed = true;
                    continue;
                }

                if (!parsed)
                {
                    var values = line.Split("|").Select(x => int.Parse(x)).ToArray();

                    ordering.Add((values[0], values[1]));
                    continue;
                }

                var valuesA = line.Split(",").Select(x => int.Parse(x)).ToList();

                updates.Add(valuesA);
            }

            List<List<int>> valid = new List<List<int>>();
            List<List<int>> inValid = new List<List<int>>();
            bool finished = false;

            foreach (List<int> update in updates)
            {
                finished = false;

                for (int i = 0; i < update.Count; i++)
                {
                    if (finished)
                    {
                        break;
                    }

                    for (int j = i + 1; j < update.Count; j++)
                    {
                        if (finished)
                        {
                            break;
                        }

                        if (!ordering.Contains((update[i], update[j])))
                        {
                            finished = true;
                            break;
                        }
                    }
                }

                if (!finished)
                {
                    valid.Add(update);
                }
                else
                {
                    inValid.Add(update);
                }
            }

            valid = new List<List<int>>();

            foreach (var invalid in inValid)
            {
                invalid.Sort((a, b) =>
                {
                    var yAxis = ordering.Where(x => x.X == a).Select(x => x.Y);

                    if (yAxis.Contains(b))
                    {
                        return -1;
                    }

                    return 1;
                });

                valid.Add(invalid);
            }

            int result = 0;

            foreach (var v in valid)
            {
                decimal x = Math.Ceiling((decimal)v.Count / 2);
                result += v[(int)x - 1];
            }

            return $"{result}";
        }
    }
}
