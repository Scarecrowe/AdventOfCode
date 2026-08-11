namespace AdventOfCode.Core.ConsoleMenu
{
    public class ConsoleMenuItem : IConsoleMenuItem
    {
        public ConsoleMenuItem(
           string name,
           string description)
        {
            this.Name = name;
            this.Description = description;
        }

        public ConsoleMenuItem(object key, string name, string description)
            : this(name, description)
        {
            this.Key = key;
        }

        public object? Key { get; }

        public int Index { get; private set; }

        public string Name { get; }

        public string Description { get; }

        public IConsoleMenuItem SetIndex(int index)
        {
            this.Index = index;

            return this;
        }

        public T? GetKey<T>() => this.Key is T value ? value : default;
    }
}
