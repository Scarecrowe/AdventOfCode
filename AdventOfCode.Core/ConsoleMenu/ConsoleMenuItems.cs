namespace AdventOfCode.Core.ConsoleMenu
{
    public class ConsoleMenuItems : List<IConsoleMenuItem>, IConsoleMenuItems
    {
        public void Add(
            object key,
            string name,
            string description)
        {
            int index = !this.Any() ? 1 : this.Max(x => x.Index) + 1;

            ConsoleMenuItem item = new(key, name, description);
            item.SetIndex(index);

            this.Add(item);
        }

        public void Add(string name, string description)
        {
            int index = !this.Any() ? 1 : this.Max(x => x.Index) + 1;
            ConsoleMenuItem item = new(name, description);
            item.SetIndex(index);

            this.Add(item);
        }

        public void Add(int index, string name, string description)
        {
            ConsoleMenuItem item = new(name, description);
            item.SetIndex(index);

            this.Add(item);
        }

        public IConsoleMenuItem? GetByKey(object key)
            => this.FirstOrDefault(x => x == key);
    }
}
