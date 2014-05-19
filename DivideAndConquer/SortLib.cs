using System;
using System.Linq;

namespace DivideAndConquer
{
    public class SortLib
    {
        public int[] MergeSort(int[] input)
        {
            if (input.Count() <= 1)
                return input;

            MergeSortRecurs(input);
            return input;
        }

        public long CountInv(long[] input)
        {
            if (input.Length <= 1)
                return 0;

            var invCount = MergeSort_CountInvRecurs(input);

            return invCount;
        }

        public int[] BubbleSort(int[] input)
        {
            for (var i = 0; i < input.Length - 1; i++)
            {
                for (var j = 0; j < input.Length - i - 1; j++)
                {
                    if (input[j + 1] < input[j])
                    {
                        var temp = input[j+1];
                        input[j + 1] = input[j];
                        input[j] = temp;
                    }   
                } 
            }
            return input;
        }

        public int QuickSort(int[] input, Func<int[], int, int, int> choosePivot)
        {
            if (input.Length <= 1) return 0;

            var nbComparisons = Partition(input, 0, input.Length-1, choosePivot);

            return nbComparisons;
        }

        public int RSelect(int[] input, int orderStatistic, Func<int[], int, int, int> choosePivot)
        {
            if (input.Length == 1) return input[0];

            var selection = Partition(input, 0, input.Length - 1, orderStatistic, choosePivot);

            return selection;
        }


        private void ChoosePivot(int[] input, int left, int right, Func<int[], int, int, int> choosePivot)
        {
            var pivot = choosePivot(input, left, right);
            Swap(input, left, pivot); // swap pivot with left-most array value
        }

        private int Partition(int[] input, int left, int right, Func<int[], int, int, int> choosePivot)
        {
            var nbComparisons = right - left;
            
            ChoosePivot(input, left, right, choosePivot);
            var pivot = input[left];
            var i = left + 1;
            for (var j = left + 1; j <= right; j++)
            {
                if (input[j] >= pivot) continue;
                if (input[j] != input[i]) {
                    Swap(input, i, j);
                }
                i++; // move separation to the right
            }
            Swap(input, left, i - 1); // put pivot back in its place

            if (left < i - 2) // if length of left-most part > 1, recurs
                nbComparisons += Partition(input, left, i - 2, choosePivot);

            if(i < right) // if length of right-most part > 1, recurs
                nbComparisons += Partition(input, i, right, choosePivot);

            return nbComparisons;
        }

        private int Partition(int[] input, int left, int right, int order, Func<int[], int, int, int> choosePivot)
        {
            ChoosePivot(input, left, right, choosePivot);
            var pivot = input[left];
            var i = left + 1;
            for (var j = left + 1; j <= right; j++)
            {
                if (input[j] >= pivot) continue;
                if (input[j] != input[i])
                {
                    Swap(input, i, j);
                }
                i++; // move separation to the right
            }
            Swap(input, left, i - 1); // put pivot back in its place

            if (i - 1 == order) // pivot is the order statistics
                return input[i - 1];
            if (i - 1 > order) // pivot is bigger
            {
                if (left - i + 2 == 1) // but only 1 element left
                    return input[i - 2];
                return Partition(input, left, i - 2, order, choosePivot); // recurs on left part
            }
            if (right - i == 1) //pivot is smaller but only 1 element left
                return input[i];
            return Partition(input, i, right, order, choosePivot); // recurs on right part
        }

        private void Swap(int[] input, int a, int b)
        {
            var temp = input[a];
            input[a] = input[b];
            input[b] = temp;
        }

        private void MergeSortRecurs(int[] input)
        {
            if (input.Length < 2) return;

            int mid = (input.Length + 1) / 2;
            int[] left = new int[mid];
            int[] right = new int[input.Length - mid];

            Array.Copy(input, 0, left, 0, mid);
            Array.Copy(input, mid, right, 0, input.Length - mid);

            MergeSortRecurs(left);
            MergeSortRecurs(right);

            Merge(input, left, right);
        }

        private void Merge(int[] input, int[] left, int[] right)
        {
            int i = 0;
            int j = 0;
            while (i < left.Length || j < right.Length)
            {
                if (i == left.Length)
                {
                    input[i+j] = right[j++];
                }
                else if (j == right.Length)
                {
                    input[i+j] = left[i++];
                }
                else if (left[i] <= right[j])
                {
                    input[i+j] = left[i++];
                }
                else if (right[j] < left[i])
                {
                    input[i+j] = right[j++];
                }
            }
        }

        private long MergeSort_CountInvRecurs(long[] input)
        {
            if (input.Length < 2) return 0;

            long mid = (input.Length + 1) / 2;
            long[] left = new long[mid];
            long[] right = new long[input.Length - mid];

            Array.Copy(input, 0, left, 0, mid);
            Array.Copy(input, mid, right, 0, input.Length - mid);

            return MergeSort_CountInvRecurs(left) + MergeSort_CountInvRecurs(right) + Merge_CountInv(input, left, right);
        }

        private long Merge_CountInv(long[] input, long[] left, long[] right)
        {
            long i = 0;
            long j = 0;
            long count = 0;
            while (i < left.Length || j < right.Length)
            {
                if (i == left.Length)
                {
                    input[i + j] = right[j++];
                }
                else if (j == right.Length)
                {
                    input[i + j] = left[i++];
                }
                else if (left[i] <= right[j])
                {
                    input[i + j] = left[i++];
                }
                else if (right[j] < left[i])
                {
                    input[i + j] = right[j++];
                    count += left.Length - i;
                }
            }
            return count;
        }

    }
}
