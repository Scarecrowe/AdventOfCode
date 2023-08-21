namespace AdventOfCode.Puzzles._2020.Day_17___Conway_Cubes
{
    using AdventOfCode.Core;

    public class ConwayCubes
    {
        public static int Simple(string[] input)
        {
            Dictionary<Vector<int>, bool> cubes = new();

            for (var y = 0; y < input.Length; y++)
            {
                for (var x = 0; x < input[y].Length; x++)
                {
                    cubes[new(x, y, 0)] = input[y][x] == '#';
                }
            }

            var cycles = 0;

            while (cycles < 6)
            {
                Dictionary<Vector<int>, bool> next = new();

                foreach (var cube in cubes)
                {
                    var allAdjacent = GetAdjacent3D(cube.Key, cubes);

                    foreach (var adjacent in allAdjacent)
                    {
                        if (cubes.ContainsKey(adjacent.Key))
                        {
                            continue;
                        }

                        var outer = GetAdjacent3D(adjacent.Key, cubes).Where(x => cubes.ContainsKey(x.Key));

                        if (outer.Count(x => x.Value) == 3)
                        {
                            next[adjacent.Key] = true;
                        }
                    }

                    if (cube.Value == true && (allAdjacent.Count(x => x.Value) == 2 || allAdjacent.Count(x => x.Value) == 3))
                    {
                        next[cube.Key] = cube.Value;
                    }
                    else if (cube.Value == false && allAdjacent.Count(x => x.Value) == 3)
                    {
                        next[cube.Key] = true;
                    }
                    else
                    {
                        next[cube.Key] = false;
                    }
                }

                cycles++;
                cubes = next.ToDictionary(x => x.Key, x => x.Value);
            }

            return cubes.Count(x => x.Value);
        }

        public static int Advanced(string[] input)
        {
            Dictionary<Vector<int>, bool> cubes = new();

            for (var y = 0; y < input.Length; y++)
            {
                for (var x = 0; x < input[y].Length; x++)
                {
                    cubes[new(x, y, 0, 0)] = input[y][x] == '#';
                }
            }

            int cycles = 0;

            while (cycles < 6)
            {
                Dictionary<Vector<int>, bool> next = new();

                foreach (var cube in cubes)
                {
                    var allAdjacent = GetAdjacent4D(cube.Key, cubes);

                    foreach (var adjacent in allAdjacent)
                    {
                        if (cubes.ContainsKey(adjacent.Key))
                        {
                            continue;
                        }

                        var outer = GetAdjacent4D(adjacent.Key, cubes).Where(x => cubes.ContainsKey(x.Key));

                        if (outer.Count(x => x.Value) == 3)
                        {
                            next[adjacent.Key] = true;
                        }
                    }

                    if (cube.Value == true && (allAdjacent.Count(x => x.Value) == 2 || allAdjacent.Count(x => x.Value) == 3))
                    {
                        next[cube.Key] = cube.Value;
                    }
                    else if (cube.Value == false && allAdjacent.Count(x => x.Value) == 3)
                    {
                        next[cube.Key] = true;
                    }
                    else
                    {
                        next[cube.Key] = false;
                    }
                }

                cycles++;
                cubes = next.ToDictionary(x => x.Key, x => x.Value);
            }

            return cubes.Count(x => x.Value);
        }

        private static Dictionary<Vector<int>, bool> GetAdjacent3D(Vector<int> target, Dictionary<Vector<int>, bool> cubes)
        {
            List<Vector<int>> adjacent = new();

            for (int x = -1 + target.X; x <= 1 + target.X; x++)
            {
                for (int y = -1 + target.Y; y <= 1 + target.Y; y++)
                {
                    for (int z = -1 + target.Z; z <= 1 + target.Z; z++)
                    {
                        if (x == target.X && y == target.Y && z == target.Z)
                        {
                            continue;
                        }
                        else
                        {
                            adjacent.Add(new(x, y, z));
                        }
                    }
                }
            }

            Dictionary<Vector<int>, bool> result = new();

            foreach (var item in adjacent)
            {
                cubes.TryGetValue(item, out var state);
                result[item] = state;
            }

            return result;
        }

        private static Dictionary<Vector<int>, bool> GetAdjacent4D(Vector<int> target, Dictionary<Vector<int>, bool> cubes)
        {
            List<Vector<int>> adjacent = new();

            for (int x = -1 + target.X; x <= 1 + target.X; x++)
            {
                for (int y = -1 + target.Y; y <= 1 + target.Y; y++)
                {
                    for (int z = -1 + target.Z; z <= 1 + target.Z; z++)
                    {
                        for (int t = -1 + target.T; t <= 1 + target.T; t++)
                        {
                            if (x == target.X && y == target.Y && z == target.Z && t == target.T)
                            {
                                continue;
                            }
                            else
                            {
                                adjacent.Add(new(x, y, z, t));
                            }
                        }
                    }
                }
            }

            Dictionary<Vector<int>, bool> result = new();

            foreach (var item in adjacent)
            {
                cubes.TryGetValue(item, out var state);
                result[item] = state;
            }

            return result;
        }
    }
}
