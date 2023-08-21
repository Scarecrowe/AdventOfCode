namespace AdventOfCode.Puzzles._2019.Day_14___Space_Stoichiometry
{
    public class Reaction
    {
        public Reaction(string name, long output = 1)
        {
            this.Input = new();
            this.Product = new();
            this.Name = name;
            this.Output = output;
        }

        public string Name { get; }

        public long Output { get; set; }

        private Dictionary<string, long> Input { get; }

        private Dictionary<string, long> Product { get; }

        public void AddSource(string name, long quantity) => this.Input.Add(name, quantity);

        public void AddProduct(string name, long quantity) => this.Product.Add(name, quantity);

        public IEnumerable<KeyValuePair<string, long>> GetDependencies() => this.Input;

        public IEnumerable<KeyValuePair<string, long>> GetProducts() => this.Product;
    }
}
