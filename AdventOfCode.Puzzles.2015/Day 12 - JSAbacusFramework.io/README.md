# 🎄 Advent of Code 2015 - Day 12: JSAbacusFramework.io

## 📜 Puzzle Overview

Santa's accounting system stores its data as JSON.

The input can contain:

- Objects
- Arrays
- Numbers
- Strings

Part 1 asks for the sum of **all numbers** found anywhere in the JSON document.

Part 2 changes the rules for objects:

- If any property in an object has the value `"red"`
- That entire object is ignored
- Arrays containing `"red"` are **not** ignored unless they contain an object that is ignored

---

## 🧩 Part 1

Determine the sum of all numbers in the JSON document.

### 💡 Approach

- Read the JSON as a raw string
- Scan character by character
- Build number tokens when digits or a minus sign are found
- Convert completed number tokens into integers
- Add each value to a running total

This avoids fully parsing the JSON for Part 1.

---

## 🧩 Part 2

Determine the sum of all numbers in the JSON document, ignoring any object that contains the value `"red"`.

### 💡 Approach

- Parse the JSON into a structured object model
- Walk the JSON recursively
- Sum integers from arrays, objects, and values
- Skip any object where one of its property values is `"red"`

This allows the structure of the JSON to be respected while applying the exclusion rule.

---

## 🧠 Code Breakdown

### `Day12.cs`

This is the puzzle entry point.

- Sets the puzzle title
- Loads the puzzle input
- Calls the silver and gold solutions

For Part 1:

- Calls `JSAbacusFramework.SumAll(this.Input[0])`

For Part 2:

- Calls `JSAbacusFramework.SumNonRed(this.Input[0])`

Both answers are returned as strings.

---

### `JSAbacusFramework.cs`

This class contains the full solution for both parts.

It exposes two public methods:

- `SumAll(string json)`
- `SumNonRed(string json)`

Part 1 uses direct string scanning.

Part 2 uses JSON parsing and recursive traversal.

---

### Part 1: Summing All Numbers

`SumAll()` processes the raw JSON text without deserializing it.

It uses:

- `result` to store the running total
- `StringBuilder sb` to collect numeric characters

As the input is scanned:

- `-` and numeric characters are appended to the builder
- Any non-numeric separator ends the current number
- Completed numbers are converted and added to the total
- The builder is then cleared for the next value

At a high level, the logic works like this:

    if character is '-' or digit
        append to current number
    else if current number exists
        convert and add to result
        clear builder

This works because the puzzle guarantees that strings do not contain embedded numbers.

---

### Part 2: Ignoring Red Objects

`SumNonRed()` parses the JSON into a dynamic object using `JsonConvert.DeserializeObject()`.

Once parsed, it calls:

    GetSum(jsonObject, "red")

This starts a recursive walk through the JSON structure while excluding objects that contain the value `"red"`.

---

### Recursive Summing

The recursive logic is split into overloaded `GetSum()` methods for:

- `JObject`
- `JArray`
- `JValue`

This allows each JSON type to be handled according to its structure.

---

### Object Handling

For `JObject` values:

- All property values are inspected
- If any property value is `"red"`, the object contributes `0`
- Otherwise, the sum is calculated recursively across all property values

At a high level:

    if object contains value "red"
        return 0
    else
        sum all child values

Only objects are excluded this way.

---

### Array Handling

For `JArray` values:

- Every child element is processed recursively
- No special `"red"` filtering is applied to the array itself

This means arrays still contribute normally unless they contain an ignored object somewhere inside them.

---

### Value Handling

For `JValue` values:

- If the token is an integer, its numeric value is returned
- Otherwise, it contributes `0`

This filters out strings and other non-numeric values automatically.

---

## 🛠 Implementation Notes

- Part 1 avoids JSON parsing completely and uses direct character scanning
- Part 2 uses `Newtonsoft.Json` for structured traversal
- Recursive overloads keep handling clean across objects, arrays, and scalar values
- The `"red"` rule is applied only to objects, matching the puzzle requirements
- Both parts are kept separate, which makes each solution easy to follow

---

## 🧪 Examples

### Part 1

| Input | Result |
|-------|--------|
| `[1,2,3]` | `6` |
| `{"a":2,"b":4}` | `6` |
| `[[[3]]]` | `3` |
| `{"a":{"b":4},"c":-1}` | `3` |
| `{"a":[-1,1]}` | `0` |
| `[-1,{"a":1}]` | `0` |
| `[]` | `0` |
| `{}` | `0` |

### Part 2

| Input | Result |
|-------|--------|
| `[1,2,3]` | `6` |
| `[1,{"c":"red","b":2},3]` | `4` |
| `{"d":"red","e":[1,2,3,4],"f":5}` | `0` |
| `[1,"red",5]` | `6` |

---

## 🚀 Key Takeaways

- Good example of using two different strategies for two related parts
- Part 1 stays lightweight by scanning the raw text directly
- Part 2 switches to structured parsing so object-level rules can be enforced cleanly
- Recursive overloads make the JSON traversal easy to read and maintain

---

## 🔗 References

- https://adventofcode.com/2015/day/12