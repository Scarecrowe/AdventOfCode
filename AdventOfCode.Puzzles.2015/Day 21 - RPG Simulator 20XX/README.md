# 🎄 Advent of Code 2015 - Day 21: RPG Simulator 20XX

## 📜 Puzzle Overview

The player needs to buy equipment from a shop and then fight the boss.

Each fight is fully deterministic:

- the player always attacks first
- damage dealt is based on total attack minus total defence
- every attack deals at least `1` damage
- the fight continues until one side reaches `0` hit points or lower

The shop contains:

- exactly one weapon must be equipped
- armour is optional
- up to two rings can be equipped

Part 1 asks for the **least amount of gold** that can be spent and still win the fight.

Part 2 asks for the **most gold** that can be spent while still losing.

---

## 🧩 Part 1

Determine the minimum gold required for the player to defeat the boss.

### 💡 Approach

- Parse the boss stats from the input
- Generate every valid equipment loadout
- Equip the player with that loadout
- Simulate the fight against the boss
- Record the gold cost of every winning setup
- Return the cheapest winning cost

---

## 🧩 Part 2

Determine the maximum gold that can be spent while still losing the fight.

### 💡 Approach

- Reuse the same set of equipment combinations
- Simulate every battle again
- Record the gold cost of every losing setup
- Return the most expensive losing cost

---

## 🧠 Code Breakdown

### `Day21.cs`

This is the puzzle entry point.

- Sets the puzzle title
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Creates `new RpgSimulator20XX(this.Input)`
- Calls `MinGold()`

For Part 2:

- Creates `new RpgSimulator20XX(this.Input)`
- Calls `MaxGold()`

The two parts share the same solver and differ only in which result is selected.

---

### `RpgSimulator20XX.cs`

This class contains the full battle simulation and loadout search.

The constructor:

- creates the shop
- loads the boss from the puzzle input
- creates the base player with `100` hit points
- creates working copies of both combatants for each battle

It stores:

- `Shop`
- `InitalEnemy`
- `InitalPlayer`
- `Enemy`
- `Player`

The main public methods are:

- `Battle()`
- `MinGold()`
- `MaxGold()`

---

### Loading the Boss

`LoadEnemy()` parses the three boss stat lines from the input:

- hit points
- damage
- armor

Those values are used to create the initial enemy instance.

The player starts with:

- `100` hit points
- `0` base strength
- `0` base defence

All extra combat stats come from the equipped items.

---

### Battle Simulation

`Battle()` runs the combat loop.

It starts with:

- the player as attacker
- the enemy as defender

Each round:

- the attacker deals damage
- attacker and defender are swapped
- the loop continues while both still have hit points remaining

When the fight ends, the surviving entity is returned.

This makes the outcome entirely determined by the chosen equipment loadout.

---

### Entity Combat Rules

Combat behaviour is implemented in `Entity.cs`.

Each entity stores:

- `HitPoints`
- `Strength`
- `Defence`
- `Weapon`
- `Armor`
- `Rings`
- `Type`

Two helper methods calculate the final combat stats:

- `TotalAttack()`
- `TotalDefence()`

Attack damage is calculated as:

    TotalAttack - defender.TotalDefence

If that result is below `1`, it is forced to `1`.

This matches the puzzle rule that every attack always causes at least one point of damage.

---

### Equipment Handling

`EquipItem()` and `EquipItems()` in `Entity.cs` assign purchased items to the correct slots:

- one weapon slot
- one armour slot
- a list of rings

The final attack and defence values are built from:

- base entity stats
- weapon power
- armour defence
- ring bonuses

This keeps battle logic separate from shop and loadout generation.

---

### Shop Inventory

`Shop.cs` stores all purchasable items grouped by type:

- weapons
- armour
- rings

It exposes helpers for:

- `Weapons()`
- `Armor()`
- `Rings()`
- `AllRings()`
- `GoldSpent(List<Item> items)`

The shop is populated with the full puzzle inventory, including:

- `5` weapons
- `5` armour options
- `6` rings

Gold cost is calculated by summing the `Cost` value of every equipped item.

---

### Generating Item Combinations

`ItemCombinations()` in `RpgSimulator20XX.cs` builds all valid loadouts.

It creates combinations for:

- weapon only
- weapon and armour
- weapon and one ring
- weapon, armour, and one ring
- weapon and two rings
- weapon, armour, and two rings

Ring pairs are generated using:

- `this.Shop.AllRings().Permutations(2)`

Those ring names are then mapped back to actual shop items.

This ensures the search covers all valid purchasing choices allowed by the puzzle rules.

---

### Finding the Cheapest Win

`MinGold()` loops through every item combination.

For each loadout:

- reset the enemy from the original boss
- reset the player from the original warrior
- equip the items
- calculate the gold spent
- run the battle

If the player wins, the gold value is stored.

The final answer is:

- the minimum gold value from all winning battles

---

### Finding the Most Expensive Loss

`MaxGold()` follows the same structure as `MinGold()`.

The only difference is the filter condition:

- only losing battles are stored

The final answer is:

- the maximum gold value from all losing battles

This makes Part 2 a clean reuse of the same simulation pipeline.

---

## 🛠 Implementation Notes

- Boss stats are parsed directly from the three input lines
- The player and boss are reset before every simulated battle
- Battle order is deterministic because the player always attacks first
- Equipment combinations are generated explicitly rather than discovered during combat
- Part 1 and Part 2 reuse the same equipment search and battle logic
- The only difference between the two parts is whether wins or losses are selected

---

## 🧪 Examples

The puzzle describes a sample fight with these stats:

Player:

- hit points: `8`
- damage: `5`
- armor: `5`

Boss:

- hit points: `12`
- damage: `7`
- armor: `2`

Round by round:

- player deals `3` damage per turn
- boss deals `2` damage per turn

Because the player attacks first, the boss is defeated before the player runs out of hit points.

This demonstrates how deterministic combat makes it possible to evaluate each loadout by simulation.

---

## 🚀 Key Takeaways

- Good example of separating inventory, combat, and search logic into focused classes
- Loadout generation is kept independent from the battle simulation
- Part 2 is solved by reusing the same search and selecting the opposite result set
- Resetting the player and boss before each fight keeps every simulation isolated and predictable

---

## 🔗 References

- https://adventofcode.com/2015/day/21