﻿using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using MergeSort;

/*
 * Note: this implementation is not mine, just benchmarking
 * Understanding how to write my own from scratch is on the TODO list
 */
var a = new Benchmark();
a.PrintArray();
a.MergeSort();
a.PrintArray();
Console.WriteLine();

BenchmarkRunner.Run<Benchmark>();

namespace MergeSort
{
    [MemoryDiagnoser]
    public class Benchmark
    {
        private int[] _array;

        public Benchmark()
        {
            var rand = new Random();
            var tempList = new List<int>();
            for (var i = 1; i <= 100; i++)
            {
                var x = rand.Next(1, 100);
                tempList.Add(x);
            }

            _array = tempList.ToArray();
        }

        public void PrintArray()
        {
            Console.WriteLine("[{0}]", string.Join(", ", _array));
        }

        [Benchmark]
        public void MergeSort()
        {
            _array = Sort.MergeSort(_array);
        }
    }

    public static class Sort
    {
        public static int[] MergeSort(int[] array)
        {
            int[] left;
            int[] right;
            var result = new int[array.Length];
            //As this is a recursive algorithm, we need to have a base case to 
            //avoid an infinite recursion and therfore a stackoverflow
            if (array.Length <= 1)
                return array;
            // The exact midpoint of our array  
            var midPoint = array.Length / 2;
            //Will represent our 'left' array
            left = new int[midPoint];

            //if array has an even number of elements, the left and right array will have the same number of 
            //elements
            if (array.Length % 2 == 0)
                right = new int[midPoint];
            //if array has an odd number of elements, the right array will have one more element than left
            else
                right = new int[midPoint + 1];
            //populate left array
            for (var i = 0; i < midPoint; i++)
                left[i] = array[i];
            //populate right array   
            var x = 0;
            //We start our index from the midpoint, as we have already populated the left array from 0 to midpont
            for (var i = midPoint; i < array.Length; i++)
            {
                right[x] = array[i];
                x++;
            }

            //Recursively sort the left array
            left = MergeSort(left);
            //Recursively sort the right array
            right = MergeSort(right);
            //Merge our two sorted arrays
            result = Merge(left, right);
            return result;
        }

        //This method will be responsible for combining our two sorted arrays into one giant array
        public static int[] Merge(int[] left, int[] right)
        {
            var resultLength = right.Length + left.Length;
            var result = new int[resultLength];
            //
            int indexLeft = 0, indexRight = 0, indexResult = 0;
            //while either array still has an element
            while (indexLeft < left.Length || indexRight < right.Length)
                //if both arrays have elements  
                if (indexLeft < left.Length && indexRight < right.Length)
                {
                    //If item on left array is less than item on right array, add that item to the result array 
                    if (left[indexLeft] <= right[indexRight])
                    {
                        result[indexResult] = left[indexLeft];
                        indexLeft++;
                        indexResult++;
                    }
                    // else the item in the right array wll be added to the results array
                    else
                    {
                        result[indexResult] = right[indexRight];
                        indexRight++;
                        indexResult++;
                    }
                }
                //if only the left array still has elements, add all its items to the results array
                else if (indexLeft < left.Length)
                {
                    result[indexResult] = left[indexLeft];
                    indexLeft++;
                    indexResult++;
                }
                //if only the right array still has elements, add all its items to the results array
                else if (indexRight < right.Length)
                {
                    result[indexResult] = right[indexRight];
                    indexRight++;
                    indexResult++;
                }

            return result;
        }
    }
}