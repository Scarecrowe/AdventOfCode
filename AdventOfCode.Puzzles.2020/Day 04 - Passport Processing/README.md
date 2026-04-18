# 🎄 Advent of Code 2020 - Day 4: Passport Processing

## 📜 Puzzle Overview

This puzzle processes a batch of passport records.

Each passport is made up of key-value fields such as:

```text
ecl:gry pid:860033327 eyr:2020 hcl:#fffffd
byr:1937 iyr:2017 cid:147 hgt:183cm
```

Passports can span multiple lines, and blank lines separate one passport from the next.

The solver supports two validation modes:

- Part 1 checks whether all required fields are present
- Part 2 checks whether all required fields are present and also valid

The optional field:

- `cid`

is ignored in both parts.

---

## 🧩 Part 1

Count how many passports contain all required fields.

### 💡 Approach

- Read passport data until a blank line is reached
- Merge each passport into one space-separated string
- Split the passport into individual `key:value` fields
- Ignore `cid`
- Collect the remaining field names
- Count the passport if all required field keys are present

---

## 🧩 Part 2

Count how many passports contain all required fields with valid values.

### 💡 Approach

- Reuse the same passport grouping logic
- Ignore `cid`
- Validate each field value using dedicated rules
- Keep only fields whose values pass validation
- Count the passport if all required fields remain after validation

---

## 🧠 Code Breakdown

### `Day4.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Passport Processing`
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

```text
PassportProcessing.Simple(this.Input)
```

For Part 2:

```text
PassportProcessing.Advanced(this.Input)
```

---

### `PassportProcessing.cs`

This class contains the full passport parsing and validation logic.

It defines:

- `Valid`
- `HairColours`
- `Validate`

`Valid` contains the required field keys:

- `byr`
- `iyr`
- `eyr`
- `hgt`
- `hcl`
- `ecl`
- `pid`

`Validate` maps each field key to its corresponding validation method.

---

### Required Fields

The required fields are:

```text
byr iyr eyr hgt hcl ecl pid
```

The field:

```text
cid
```

is explicitly ignored and is not required for a passport to count as valid.

---

### Main Processing Flow

Both puzzle parts call:

```text
Run(string[] input, Func<string[], string[]> fields)
```

This method:

- loops through every input line
- appends non-empty lines to the current passport string
- when it finds a blank line:
  - splits the accumulated passport into fields
  - applies either basic or advanced field extraction
  - checks whether all required keys are present
  - increments the result if valid
- resets the passport buffer and continues

So both parts share the same overall parsing flow, with only the field-filtering logic changing.

---

### Passport Accumulation

Each passport is built into a single string.

At a high level:

- non-empty lines are appended with a trailing space
- blank lines mark the end of one passport

This means a multi-line passport becomes one flat space-separated record before being analysed.

---

### Basic Field Extraction

Part 1 uses:

```text
Fields(string[] fields)
```

This method:

- splits each `key:value` field
- removes any field where the key is `cid`
- returns just the remaining field names

This allows the solver to check presence only, without caring about the actual values.

---

### Advanced Field Extraction

Part 2 uses:

```text
ValidFields(string[] fields)
```

This method:

- splits each `key:value` field
- removes `cid`
- validates each remaining field value using the `Validate` dictionary
- returns only the field names whose values passed validation

This means Part 2 only counts a passport when every required field both exists and passes its rule.

---

### Final Validity Check

After field extraction, the solver checks validity with the intersection count against the required field list.

At a high level it does this:

- compare returned field names against the required list
- if the number of matching required fields equals the total required field count
- count the passport as valid

So validity is based on having all 7 required fields after filtering.

---

## 🛠 Field Validation Rules

### `byr` - Birth Year

Validated by:

```text
ValidateBirthYear(string value)
```

Rules:

- must be exactly 4 characters
- must be between `1920` and `2002`

---

### `iyr` - Issue Year

Validated by:

```text
ValidateIssueYear(string value)
```

Rules:

- must be exactly 4 characters
- must be between `2010` and `2020`

---

### `eyr` - Expiration Year

Validated by:

```text
ValidateExpirationYear(string value)
```

Rules:

- must be exactly 4 characters
- must be between `2020` and `2030`

---

### `hgt` - Height

Validated by:

```text
ValidateHeight(string value)
```

Rules:

- value must end in either `cm` or `in`
- if `cm`, number must be between `150` and `193`
- if `in`, number must be between `59` and `76`

If the string is too short or has an invalid suffix, it fails immediately.

---

### `hcl` - Hair Colour

Validated by:

```text
ValidateHairColor(string value)
```

Rules:

- must start with `#`
- must have length `7`
- remaining characters must be:
  - digits `0-9`
  - lowercase letters `a-f`

The method checks each character manually.

---

### `ecl` - Eye Colour

Validated by:

```text
ValidateEyeColour(string value)
```

Accepted values are:

- `amb`
- `blu`
- `brn`
- `gry`
- `grn`
- `hzl`
- `oth`

---

### `pid` - Passport ID

Validated by:

```text
ValidatePassportID(string value)
```

Rules:

- must be exactly 9 characters
- every character must be numeric

---

## 🧪 Behaviour Summary

Given a batch of passport records:

- group lines into individual passports
- flatten each passport into a single string
- split into `key:value` pairs
- ignore `cid`
- Part 1 checks that all required fields exist
- Part 2 checks that all required fields exist and pass strict validation
- count the number of valid passports

---

## 🚀 Key Takeaways

- Clean shared pipeline for both puzzle parts
- Part 1 and Part 2 differ only in how fields are filtered
- Validation is neatly separated into dedicated methods
- `cid` is ignored throughout
- The solution uses required-field intersection to determine final validity

---

## 🔗 References

- https://adventofcode.com/2020/day/4