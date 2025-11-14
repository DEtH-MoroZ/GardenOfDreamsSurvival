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
    public static List<GameObject> FindNearestGameobject(List<GameObject>[][] grid, int startRow, int startCol)
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
    public static List<GameObject> FindNearestInRadius(List<GameObject>[][] grid, int startRow, int startCol, int radius)
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

                    if (r - startRow > radius) return null; // <------------------------------------------ this is the difference, if check exeed radius => stop execution
                    if (r - startCol > radius) return null;

                    // If outside bounds, just skip counting as visited; only count valid cells
                    if (r >= 0 && r < rows && c >= 0 && c < cols)
                    {

                        //if go is inactive -> remove from grid
                        for (int l = grid[r][c].Count - 1; l >= 0; l--)
                        {
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

    public static GameObject FindNearestInRadiusAndLayer(List<GameObject>[][] grid, int startRow, int startCol, int radius, LayerMask layerMask)
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
        {
            for (int a = 0; a < grid[startRow][startCol].Count; a++)
            {
                if (LayerMaskExtensions.Contains(layerMask, grid[startRow][startCol][a]) == true)
                {
                    return grid[startRow][startCol][a];
                }
            }

        }
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

                    if (r - startRow > radius+1) return null; // <------------------------------------------ this is the difference, if check exeed radius => stop execution
                    if (c - startCol > radius+1) return null; // + needed, cos objects not allways detected, when they are inside radius

                    // If outside bounds, just skip counting as visited; only count valid cells
                    if (r >= 0 && r < rows && c >= 0 && c < cols)
                    {

                        //if go is inactive -> remove from grid
                        for (int l = grid[r][c].Count - 1; l >= 0; l--)
                        {
                            
                            if (grid[r][c][l].gameObject.activeSelf == false)
                            {
                                grid[r][c].RemoveAt(l);
                            }
                        }
                        cellsVisited++;

                        var cell = grid[r][c];
                        if (cell != null && cell.Count > 0)
                        {
                            for (int i = 0; i < cell.Count; i++) {
                                if (LayerMaskExtensions.Contains(layerMask, cell[i]) == true)
                                {
                                    return cell[i];
                                }
                            }
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

    public static int FindDistance (int Ax, int Ay, int Bx, int By) //should be replaced with lookup table for (0)1
    {
        int dx = Bx - Ax, dy = By - Ay;
        if (dx == 0 && dy == 0) return 0;

        uint squared = (uint)(dx * dx + dy * dy);
        uint x = squared;

        // Fast integer square root using bit manipulation
        uint bit = 1u << 30;
        while (bit > squared) bit >>= 2;

        uint result = 0;
        while (bit != 0)
        {
            if (squared >= result + bit)
            {
                squared -= result + bit;
                result = (result >> 1) + bit;
            }
            else
            {
                result >>= 1;
            }
            bit >>= 2;
        }

        // Round up if closer to next integer
        return (int)((squared > result) ? result + 1 : result);
    }
}