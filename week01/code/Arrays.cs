using System;
using System.Collections.Generic;

public static class Arrays
{
    /// <summary>
    /// This function will produce an array of size 'length' starting with 'number' followed by multiples of 'number'.
    /// For example, MultiplesOf(7, 5) -> {7, 14, 21, 28, 35, 42}.
    /// Assume that length is a positive integer greater than 0.
    /// </summary>
    /// <returns>array of doubles that are the multiples of the supplied number</returns>
    public static double[] MultiplesOf(double number, int length)
    {
        // PLAN / PROCESS:
        // 1) Ensure we will create an array with exactly 'length' elements.
        // 2) Each element should be the (i+1)-th multiple of 'number' (i.e., number * (i + 1)).
        // 3) Allocate a double[] of size 'length'.
        // 4) Loop i from 0 to length-1:
        //      - compute number * (i + 1)
        //      - store the result in result[i]
        // 5) Return the filled array.
        //
        // Note: The assignment guarantees length > 0. If you'd like defensive behavior,
        // you could uncomment the guard below to throw an exception for invalid lengths.
        // if (length <= 0) throw new ArgumentOutOfRangeException(nameof(length), "length must be > 0.");

        double[] result = new double[length];

        for (int i = 0; i < length; i++)
        {
            // i = 0 -> first element: number * 1
            // i = 1 -> second element: number * 2
            result[i] = number * (i + 1);
        }

        return result;
    }

    /// <summary>
    /// Rotate the 'data' to the right by the 'amount'.  For example, 
    /// data = {1,2,3,4,5,6,7,8,9}, amount = 3  ->  {7,8,9,1,2,3,4,5,6}
    /// The value of amount will be in the range 1..data.Count inclusive.
    /// This modifies the input list in place.
    /// </summary>
    public static void RotateListRight(List<int> data, int amount)
    {
        // PLAN / PROCESS:
        // 1) Validate inputs: if data is null -> throw; if data has 0 or 1 element -> nothing to do.
        // 2) Normalize amount by using modulo with data.Count:
        //      amount = amount % data.Count
        //    If the normalized amount is 0, the list remains unchanged -> return.
        // 3) Use list slicing:
        //      - tail = the last 'amount' elements (data.GetRange(data.Count - amount, amount))
        //      - remove those last 'amount' elements from 'data' (data.RemoveRange(...))
        //      - insert the saved tail at the beginning (data.InsertRange(0, tail))
        // 4) The input list 'data' is now rotated right by 'amount' and modified in place.
        //
        // Example:
        // data = {1,2,3,4,5,6,7,8,9}, amount = 3
        // tail = {7,8,9}
        // after RemoveRange -> {1,2,3,4,5,6}
        // after InsertRange(0, tail) -> {7,8,9,1,2,3,4,5,6}

        // Step 1: input validation
        if (data == null) throw new ArgumentNullException(nameof(data));
        int n = data.Count;
        if (n <= 1) return; // nothing to rotate

        // Step 2: normalize amount (guard against amount equal to data.Count)
        int k = amount % n;
        if (k == 0) return; // rotating by a full length results in original list

        // Step 3: slice the last k elements
        List<int> tail = data.GetRange(n - k, k);

        // Step 4: remove them from the end of the original list
        data.RemoveRange(n - k, k);

        // Step 5: insert the tail at the beginning
        data.InsertRange(0, tail);

        // Done — 'data' is rotated in place.
    }
}
