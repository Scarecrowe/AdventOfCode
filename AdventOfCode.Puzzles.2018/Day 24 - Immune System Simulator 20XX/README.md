# 🎄 Advent of Code 2018 - Day 24: Immune System Simulator 20XX

## 📜 Puzzle Overview

This puzzle simulates a large-scale battle between two armies:

- Immune System
- Infection

Each army consists of multiple groups, where each group contains identical units with shared stats.

An input block looks like:

    Immune System:
    17 units each with 5390 hit points (weak to radiation, bludgeoning) with
     an attack that does 4507 fire damage at initiative 2

    Infection:
    801 units each with 4706 hit points (weak to radiation) with an attack
     that does 116 bludgeoning damage at initiative 1

Each group has:

- number of units
- hit points per unit
- attack damage
- attack type
- initiative
- weaknesses and immunities

Combat proceeds in full rounds until one army is eliminated.

---

## 🧩 Part 1

Determine how many units remain in the winning army after the battle ends.

### 💡 Approach

- Parse both armies into structured `Group` objects
- Each group computes:
  - effective power = units × attack damage
- Simulate combat in rounds:

### Phase 1: Target Selection

- Groups select targets in order of:
  - highest effective power
  - then highest initiative
- Each group chooses the enemy group it can deal the most damage to
- Damage is affected by:
  - weaknesses (×2)
  - immunities (×0)
- Each target can only be chosen once

---

### Phase 2: Attacking

- Groups attack in descending initiative order
- Damage is applied:
  - units killed = floor(damage / hit points per unit)
- Groups losing all units are removed immediately

---

### Combat Loop

Repeat:

- target selection
- attacking

until one side has no remaining groups.

---

## 🧩 Part 2

Find the minimum boost to the Immune System that allows it to win.

### 💡 Approach

- Introduce a boost value to Immune System attack damage
- Re-run full simulation for increasing boost values
- For each simulation:
  - increase immune group attack power by boost
  - run full battle
- Stop when:
  - Infection is fully eliminated
  - Immune System wins

Return:

- remaining Immune System units

---

## 🧠 Code Breakdown

### `Day24.cs`

This is the puzzle entry point.

- Sets title to `Immune System Simulator 20XX`
- Loads input groups
- Runs combat simulation

For Part 1:

- runs battle with base stats
- returns total surviving units of winning side

For Part 2:

- repeatedly runs simulation with increasing boost
- returns first immune victory result

---

### Group Model

Each group stores:

- `Units`
- `HitPoints`
- `AttackDamage`
- `AttackType`
- `Initiative`
- `Weaknesses`
- `Immunities`

Derived values:

- effective power
- potential damage against a target

---

### Target Selection Phase

Groups are sorted by:

1. effective power (descending)
2. initiative (descending)

Each group:

- evaluates all enemy groups
- computes potential damage
- selects best target based on:
  - max damage
  - then enemy effective power
  - then enemy initiative

---

### Damage Calculation

Damage is computed as:

- base = attacking group effective power
- ×2 if defender is weak
- ×0 if defender is immune

Only full unit deaths count:

- units lost = damage / unit HP (floor division)

---

### Attack Phase

- groups attack in initiative order
- dead groups do not get to attack
- casualties are applied immediately
- ordering matters significantly

---

### Stalemate Handling

A key edge case:

- if no units die in a full round
- the battle is considered stuck

Implementation typically stops to avoid infinite loops.

---

### Boost Search (Part 2)

Part 2 is effectively:

- parameter search over attack boost

Strategy:

- start at low boost
- simulate full battle
- increase until immune wins

Optimisation often includes:

- binary search (in some implementations)
- early exit if infection wins decisively

---

## 🛠 Implementation Notes

- This is a deterministic turn-based combat simulator
- Two-phase round system is critical (targeting → attacking)
- Sorting rules dominate correctness
- Damage calculation includes conditional multipliers
- Part 2 is a constrained optimisation problem over simulation
- Stalemate detection is essential for correctness/performance

---

## 🧪 Behaviour Summary

Given two opposing armies:

- groups are parsed into structured combat units
- each round:
  - groups select targets based on damage potential
  - groups attack in initiative order
- units are removed when killed
- battle continues until one army remains
- Part 1 returns surviving unit count of winner
- Part 2 finds smallest boost where Immune System wins

---

## 🚀 Key Takeaways

- Highly rule-driven simulation problem
- Ordering (power, initiative) is critical everywhere
- Two-phase combat loop is the core mechanic
- Weakness/immune modifiers heavily influence strategy
- Part 2 turns simulation into a parameter search problem
- Good example of emergent complexity from simple rules

---

## 🔗 References

- https://adventofcode.com/2018/day/24