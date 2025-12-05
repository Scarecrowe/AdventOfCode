namespace AdventOfCode.Puzzles._2024.Day_24___Crossed_Wires
{
    public class CrossedWires
    {
        private readonly Dictionary<string, string> registers = new();
        private readonly Dictionary<string, int> cache = new();

        public CrossedWires(string filePath)
        {
            var sections = File.ReadAllText(filePath).Split(new string[] { "\r\n\r\n", "\n\n" }, StringSplitOptions.None);

            foreach (var line in sections[0].Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
            {
                var parts = line.Split(": ", StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length == 2)
                {
                    registers[parts[0]] = parts[1];
                }
            }

            foreach (var line in sections[1].Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
            {
                var parts = line.Split(" -> ", StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length == 2)
                {
                    registers[parts[1]] = parts[0];
                }
            }
        }

        private int Evaluate(string name)
        {
            if (int.TryParse(name, out int val))
            {
                return val;
            }
                
            if (cache.TryGetValue(name, out int cached))
            {
                return cached;
            }

            var value = registers[name];
            var parts = value.Split(' ');
            int result;

            if (parts.Length == 1)
            {
                result = Evaluate(parts[0]);
            }
            else
            {
                int op1 = Evaluate(parts[0]);
                int op2 = Evaluate(parts[2]);
                result = parts[1] switch
                {
                    "AND" => op1 & op2,
                    "OR" => op1 | op2,
                    "XOR" => op1 ^ op2,
                    _ => throw new InvalidOperationException($"Unknown operator: {parts[1]}")
                };
            }

            cache[name] = result;
            return result;
        }

        public long ZOutput()
        {
            var zRegisters = registers.Keys
                                      .Where(k => k.StartsWith("z"))
                                      .OrderByDescending(k => k);

            long result = 0;

            foreach (var reg in zRegisters)
            {
                result = result * 2 + Evaluate(reg);
            }

            return result;
        }

        public string WireNames()
        {
            var swaps = new List<string>();
            int index = 0;
            string? carryReg = null;

            while (registers.ContainsKey($"x{index:00}") && swaps.Count < 8)
            {
                string xReg = $"x{index:00}";
                string yReg = $"y{index:00}";
                string zReg = $"z{index:00}";

                if (index == 0)
                {
                    carryReg = FindExpression(xReg, "AND", yReg);
                }
                else
                {
                    string? xorReg = FindExpression(xReg, "XOR", yReg);
                    string? andReg = FindExpression(xReg, "AND", yReg);
                    string? carryInReg = FindExpression(xorReg, "XOR", carryReg);

                    if (carryInReg == null)
                    {
                        swaps.Add(xorReg);
                        swaps.Add(andReg);
                        (registers[xorReg], registers[andReg]) = (registers[andReg], registers[xorReg]);
                        index = 0;
                        continue;
                    }

                    if (carryInReg != zReg)
                    {
                        swaps.Add(carryInReg);
                        swaps.Add(zReg);
                        (registers[carryInReg], registers[zReg]) = (registers[zReg], registers[carryInReg]);
                        index = 0;
                        continue;
                    }

                    carryInReg = FindExpression(xorReg, "AND", carryReg);
                    carryReg = FindExpression(andReg, "OR", carryInReg);
                }

                index++;
            }

            return string.Join(",", swaps.OrderBy(s => s));
        }

        private string? FindExpression(string op1, string op, string op2)
        {
            var try1 = registers.FirstOrDefault(r => r.Value == $"{op1} {op} {op2}").Key;
            var try2 = registers.FirstOrDefault(r => r.Value == $"{op2} {op} {op1}").Key;
            return try1 ?? try2;
        }
    }
}
