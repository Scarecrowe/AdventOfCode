# AdventOfCode.Core

## What is this

The `AdventOfCode.Core` library contains shared utilities and common functionality used across multiple puzzle implementations.  
It’s designed to reduce duplication and provide a consistent foundation for parsing input, handling data transformations, and common helper routines — for all years of puzzles.

## What’s inside

This library may include (but is not limited to):

- Input parsing utilities (reading puzzle input files, splitting, trimming, converting to numbers, etc.)  
- Common data structures / helpers (e.g. 2D-grid handling, coordinate manipulation, general collections, etc.)  
- Logging / debugging helpers (if any)  
- Shared constants or config handling (e.g. file paths, batch-input indexing, etc.)  
- Utility functions used across puzzles (e.g. string parsing, date/time helpers, math helpers, etc.)

## How to use

1. Add a project reference to `AdventOfCode.Core` from your puzzle project.  
2. Use the exposed helper methods for input reading, parsing or common routines rather than rewriting for each puzzle.  
3. Keep shared, generic code here so that puzzle-specific logic remains isolated.

## Why this separation helps

- Avoids code duplication across puzzle projects — once a utility is written, all puzzles can benefit.  
- Simplifies maintenance: bug fixes / improvements in one place.  
- Encourages cleaner code in puzzle-specific projects (they only handle puzzle logic, not setup/parsing).  
- Facilitates testing of core utilities in isolation (via the shared test project, if applicable).

## Contributing / Extending

If you want to add a new helper to `Core`:

- Keep it generic — ensure it doesn’t contain puzzle-specific logic.  
- Write unit tests (in your test project) to validate functionality.  
- Ensure naming and documentation are clear — utilities are meant to be reused across years.  

## Example usage

```csharp
// Example: reading input file for year/day
var inputLines = InputReader.ReadLines(year: 2025, day: 5, batch: 1);

// Example: parsing lines to ints
var numbers = inputLines.Select(CoreHelpers.ParseInt).ToList();

// Example: using a 2D grid helper
var grid = GridHelper.FromLines(inputLines);
var result = grid.Traverse(...);
```

## License

This library is covered under the same MIT License as the rest of the project.  
See `license.txt` in root for full details.
