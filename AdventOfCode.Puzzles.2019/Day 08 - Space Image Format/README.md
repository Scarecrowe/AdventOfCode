# 🎄 Advent of Code 2019 - Day 08: Space Image Format

## 📜 Puzzle Overview

This puzzle decodes an image encoded as a long string of digits.

Each digit represents a pixel colour:

- `0` = black
- `1` = white
- `2` = transparent

The input is a single continuous image string which must be split into layers of fixed size.

For this implementation, the image dimensions are:

- width = `25`
- height = `6`

So each layer contains:

    25 × 6 = 150

pixels.

Part 1 analyses the layers to find a checksum. Part 2 stacks the layers together to reveal the final image.

---

## 🧩 Part 1

Find the layer with the fewest `0` digits, then return:

- number of `1` digits
- multiplied by number of `2` digits

### 💡 Approach

- Split the image into layers
- Count how many `0` digits appear in each layer
- Select the layer with the lowest zero count
- Count the `1` digits in that layer
- Count the `2` digits in that layer
- Multiply those two counts together

---

## 🧩 Part 2

Decode the final image by stacking the layers from top to bottom.

### 💡 Approach

- Create an output image buffer
- For each pixel position:
  - inspect that position in layer order
  - skip transparent pixels (`2`)
  - take the first non-transparent pixel
- Store the visible pixel in the final message
- Render the decoded image using:
  - space for black
  - `#` for white

---

## 🧠 Code Breakdown

### `Day8.cs`

This is the puzzle entry point.

- Sets the puzzle title to `Space Image Format`
- Loads the puzzle input
- Creates `SpaceImageFormat` using:
  - the first input line
  - width `25`
  - height `6`

For Part 1:

- Calls `FewestDigits()`

For Part 2:

- Calls `Decode()`
- Then calls `Print()`

So the puzzle is solved with:

- `new SpaceImageFormat(this.Input[0], 25, 6).FewestDigits()`
- `new SpaceImageFormat(this.Input[0], 25, 6).Decode().Print()`

---

### `PixelColour.cs`

This enum defines the pixel values used during decoding.

It maps characters directly to colour meanings:

- `Black = '0'`
- `White = '1'`
- `Transparent = '2'`

This lets the decode logic cast image characters into meaningful pixel states.

---

### `SpaceImageFormat.cs`

This class contains the image parsing, analysis, decoding, and printing logic.

It stores:

- `Image`
- `Layers`
- `Message`
- `Width`
- `Height`

The constructor:

- stores the raw image string
- stores width and height
- initialises the layer collection
- initialises the message buffer
- calls `ParseLayers()`

---

### Layer Storage

Layers are stored as a list of rows.

Conceptually that means:

- a list of layers
- each layer is a list of strings
- each string is one image row

So each layer is built as:

- `Height` rows
- each row of length `Width`

---

### Parsing the Layers

`ParseLayers()` reads the image string from left to right.

It uses an index into the full image string and repeatedly:

- creates a new layer
- takes `Height` rows
- each row is:

      this.Image.Substring(index, this.Width)

- advances the index by `Width` each time

This continues until the whole input image has been divided into layers.

---

### Part 1 Checksum Logic

`FewestDigits()` finds the layer with the smallest number of `0` digits.

It uses an aggregate comparison across all layers, counting zeroes with flattened row data.

Once the best layer is found, it returns:

- count of `1`
- multiplied by count of `2`

Logically:

    ones * twos

This is the silver answer.

---

### Decoding the Image

`Decode()` creates the final visible image.

It first allocates:

- `new char[this.Height, this.Width]`

Then it loops over every pixel coordinate using:

- `Vector.AxisEnumerator(this.Width, this.Height)`

For each coordinate:

- scan layers from first to last
- read the pixel at that position
- cast it to `PixelColour`
- if the pixel is transparent:
  - continue to the next layer
- otherwise:
  - store that pixel in `Message`
  - stop scanning that coordinate

So the first non-transparent pixel wins.

---

### Pixel Visibility Rules

Each final pixel is resolved like this:

- `2` means invisible, keep looking
- `0` means black
- `1` means white

This matches the puzzle's stacked image behaviour.

---

### Printing the Decoded Image

`Print()` turns the decoded `Message` array into display text.

It builds the result with a `StringBuilder`.

For each pixel:

- if the stored value is `'0'`, print a space
- otherwise print `#`

So the visual output becomes a text image where:

- black = blank space
- white = `#`

The method wraps the image with blank lines and returns the full rendered string.

---

### Part 1 Return Value

Part 1 returns:

- the checksum from the layer with the fewest zeroes

This is calculated in:

- `FewestDigits()`

---

### Part 2 Return Value

Part 2 returns:

- the rendered decoded image as text

This is produced by:

- `Decode()`
- followed by `Print()`

---

## 🛠 Implementation Notes

- The input is a single image string
- Layers are parsed using fixed width and height
- Each layer is stored as rows of strings
- Part 1 uses digit counting across layers
- Part 2 scans pixel positions front-to-back through the layer stack
- Transparent pixels are skipped until a visible pixel is found
- The final image is rendered using spaces and `#`

---

## 🧪 Behaviour Summary

Given one encoded image string:

- the solver splits it into equally sized layers
- Part 1 searches for the layer with the fewest zeroes
- it multiplies the count of ones by the count of twos in that layer
- Part 2 overlays layers pixel by pixel
- transparent pixels are ignored until a visible value is found
- the final message is printed as ASCII-style image output

---

## 🚀 Key Takeaways

- Nice example of fixed-size chunk parsing
- Layers are represented in a simple row-based structure
- Part 1 is a layer analysis problem
- Part 2 is an image compositing problem
- Using an enum makes pixel meaning much clearer
- The final rendering converts raw pixel values into readable output

---

## 🔗 References

- https://adventofcode.com/2019/day/8