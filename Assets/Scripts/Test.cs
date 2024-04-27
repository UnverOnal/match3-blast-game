using System;
using UnityEngine;
using Random = System.Random;

public class Test : MonoBehaviour
{
    private void Start()
    {
        Shuffle();
    }

    private void Shuffle()
    {
        // Example 2D array
        var array = new int[,]
        {
            {1, 2, 3, 4},
            { 5, 6, 7, 8},
            {9, 10, 11, 12},
            {13, 14, 15, 16},
            {17, 18, 19, 20},
        };
            
        Shuffle2DArray(array);
    }
        
    static void Shuffle2DArray<T>(T[,] array)
    {
        var rng = new Random();
        var rows = array.GetLength(0);
        var cols = array.GetLength(1);

        for (var i = rows - 1; i > 0; i--)
        {
            for (var j = cols - 1; j > 0; j--)
            {
                var m = rng.Next(i + 1);
                var n = rng.Next(j + 1);

                var cell = array[i, j];
                var cellToReplace = array[m, n];

                array[i, j] = cellToReplace;
                array[m, n] = cell;
            }
        }
    }
}