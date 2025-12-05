# AdventOfCode.Puzzles.2015

## Overview

This project contains my **Advent of Code 2015** puzzle solutions, implemented in **C#**.  
All 25 days of the 2015 event are fully completed, with each puzzle organised into its own class/day structure.

The 2015 puzzles were the first official year of Advent of Code and include a range of logic, parsing and simulation challenges.  
This library integrates seamlessly with the main runner and core utilities used across all years.

---

## Structure

Each puzzle is structured following the standard pattern used across this repository:

- **DayXX** directory or class (e.g., `Day01`, `Day02`, … `Day25`)  
- Each day implements:
  - **Part 1** (“Silver”) solution  
  - **Part 2** (“Gold”) solution  
- Optional helpers or mini-structures specific to that puzzle  

The library follows a clean separation:

- **Puzzle logic** lives here  
- **Input handling & shared utilities** live in `AdventOfCode.Core`  
- **Execution & CLI** handled by `AdventOfCode.Runner`

---

## How Puzzles Are Executed

These puzzles are not run directly.  
Instead, they are loaded and executed via the main **Runner** project.

Example:

```
-year 2015 -day 8 -batch 1
```

The runner will:

1. Load the `2015` puzzle set  
2. Instantiate `Day08`  
3. Load the correct batch of input  
4. Execute both solutions  
5. Output results and timing

---

## Development Notes

- Each puzzle is implemented as a self-contained class to keep logic isolated.  
- Shared utilities (parsing, grids, maths, coordinate systems, etc.) come from `Core`.  
- The project may include:
  - small helper classes,
  - reusable private methods,
  - or data models specific to individual days.

---

## Example Puzzle Class Layout

```csharp
public class Day05 : IPuzzle
{
    public string SolveSilver(string[] input)
    {
        // Part 1 logic
    }

    public string SolveGold(string[] input)
    {
        // Part 2 logic
    }
}
```

*(Adjust IPuzzle / interface names to match your exact structure.)*

---

## Completed Days

All 25 days for 2015 are complete:

| Day | Status |
|-----|--------|
| 01–25 | ✔️ Completed |

---

## Adding Improvements

Even completed years can be enhanced with:

- Performance optimisations  
- Cleaner implementations  
- More readable code  
- Optional animations for visual puzzles  
- Additional batch inputs  

If improvements are made, this README can be updated to reflect them.

---

## License

This project follows the root repository MIT License.  
See `license.txt` in the main repo.

