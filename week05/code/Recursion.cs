using System;
using System.Collections.Generic;

public static class Recursion
{
    /// <summary>
    /// #############
    /// # Problem 1 #
    /// #############
    /// Using recursion, find the sum of 1^2 + 2^2 + 3^2 + ... + n^2
    /// and return it. If the value of n <= 0, just return 0.
    /// </summary>
    public static int SumSquaresRecursive(int n)
    {
        // Base case: if n <= 0, return 0
        if (n <= 0)
        {
            return 0;
        }
        
        // Recursive case: n^2 + SumSquaresRecursive(n-1)
        return n * n + SumSquaresRecursive(n - 1);
    }

    /// <summary>
    /// #############
    /// # Problem 2 #
    /// #############
    /// Using recursion, insert permutations of length
    /// 'size' from a list of 'letters' into the results list.
    /// </summary>
    public static void PermutationsChoose(List<string> results, string letters, int size, string word = "")
    {
        if (word.Length == size)
        {
            results.Add(word);
            return;
        }

        for (int i = 0; i < letters.Length; i++)
        {
            PermutationsChoose(results, letters.Remove(i, 1), size, word + letters[i]);
        }
    }

    /// <summary>
    /// #############
    /// # Problem 3 #
    /// #############
    /// Count how many ways there are to climb the stairs.
    /// This function uses memoization.
    /// </summary>
    public static decimal CountWaysToClimb(int s, Dictionary<int, decimal>? remember = null)
    {
        // Initialize the dictionary for memoization
        remember ??= new Dictionary<int, decimal>();

        // Base Cases
        if (s < 0) return 0;
        if (s == 0) return 1;

        // Check if already computed
        if (remember.ContainsKey(s))
        {
            return remember[s];
        }

        // Solve using recursion
        decimal ways = CountWaysToClimb(s - 1, remember) + 
                       CountWaysToClimb(s - 2, remember) + 
                       CountWaysToClimb(s - 3, remember);
        
        // Store the result in the dictionary
        remember[s] = ways;

        return ways;
    }

    /// <summary>
    /// #############
    /// # Problem 4 #
    /// #############
    /// Using recursion, insert all possible binary strings for a given pattern into the results list.
    /// </summary>
    public static void WildcardBinary(string pattern, List<string> results)
    {
        GeneratePatterns(pattern, "", results);
    }
    
    private static void GeneratePatterns(string pattern, string current, List<string> results)
    {
        if (pattern.Length == 0)
        {
            results.Add(current);
            return;
        }

        char firstChar = pattern[0];
        string remainingPattern = pattern.Substring(1);

        if (firstChar == '*')
        {
            // Replace '*' with '0' and '1'
            GeneratePatterns(remainingPattern, current + '0', results);
            GeneratePatterns(remainingPattern, current + '1', results);
        }
        else
        {
            GeneratePatterns(remainingPattern, current + firstChar, results);
        }
    }

    /// <summary>
    /// Use recursion to insert all paths that start at (0,0) and end at the
    /// 'end' square into the results list.
    /// </summary>
    public static void SolveMaze(List<string> results, Maze maze, int x = 0, int y = 0, List<ValueTuple<int, int>>? currPath = null)
    {
        if (currPath == null) {
            currPath = new List<ValueTuple<int, int>>();
        }

        // Check if the move is valid
        if (!maze.IsValidMove(x, y, currPath))
            return;

        currPath.Add((x, y));

        if (maze.IsEnd(x, y))
        {
            results.Add(currPath.AsString());
        }
        else
        {
            // Explore all directions
            SolveMaze(results, maze, x + 1, y, currPath); // Down
            SolveMaze(results, maze, x - 1, y, currPath); // Up
            SolveMaze(results, maze, x, y + 1, currPath); // Right
            SolveMaze(results, maze, x, y - 1, currPath); // Left
        }

        currPath.RemoveAt(currPath.Count - 1); // Backtrack
    }
}