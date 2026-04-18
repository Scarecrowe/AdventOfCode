# 🎄 Advent of Code 2020 - Day 16: Ticket Translation

## 📜 Puzzle Overview

This puzzle works with train ticket data split into three sections:

- field rules
- your ticket
- nearby tickets

Each field rule defines two valid numeric ranges, for example:

```text
class: 1-3 or 5-7
```

The solver handles two tasks:

- Part 1 finds invalid values on nearby tickets and totals them
- Part 2 works out which field belongs to which ticket column, then multiplies the values on your ticket whose field names start with `departure`

---

## 🧩 Part 1

Calculate the ticket scanning error rate.

### 💡 Approach

- Read all field rules first
- Expand each rule into a set of valid numbers
- Process nearby tickets only
- For each nearby ticket:
  - check each value against all known rule ranges
  - if a value matches no rule, it is invalid
- Add the first invalid value from each bad ticket to the total
- Return the final error rate

---

## 🧩 Part 2

Determine which field maps to which column, then multiply the `departure` values from your ticket.

### 💡 Approach

- Parse the rule section into structured rule objects
- Build nearby ticket objects
- Discard invalid nearby tickets
- Group values by column index across all valid tickets
- For each column, find which rules could match every value in that column
- Repeatedly resolve columns that have only one possible rule
- Build a final index-to-field-name lookup
- Read your ticket
- Multiply together every value whose resolved field name starts with `departure`

---

## 🧠 Code Breakdown

### `Day16.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Ticket Translation`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

```text
TicketTranslation.ErrorRate(this.Input)
```

For Part 2:

```text
TicketTranslation.Departures(this.Input)
```

---

### `TicketTranslation.cs`

This class contains the main solution logic.

It stores:

- `Rules`

The constructor parses the rule section immediately:

```text
new TicketTranslation(input) => this.Rules = Parse(input)
```

It also contains the main methods for:

- error-rate calculation
- ticket building
- ticket validation
- rule lookup resolution

---

### `TicketMode.cs`

This enum is used while reading the raw input for Part 1.

It has three modes:

- `Fields`
- `YourTicket`
- `NearbyTickets`

This lets the parser switch behaviour as it moves through the three input sections.

---

### `TicketRule.cs`

This class models a single field rule.

It stores:

- `Name`
- `MinA`
- `MaxA`
- `MinB`
- `MaxB`

A rule like:

```text
seat: 13-40 or 45-50
```

is split into:

- field name
- first range
- second range

Validation is handled by:

```text
Validate(int value)
```

which returns true when the value fits either range.

---

### `Ticket.cs`

This class models a single ticket.

It stores:

- `Values`

The constructor takes:

- a comma-separated ticket string
- a value validator delegate

It provides:

- `GetErrorValues()`
- `Validate()`

So a ticket knows how to identify which of its values fail validation.

---

## 🧩 Part 1 Flow

### Rule Parsing

`ErrorRate(string[] input)` reads the input line by line.

While in `Fields` mode, each rule line is split into:

- field name
- two numeric ranges

Unlike Part 2's object-based rules, this method expands every allowed value into a `HashSet<int>` for each field.

So each field name maps to a full set of valid integers.

---

### Section Switching

Blank lines move the parser between modes:

- `Fields` → `YourTicket`
- `YourTicket` → `NearbyTickets`

The `YourTicket` section is ignored completely in Part 1.

Only nearby tickets are checked for invalid values.

---

### Nearby Ticket Validation

For each nearby ticket:

- skip the header line `nearby tickets:`
- split the comma-separated values
- test each value against every field's valid-value set

If a value is not present in any field range:

- add it to the running invalid total
- increment the invalid ticket counter
- stop checking the rest of that ticket

So Part 1 only adds the first invalid value found on each invalid nearby ticket.

---

### Part 1 Return Value

The method returns:

```text
invalid
```

which is the total ticket scanning error rate.

The variable `count` is also incremented for invalid tickets, but it is not used in the final result.

---

## 🧩 Part 2 Flow

### Input Grouping

`Departures(string[] input)` first splits the input into 3 groups:

- rules
- your ticket block
- nearby tickets block

This is done by splitting the full input on blank lines.

Then it creates:

```text
TicketTranslation ticket = new(groups[0].ToArray())
```

So Part 2 parses only the rule block into structured `TicketRule` objects.

---

### Building Tickets

Nearby tickets are built with:

```text
ticket.BuildTickets(groups[2])
```

This method:

- strips the `nearby tickets:` header if present
- converts each ticket line into a `Ticket`

Each `Ticket` is given the validator:

```text
this.ValidateTicket
```

That validator checks whether a value satisfies at least one parsed rule.

---

### Removing Invalid Tickets

Once all nearby tickets are built, the solver keeps only the valid ones:

```text
tickets.Where(t => t.Validate()).ToList()
```

So every ticket used for field resolution contains only values that pass at least one rule.

---

### Column Indexing

`IndexedTicket(List<Ticket> tickets)` reorganises the data by column.

Instead of working ticket-by-ticket, it creates a list where each entry contains:

- all values from column 0
- all values from column 1
- all values from column 2
- and so on

This makes it possible to test one field rule against one full column of values.

---

### Finding Possible Rules Per Column

`PossibleRules(...)` takes:

- the indexed columns
- the currently available rules

For each column it returns all rules where:

- every value in that column passes the rule

So each column gets a list of candidate field rules.

---

### Resolving the Field Mapping

`RuleLookup(List<Ticket> tickets)` repeatedly resolves the rule assignment.

It starts with:

- all indexed columns
- all rules marked as available
- an empty `lookup`
- an empty `assigned` set

Then it loops while rules remain available.

On each pass it:

- builds the possible-rule list for every column
- looks for any unassigned column with exactly one possible rule
- records that rule in the lookup
- removes that rule from the available set
- marks the column as assigned

This continues until every column has been matched to exactly one rule name.

---

### Reading Your Ticket

Your ticket is built from:

```text
groups[1][1]
```

which is the line after the `your ticket:` header.

It is converted into a `Ticket` object in the same way as nearby tickets.

---

### Final Departure Product

After the field lookup is known, the solver finds all column indexes where the resolved field name starts with:

```text
departure
```

It then multiplies those values from your ticket together using:

```text
Aggregate(1L, ...)
```

So the gold answer is the product of all departure-related fields on your own ticket.

---

## 🛠 Implementation Notes

- Part 1 and Part 2 use different parsing styles:
  - Part 1 expands ranges into `HashSet<int>`
  - Part 2 uses `TicketRule` objects with direct range checks
- Part 1 ignores your ticket entirely
- Part 1 stops at the first invalid value per nearby ticket
- Part 2 filters out invalid nearby tickets before resolving fields
- Column-to-rule mapping is solved by repeated elimination
- The final product uses `long` because the multiplied result can grow large

---

## 🧪 Behaviour Summary

Given field rules, your ticket, and nearby tickets:

- Part 1 scans nearby tickets for invalid values
- sums the first invalid value from each bad ticket
- returns the total error rate

For Part 2:

- parse rules into structured validators
- build nearby tickets
- discard invalid tickets
- analyse values by column
- determine which rule belongs to each column
- find fields starting with `departure`
- multiply those values from your ticket

---

## 🚀 Key Takeaways

- Nice split between raw parsing, ticket modelling, and rule validation
- Part 1 uses straightforward invalid-value detection
- Part 2 turns the puzzle into a column-matching deduction problem
- Re-indexing tickets by column keeps the rule matching clean
- Repeated single-option elimination makes the final field mapping simple and readable

---

## 🔗 References

- https://adventofcode.com/2020/day/16