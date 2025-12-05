namespace AdventOfCode.Puzzles._2019.Day_06___Universal_Orbit_Map
{
    public class UniversalOrbitMap
    {
        public UniversalOrbitMap(string[] input) => this.Input = (string[])input.Clone();

        public string[] Input { get; }

        public void Process(string key, ref OrbitObject? current)
        {
            if (current == null)
            {
                current = new OrbitObject(key);
            }

            List<(int, string, string)> objects = new();

            int i = 0;

            foreach (string orbit in this.Input)
            {
                if (string.IsNullOrEmpty(orbit))
                {
                    i++;
                    continue;
                }

                string[] parts = orbit.Split(")");

                if (parts[0] == key)
                {
                    objects.Add((i, parts[0], parts[1]));
                }

                i++;
            }

            foreach ((int, string, string) item in objects)
            {
                OrbitObject? orbitObject = new(item.Item3, current);
                current.Children.Add(orbitObject);

                this.Input[item.Item1] = string.Empty;

                this.Process(orbitObject.Name, ref orbitObject);
            }
        }

        public int DirectInDirectCount()
        {
            OrbitObject? com = null;

            this.Process("COM", ref com);

            return com?.Orbits() ?? 0;
        }

        public int MinimumOrbitTransfer()
        {
            OrbitObject? com = null;

            this.Process("COM", ref com);

            OrbitObject? you = com?.Find("YOU");

            return (you?.Parent?.TravelTo("SAN") - 2) ?? 0;
        }
    }
}
