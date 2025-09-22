using System;
using System.Collections.Generic;

public class Node
{
    public int Value;
    public Node Left;
    public Node Right;

    public Node(int value)
    {
        Value = value;
        Left = null;
        Right = null;
    }

    // Problem 1: Insert Unique Values Only
    public void Insert(int value)
    {
        if (value < Value)
        {
            if (Left == null)
                Left = new Node(value);
            else
                Left.Insert(value);
        }
        else if (value > Value) // Ensure only unique values are added
        {
            if (Right == null)
                Right = new Node(value);
            else
                Right.Insert(value);
        }
    }

    // Problem 4: Tree Height
    public int GetHeight()
    {
        if (this == null) return 0; // Base case: height of an empty node is 0
        int leftHeight = Left?.GetHeight() ?? 0;
        int rightHeight = Right?.GetHeight() ?? 0;
        return 1 + Math.Max(leftHeight, rightHeight);
    }

    // Problem 2: Contains
    public bool Contains(int value)
    {
        if (value == Value) return true;
        if (value < Value)
            return Left?.Contains(value) ?? false;
        return Right?.Contains(value) ?? false;
    }
}

public class BinarySearchTree
{
    public Node Root;

    public BinarySearchTree()
    {
        Root = null;
    }

    public void Insert(int value)
    {
        if (Root == null)
            Root = new Node(value);
        else
            Root.Insert(value);
    }

    // Problem 3: Traverse Backwards
    public IEnumerable<int> Reversed()
    {
        return TraverseBackward(Root);
    }

    private IEnumerable<int> TraverseBackward(Node node)
    {
        if (node == null) yield break;
        foreach (var value in TraverseBackward(node.Right))
            yield return value;
        yield return node.Value;
        foreach (var value in TraverseBackward(node.Left))
            yield return value;
    }
}

public static class Trees
{
    /// <summary>
    /// Given a sorted list (sorted_list), create a balanced BST.
    /// </summary>
    public static BinarySearchTree CreateTreeFromSortedList(int[] sortedNumbers)
    {
        var bst = new BinarySearchTree(); // Create an empty BST to start with 
        InsertMiddle(sortedNumbers, 0, sortedNumbers.Length - 1, bst);
        return bst;
    }

    /// <summary>
    /// This function will attempt to insert the item in the middle of 'sortedNumbers' into
    /// the 'bst' tree.
    /// </summary>
    private static void InsertMiddle(int[] sortedNumbers, int first, int last, BinarySearchTree bst)
    {
        if (first > last) return; // Base case: no elements to insert

        int mid = (first + last) / 2; // Find the middle index
        bst.Insert(sortedNumbers[mid]); // Insert the middle value

        InsertMiddle(sortedNumbers, first, mid - 1, bst); // Insert left half
        InsertMiddle(sortedNumbers, mid + 1, last, bst); // Insert right half
    }
}