namespace AdventOfCode.Puzzles._2015.Day_19___Medicine_for_Rudolph
{
    public class MedicineForRudolph
    {
        public MedicineForRudolph(string[] input)
        {
            this.Replacements = ParseRequirements(input);
            this.Molecule = input[^1];
        }

        public Dictionary<string, List<string>> Replacements { get; }

        public string Molecule { get; }

        public Dictionary<string, int> CreateMolecules()
        {
            Dictionary<string, int> molecules = new();

            foreach (KeyValuePair<string, List<string>> replacement in this.Replacements)
            {
                int index = this.Molecule.IndexOf(replacement.Key);

                while (index != -1)
                {
                    string start = this.Molecule[..index];
                    string end = this.Molecule[(index + replacement.Key.Length) ..];

                    foreach (string value in replacement.Value)
                    {
                        string molecule = $"{start}{value}{end}";

                        if (!molecules.ContainsKey(molecule))
                        {
                            molecules.Add(molecule, 1);
                        }
                    }

                    index = this.Molecule.IndexOf(replacement.Key, ++index);
                }
            }

            return molecules;
        }

        public int FabricateMolecule()
        {
            string molecule = this.Molecule;
            return this.ReduceCompound(ref molecule) + this.Reduce(ref molecule, x => x == "e");
        }

        private static Dictionary<string, List<string>> ParseRequirements(string[] input)
        {
            Dictionary<string, List<string>> result = new();

            foreach (string line in input)
            {
                if (string.IsNullOrEmpty(line))
                {
                    break;
                }

                string[] tokens = line.Split(" => ");

                if (!result.ContainsKey(tokens[0]))
                {
                    result.Add(tokens[0], new());
                }

                result[tokens[0]].Add(tokens[1]);
            }

            return result;
        }

        private int ReduceCompound(ref string compound)
        {
            var steps = 0;

            while (compound.Contains("Ar"))
            {
                var argonIndex = compound.IndexOf("Ar");
                var radonIndex = compound.LastIndexOf("Rn", argonIndex);
                var previousRadonIndex = compound.LastIndexOf("Rn", radonIndex - 1);
                var molecule = previousRadonIndex == -1
                    ? compound.Substring(0, argonIndex + 2)
                    : compound.Substring(previousRadonIndex + 2, argonIndex - previousRadonIndex);
                steps += this.Reduce(ref molecule, x => !x.Contains("Ar"));
                compound = previousRadonIndex == -1
                    ? string.Concat(molecule, compound[(argonIndex + 2) ..])
                    : string.Concat(compound.Substring(0, previousRadonIndex + 2), molecule, compound.Substring(argonIndex + 2));
            }

            return steps;
        }

        private int Reduce(ref string molecule, Func<string, bool> endCondition)
        {
            Dictionary<string, int> list = new() { [molecule] = 0 };

            while (true)
            {
                var item = list.Keys.OrderBy(x => x.Length).First();
                var currentSteps = list[item];
                list.Remove(item);

                foreach (KeyValuePair<string, List<string>> replacement in this.Replacements)
                {
                    foreach (string value in replacement.Value)
                    {
                        for (var index = item.IndexOf(value); index >= 0; index = item.IndexOf(value, index + 1))
                        {
                            var newMolecule = $"{item[..index]}{replacement.Key}{item.Substring(index + value.Length)}";

                            if (endCondition(newMolecule))
                            {
                                molecule = newMolecule;
                                return currentSteps + 1;
                            }

                            list[newMolecule] = currentSteps + 1;
                        }
                    }
                }

                if (list.Keys.Count == 0)
                {
                    return currentSteps;
                }
            }
        }
    }
}
