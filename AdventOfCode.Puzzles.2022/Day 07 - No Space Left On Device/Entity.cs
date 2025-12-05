namespace AdventOfCode.Puzzles._2022.Day_07___No_Space_Left_On_Device
{
    public class Entity
    {
        public Entity(string name, Entity? parent)
        {
            this.Name = name;
            this.Parent = parent;
            this.Type = EntityType.Directory;
            this.Children = new();
            parent?.Children.Add(this);
        }

        public Entity(string name, Entity? parent, int size)
            : this(name, parent)
        {
            this.Type = EntityType.File;
            this.Size = size;

            if (parent != null)
            {
                Entity? current = parent;

                while (current != null)
                {
                    current.Size += this.Size;

                    current = current.Parent;
                }
            }
        }

        public string Name { get; private set; }

        public Entity? Parent { get; private set; }

        public EntityType Type { get; private set; }

        public int Size { get; private set; }

        public List<Entity> Children { get; private set; }

        public bool IsFile => this.Type == EntityType.File;
    }
}
