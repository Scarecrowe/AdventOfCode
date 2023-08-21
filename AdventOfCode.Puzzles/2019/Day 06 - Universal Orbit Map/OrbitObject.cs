namespace AdventOfCode.Puzzles._2019.Day_06___Universal_Orbit_Map
{
    public class OrbitObject
    {
        public OrbitObject(string name)
        {
            this.Name = name;
            this.Children = new();
        }

        public OrbitObject(string name, OrbitObject parent)
        {
            this.Name = name;
            this.Children = new();
            this.Parent = parent;
        }

        public string Name { get; }

        public OrbitObject? Parent { get; set; }

        public HashSet<OrbitObject> Children { get; }

        public new string ToString() => this.Name;

        public int TravelTo(string name)
        {
            int count = 0;
            bool travelled = false;

            var parent = this.Parent;

            this.Traverse(ref parent, name, ref count, ref travelled);

            return count;
        }

        public int Orbits()
        {
            int result = 0;
            OrbitObject? parent = this.Parent;

            while (parent != null)
            {
                result += 1;

                parent = parent.Parent;
            }

            return result + this.Children.Sum(x => x.Orbits());
        }

        public int OrbitsTo(string name)
        {
            OrbitObject? parent = this;
            int count = 0;

            while (parent != null)
            {
                count += 1;

                if (parent.Name == name)
                {
                    break;
                }

                parent = parent.Parent;
            }

            return count;
        }

        public OrbitObject? Find(string name)
        {
            if (this.Name == name)
            {
                return this;
            }

            foreach (OrbitObject child in this.Children)
            {
                OrbitObject? result = child.Find(name);

                if (result == null)
                {
                    continue;
                }

                return result;
            }

            return null;
        }

        private void Traverse(ref OrbitObject? current, string name, ref int count, ref bool travelled)
        {
            if (travelled || current == null)
            {
                return;
            }

            count++;

            OrbitObject? destination;

            foreach (OrbitObject child in current.Children)
            {
                destination = child.Find(name);

                if (destination != null)
                {
                    count += destination.OrbitsTo(current.Name);
                    travelled = true;
                    return;
                }
            }

            current = current.Parent;

            if (current != null)
            {
                this.Traverse(ref current, name, ref count, ref travelled);
            }
        }
    }
}