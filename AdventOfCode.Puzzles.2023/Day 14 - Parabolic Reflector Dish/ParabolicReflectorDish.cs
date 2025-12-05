namespace AdventOfCode.Puzzles._2023.Day_14___Parabolic_Reflector_Dish
{
    using AdventOfCode.Core;

    public class ParabolicReflectorDish
    {
        public ParabolicReflectorDish(string[] input)
        {
            this.Map = new(input, (c) => c);
            Dictionary<string, (int Count, int Load)> states = new();

            while(true)
            {
                this.Tilt(Cardinal.North);
                this.Tilt(Cardinal.West);
                this.Tilt(Cardinal.South);
                this.Tilt(Cardinal.East);

                string state = this.Map.ToString(c => c);

                if (!states.ContainsKey(state))
                {
                    states.Add(state, (1, this.Load()));
                    continue;
                }

                if (states[state].Count == 2)
                {
                    break;
                }

                states[state] = (2, states[state].Load);
            }

            int count = states.Count;
            states = states.Where(x => x.Value.Count == 2).ToDictionary(x => x.Key, x => x.Value);
            int index = (1000000000 - (count - states.Count)) % states.Count;
            int result = states.ElementAt(index - 1).Value.Load;
        }

        private VectorArray<int, char> Map { get; }

        private int Load() => this.Map.Values('O').Sum(x => this.Map.Height - x.Point.Y);

        private void Tilt(Cardinal direction)
        {
            var enumerator = this.Map.AxisEnumerator();

            if (direction == Cardinal.East
                || direction == Cardinal.South)
            {
                enumerator = enumerator.Reverse();
            }

            foreach (var cell in enumerator)
            {
                if (cell.Value == 'O')
                {
                    var point = cell.Point;

                    while (true)
                    {
                        var adjacent = this.Map.AdjacentCardinal(point);

                        if (adjacent.Any(x => x.Direction == direction))
                        {
                            var tilt = adjacent.FirstOrDefault(x => x.Direction == direction);

                            if (tilt?.Value == '.')
                            {
                                this.Map[point] = '.';
                                this.Map[tilt.Point] = 'O';
                                point = tilt.Point;
                                continue;
                            }

                            if (tilt?.Value == 'O'
                                || tilt?.Value == '#')
                            {
                                break;
                            }
                        }

                        break;
                    }
                }
            }
        }
    }
}
