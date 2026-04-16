# 🎄 Advent of Code 2015 - Day 22: Wizard Simulator 20XX

## 📜 Puzzle Overview

The player is a wizard fighting a boss in a turn-based battle.

The wizard has:

- hit points
- mana

The boss has:

- hit points
- damage

The available spells are:

- `Magic Missile`
- `Drain`
- `Shield`
- `Poison`
- `Recharge`

Some spells apply their effect immediately, while others remain active for several turns.

Part 1 asks for the **least amount of mana** needed to defeat the boss.

Part 2 uses the same fight rules, but adds hard mode:

- the player loses `1` hit point at the start of every player turn

---

## 🧩 Part 1

Determine the minimum amount of mana required for the wizard to defeat the boss.

### 💡 Approach

- Load the boss stats from the puzzle input
- Start the wizard with `50` hit points and `500` mana
- Try different spell sequences
- Simulate the battle for each sequence
- Track the total mana spent
- Return the cheapest winning path

---

## 🧩 Part 2

Repeat the same search, but apply hard mode.

### 💡 Approach

- Reuse the same spell search logic
- At the start of every player turn, remove `1` hit point from the wizard
- Reject any path where the player dies before acting
- Return the minimum mana cost among the remaining winning paths

---

## 🧠 Code Breakdown

### `Day22.cs`

This is the puzzle entry point.

- Sets the puzzle title
- Loads the puzzle input
- Creates the player as `new Wizard(50, 500)`
- Loads the boss using `WizardSimulator20XX.LoadEnemy(this.Input)`

For Part 1:

- Calls `MinMana(BattleMode.Easy)`

For Part 2:

- Calls `MinMana(BattleMode.Hard)`

The only difference between the two parts is the selected battle mode.

---

### `BattleMode.cs`

This enum switches between the two game modes:

- `Easy`
- `Hard`

This keeps the extra Part 2 rule isolated from the rest of the puzzle wiring.

---

### `WizardSimulator20XX.cs`

This class contains the full search and battle simulation.

It stores:

- the initial player
- the initial boss
- the spell list

The main public methods are:

- `LoadEnemy(string[] input)`
- `Battle((List<SpellType> Spells, IEntity Wizard, IEntity Boss) state, BattleMode mode)`
- `MinMana(BattleMode mode)`

---

### Loading the Boss

`LoadEnemy()` parses the puzzle input and reads:

- `Hit Points`
- `Damage`

Those values are used to create the boss as a `Warrior`.

The boss input does not need mana, armour, or spell setup.

---

### Spell Definitions

`Spells.cs` defines the full spell book.

The available spells are:

- `Magic Missile`
- `Drain`
- `Shield`
- `Poison`
- `Recharge`

Each spell is stored as a `Spell` object with fields such as:

- mana cost
- damage
- healing
- mana recovery
- armour bonus
- effect duration

This keeps the spell data separate from the battle logic.

---

### Spell Types

The spell model distinguishes between:

- immediate spells
- effect spells

Immediate spells apply their result right away.

Effect spells stay active for multiple turns using an `Effect` object that tracks:

- the original spell
- the number of turns remaining

This is handled by:

- `Spell.cs`
- `Effect.cs`

---

### Entity Model

Both the wizard and the boss inherit from `Entity`.

The shared entity logic includes:

- hit points
- mana points
- damage
- defence
- active effects
- spell casting
- attacking
- effect processing

Specialised classes are:

- `Wizard`
- `Warrior`

This allows the combat rules to be shared while keeping each fighter type simple.

---

### Applying Effects

`ApplyEffects()` in `Entity.cs` processes all active timed effects.

At the start of each turn:

- defence is reset
- each active effect is processed
- turns remaining are reduced
- expired effects are removed

The active effects behave like this:

- `Shield` adds armour
- `Poison` damages the target
- `Recharge` restores mana

This means timed spells do their work automatically every turn until they expire.

---

### Casting Spells

`Cast()` handles the spell action for the wizard.

For effect spells:

- `Poison` is added to the boss
- `Shield` is added to the wizard
- `Recharge` is added to the wizard

For direct spells:

- `Magic Missile` deals damage immediately
- `Drain` deals damage and heals immediately

Mana is always reduced when the spell is successfully cast, and the spent amount is added to `ManaSpent`.

If an effect spell is already active, the cast fails and that state is rejected.

---

### Battle Flow

`Battle()` simulates one round of combat for the current spell sequence state.

The round works like this:

1. In hard mode, the wizard loses `1` hit point at the start of the turn
2. The next chosen spell is looked up
3. If the wizard cannot afford that spell, the state fails
4. Effects are applied to both boss and wizard
5. If the boss dies from effects, the battle ends
6. The wizard casts the selected spell
7. Effects are applied again for the boss turn
8. If the boss survives, the boss attacks the wizard

If the wizard survives and the state is still valid, the search can continue by appending another spell.

---

### Searching for the Minimum Mana

`MinMana()` searches through possible spell sequences.

It uses a priority queue of states containing:

- the list of chosen spells
- a cloned wizard state
- a cloned boss state

The search begins by enqueueing one state for each possible opening spell.

Each time a state is processed:

- battle logic is applied for the newest spell
- dead or invalid paths are discarded
- winning paths update the best known mana result
- surviving paths are expanded by trying each spell again

State cloning is handled by the copy constructors in `Wizard`, `Warrior`, and `Entity`, which preserve:

- hit points
- mana
- damage
- defence
- active effects
- mana spent

This allows each branch of the search to continue independently.

---

### Spell Restrictions During Search

When expanding a state, the search skips some repeated effect choices.

In particular, it avoids immediately repeating:

- `Shield`
- `Poison`
- `Recharge`

This prevents obvious invalid recasts of long-running effects and keeps the search space smaller.

---

## 🛠 Implementation Notes

- The boss is loaded directly from the input stats
- The player always starts with `50` hit points and `500` mana
- Effect spells are tracked using active `Effect` objects
- Battle state is copied for each search branch
- Part 1 and Part 2 share the same solver
- Hard mode only changes the start of the player's turn
- The final answer is the minimum recorded `ManaSpent` from all winning paths

---

## 🧪 Examples

The puzzle includes these spell effects:

- `Magic Missile` costs `53` mana and deals `4` damage
- `Drain` costs `73` mana, deals `2` damage, and heals `2` hit points
- `Shield` costs `113` mana and provides armour for `6` turns
- `Poison` costs `173` mana and damages the boss for `6` turns
- `Recharge` costs `229` mana and restores mana for `5` turns

These spell definitions are used by the simulator when building and evaluating battle states.

---

## 🚀 Key Takeaways

- Good example of modelling turn-based combat with explicit game state
- Timed spell effects are separated cleanly from direct spell actions
- The search works by branching on spell choices and cloning combat state
- Part 2 is solved by adding a single hard-mode penalty before the player's turn
- The priority-queue search keeps the solution focused on finding the cheapest win

---

## 🔗 References

- https://adventofcode.com/2015/day/22