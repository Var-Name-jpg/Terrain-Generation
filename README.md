# Procedural 2D Terrain Generator

This project creates a procedural terrain map using 2D arrays in C\#. It generates a varied landscape by placing random anchor points, then interpolating ("lerping") values based on proximity to these anchors. The final terrain is represented with colored square emojis for easy visualization by users.

***

## Features

- Randomly places anchor points to define terrain regions
- Interpolates terrain values based on proximity to anchors
- Displays terrain using colored square emojis:
    - 🟦 Blue - Water
    - 🟨 Yellow - Sand
    - 🟩 Green - Grass
    - 🟫 Brown - Mountain
    - ⬜ White - Snow

***

## How it works

1. Generates a 2D grid of terrain values.
2. Randomly places anchor points on the grid.
3. Calculates each cell's value by interpolating values relative to the nearest anchor point.
4. Maps value ranges to terrain types.
5. Displays the terrain map with colored square emojis.

***

## Usage

1. Clone or download the repository.
2. Run the `Program.cs` code in your environment.
3. Adjust map size, anchor count, or terrain thresholds in the code to customize the output.
