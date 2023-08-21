namespace AdventOfCode.Puzzles._2015.Day_21___RPG_Simulator_20XX
{
    using AdventOfCode.Core.Extensions;

    public class RpgSimulator20XX
    {
        public RpgSimulator20XX(string[] input)
        {
            this.Shop = new Shop();
            this.InitalEnemy = LoadEnemy(input);
            this.InitalPlayer = new Warrior(100, 0, 0);
            this.Enemy = new Enemy(this.InitalEnemy);
            this.Player = new Warrior(this.InitalPlayer);
        }

        private Shop Shop { get; set; }

        private Enemy InitalEnemy { get; set; }

        private Warrior InitalPlayer { get; set; }

        private Enemy Enemy { get; set; }

        private Warrior Player { get; set; }

        public IEntity Battle()
        {
            IEntity attacker = this.Player;
            IEntity defender = this.Enemy;

            while (attacker.HitPoints > 0 && defender.HitPoints > 0)
            {
                attacker.Attack(defender);

                attacker = attacker.Type == EntityType.Player
                    ? (IEntity)this.Enemy
                    : (IEntity)this.Player;

                defender = attacker.Type == EntityType.Player
                    ? (IEntity)this.Enemy
                    : (IEntity)this.Player;
            }

            return this.Enemy.HitPoints > 0
                ? (IEntity)this.Enemy
                : (IEntity)this.Player;
        }

        public int MinGold()
        {
            List<int> battles = new();

            List<List<Item>> itemCombinations = this.ItemCombinations();

            foreach (List<Item> items in itemCombinations)
            {
                this.Enemy = new Enemy(this.InitalEnemy);
                this.Player = new Warrior(this.InitalPlayer);
                this.Player.EquipItems(items);

                int gold = Shop.GoldSpent(items);

                if (this.Battle().Type == EntityType.Player)
                {
                    battles.Add(gold);
                }
            }

            return battles.Min();
        }

        public int MaxGold()
        {
            List<int> battles = new();

            List<List<Item>> itemCombinations = this.ItemCombinations();

            foreach (List<Item> items in itemCombinations)
            {
                this.Enemy = new Enemy(this.InitalEnemy);
                this.Player = new Warrior(this.InitalPlayer);
                this.Player.EquipItems(items);

                int gold = Shop.GoldSpent(items);

                if (this.Battle().Type == EntityType.Enemy)
                {
                    battles.Add(gold);
                }
            }

            return battles.Max();
        }

        private static Enemy LoadEnemy(string[] input)
        {
            string[] tokens = input[0].Split(": ");

            int hitPoints = tokens[1].ToInt();

            tokens = input[1].Split(": ");

            int damage = tokens[1].ToInt();

            tokens = input[2].Split(": ");

            int armor = tokens[1].ToInt();

            return new Enemy(hitPoints, damage, armor);
        }

        private List<List<Item>> ItemCombinations()
        {
            List<List<Item>> combinations = new();

            // Weapon
            this.Shop.Weapons().ForEach(weapon => combinations.Add(new() { weapon.Value }));

            // Weapon and Armor
            foreach (KeyValuePair<string, Item> weapon in this.Shop.Weapons())
            {
                foreach (KeyValuePair<string, Item> armor in this.Shop.Armor())
                {
                    combinations.Add(new List<Item> { weapon.Value, armor.Value });
                }
            }

            // Weapon and one Ring
            foreach (KeyValuePair<string, Item> weapon in this.Shop.Weapons())
            {
                foreach (KeyValuePair<string, Item> ring in this.Shop.Rings())
                {
                    combinations.Add(new List<Item> { weapon.Value, ring.Value });
                }
            }

            // Weapon, Armor and one Ring
            foreach (KeyValuePair<string, Item> weapon in this.Shop.Weapons())
            {
                foreach (KeyValuePair<string, Item> armor in this.Shop.Armor())
                {
                    foreach (KeyValuePair<string, Item> ring in this.Shop.Rings())
                    {
                        combinations.Add(new List<Item> { weapon.Value, armor.Value, ring.Value });
                    }
                }
            }

            List<List<string>> ringPermutations = this.Shop.AllRings().Permutations(2);

            // Weapon and two Rings
            foreach (KeyValuePair<string, Item> weapon in this.Shop.Weapons())
            {
                foreach (List<string> rings in ringPermutations)
                {
                    List<Item> combination = new() { weapon.Value };

                    combination.AddRange(rings.Select(x => this.Shop[ItemType.Ring][x]).ToList());

                    combinations.Add(combination);
                }
            }

            // Weapon, Armor and two Rings
            foreach (KeyValuePair<string, Item> weapon in this.Shop.Weapons())
            {
                foreach (KeyValuePair<string, Item> armor in this.Shop.Armor())
                {
                    foreach (List<string> rings in ringPermutations)
                    {
                        List<Item> combination = new() { weapon.Value, armor.Value };

                        combination.AddRange(rings.Select(x => this.Shop[ItemType.Ring][x]).ToList());

                        combinations.Add(combination);
                    }
                }
            }

            return combinations;
        }
    }
}
