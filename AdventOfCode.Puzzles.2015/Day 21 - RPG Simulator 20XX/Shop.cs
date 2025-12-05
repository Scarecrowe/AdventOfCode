namespace AdventOfCode.Puzzles._2015.Day_21___RPG_Simulator_20XX
{
    public class Shop : Dictionary<ItemType, Dictionary<string, Item>>
    {
        public Shop()
        {
            this.Populate();
        }

        public static int GoldSpent(List<Item> items) => items.Sum(x => x.Cost);

        public Dictionary<string, Item> Weapons() => this[ItemType.Weapon];

        public Dictionary<string, Item> Armor() => this[ItemType.Armor];

        public Dictionary<string, Item> Rings() => this[ItemType.Ring];

        public List<string> AllRings() => this.Where(x => x.Key == ItemType.Ring).SelectMany(x => x.Value.Select(y => y.Key)).ToList();

        private void Populate()
        {
            this.PopulateWeapons();
            this.PopulateArmor();
            this.PopulateRings();
        }

        private void PopulateWeapons()
        {
            this.Add(ItemType.Weapon, new Dictionary<string, Item>());

            Item item = new("Dagger", 8, 4, 0, ItemType.Weapon);

            this[ItemType.Weapon].Add(item.Name, item);

            item = new("Shortsword", 10, 5, 0, ItemType.Weapon);

            this[ItemType.Weapon].Add(item.Name, item);

            item = new("Warhammer", 25, 6, 0, ItemType.Weapon);

            this[ItemType.Weapon].Add(item.Name, item);

            item = new("Longsword", 40, 7, 0, ItemType.Weapon);

            this[ItemType.Weapon].Add(item.Name, item);

            item = new("Greataxe", 74, 8, 0, ItemType.Weapon);

            this[ItemType.Weapon].Add(item.Name, item);
        }

        private void PopulateArmor()
        {
            this.Add(ItemType.Armor, new());

            Item item = new("Leather", 13, 0, 1, ItemType.Armor);

            this[ItemType.Armor].Add(item.Name, item);

            item = new("Chainmail", 31, 0, 2, ItemType.Armor);

            this[ItemType.Armor].Add(item.Name, item);

            item = new("Splintmail", 53, 0, 3, ItemType.Armor);

            this[ItemType.Armor].Add(item.Name, item);

            item = new("Bandedmail", 75, 0, 4, ItemType.Armor);

            this[ItemType.Armor].Add(item.Name, item);

            item = new("Platemail", 102, 0, 5, ItemType.Armor);

            this[ItemType.Armor].Add(item.Name, item);
        }

        private void PopulateRings()
        {
            this.Add(ItemType.Ring, new());

            Item item = new("Damage +1", 25, 1, 0, ItemType.Ring);

            this[ItemType.Ring].Add(item.Name, item);

            item = new("Damage +2", 50, 2, 0, ItemType.Ring);

            this[ItemType.Ring].Add(item.Name, item);

            item = new("Damage +3", 100, 3, 0, ItemType.Ring);

            this[ItemType.Ring].Add(item.Name, item);

            item = new("Defense +1", 20, 0, 1, ItemType.Ring);

            this[ItemType.Ring].Add(item.Name, item);

            item = new("Defense +2", 40, 0, 2, ItemType.Ring);

            this[ItemType.Ring].Add(item.Name, item);

            item = new("Defense +3", 80, 0, 3, ItemType.Ring);

            this[ItemType.Ring].Add(item.Name, item);
        }
    }
}
