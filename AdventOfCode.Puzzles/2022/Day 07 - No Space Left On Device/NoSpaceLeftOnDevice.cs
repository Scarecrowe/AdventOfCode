namespace AdventOfCode.Puzzles._2022.Day_07___No_Space_Left_On_Device
{
    public class NoSpaceLeftOnDevice
    {
        public NoSpaceLeftOnDevice(string[] input)
        {
            this.Input = input;
            this.Root = new("/", null);
            this.ParseInput();
        }

        public Entity Root { get; private set; }

        private string[] Input { get; set; }

        public int Sum(Entity? current = null)
        {
            if (current == null)
            {
                current = this.Root;
            }

            int sum = 0;

            if (current.Size < 100000)
            {
                sum = current.Size;
            }

            foreach (Entity child in current.Children)
            {
                if (!child.IsFile)
                {
                    sum += this.Sum(child);
                }
            }

            return sum;
        }

        public int Min()
        {
            List<int> results = new();
            int freeSpace = 70000000 - this.Root.Size;
            int requiredSpace = 30000000 - freeSpace;
            this.MinRecursive(this.Root, ref results, requiredSpace);

            return results.Min();
        }

        private void ParseInput()
        {
            Entity? current = this.Root;

            foreach (string line in this.Input)
            {
                string[] instruction = line.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                if (instruction[0] == "$")
                {
                    if (instruction[1] != "cd")
                    {
                        continue;
                    }

                    if (instruction[2] == "/")
                    {
                        current = this.Root;
                    }
                    else if (instruction[2] == "..")
                    {
                        current = current?.Parent;
                    }
                    else
                    {
                        current = current?.Children.FirstOrDefault(x => x.Name == instruction[2]);
                    }
                }
                else if (instruction[0] == "dir")
                {
                    _ = new Entity(instruction[1], current);
                }
                else
                {
                    _ = new Entity(instruction[1], current, int.Parse(instruction[0]));
                }
            }
        }

        private void MinRecursive(Entity current, ref List<int> results, int requiredSpace)
        {
            if (current.Size >= requiredSpace)
            {
                results.Add(current.Size);
            }

            foreach (Entity child in current.Children)
            {
                if (!child.IsFile)
                {
                    this.MinRecursive(child, ref results, requiredSpace);
                }
            }
        }
    }
}
