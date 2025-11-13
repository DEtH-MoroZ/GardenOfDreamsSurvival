using System;
using System.Collections.Generic;
using UnityEngine;

public static class GameObjectGridHelpers //chat gpt ftw. nothing works but it was soooo beautiful :,|
{
    //converts world coords to grid coords
    public static int WorldToGridIndices(float value, float gridSideLength, float gridStep)
    {
        return Mathf.RoundToInt((value + (gridSideLength / 2f)) / gridStep);
    }


    // Returns first non-empty List<GameObject> found in spiral order starting at (startRow, startCol),
    // or null if none found.
    public static List<GameObject> FindNearestGameobjects(List<GameObject>[][] grid, int startRow, int startCol)
    {
        if (grid == null) return null;
        int rows = grid.Length;
        if (rows == 0) return null;
        int cols = grid[0]?.Length ?? 0;
        if (cols == 0) return null;

        // Validate start position
        if (startRow < 0 || startRow >= rows || startCol < 0 || startCol >= cols)
            throw new ArgumentOutOfRangeException("Start position is outside the grid.");

        // Check starting cell first
        if (grid[startRow][startCol] != null && grid[startRow][startCol].Count > 0)
            return grid[startRow][startCol];

        // Directions: right, down, left, up
        int[,] dir = new int[,] { { 0, 1 }, { 1, 0 }, { 0, -1 }, { -1, 0 } };

        int r = startRow;
        int c = startCol;
        int stepSize = 1;     // how many steps to take in current leg
        int dirIndex = 0;     // 0..3
        int cellsVisited = 1; // we already checked the start
        int totalCells = rows * cols;

        while (cellsVisited < totalCells)
        {
            // Repeat two legs with the same stepSize (except maybe last partial)
            for (int repeat = 0; repeat < 2; repeat++)
            {
                int dr = dir[dirIndex, 0];
                int dc = dir[dirIndex, 1];

                for (int step = 0; step < stepSize; step++)
                {
                    r += dr;
                    c += dc;

                    // If outside bounds, just skip counting as visited; only count valid cells
                    if (r >= 0 && r < rows && c >= 0 && c < cols)
                    {
                        
                        //if go is inactive -> remove from grid
                        for (int l = grid[r][c].Count-1; l >= 0; l--) {
                            if (grid[r][c][l].gameObject.activeSelf == false)
                            {
                                grid[r][c].RemoveAt(l);
                            }
                        }
                        cellsVisited++;

                        var cell = grid[r][c];
                        if (cell != null && cell.Count > 0)
                        {
                            return cell;
                        }

                        // If we've visited all valid cells we can stop
                        if (cellsVisited >= totalCells)
                            return null;
                    }

                    // If we have visited all cells physically possible, break early
                    // (This check avoids infinite loop when grid is rectangular and spiral steps go beyond)
                    if (cellsVisited >= totalCells)
                        break;
                }

                dirIndex = (dirIndex + 1) % 4;
            }

            stepSize++;
        }

        return null;
    }
}