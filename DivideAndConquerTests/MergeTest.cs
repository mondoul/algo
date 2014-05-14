using System;
using System.Diagnostics;
using System.Linq;
using DivideAndConquer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploeh.AutoFixture;

namespace DivideAndConquerTests
{
    [TestClass]
    public class DivideAndConquerTest
    {
        private readonly SortLib _lib = new SortLib();

        [TestMethod]
        public void TestMergeSort()
        {
            var input = new[] {2, 4, 5, 1, 7, 8, 3, 6};
            var result = _lib.MergeSort(input);
            CollectionAssert.AreEqual(result, new [] {1, 2, 3, 4, 5, 6, 7, 8});
        }

        [TestMethod]
        public void TestQuickSort()
        {
            var input = new[] { 2, 4, 5, 1, 7, 8, 3, 6 };
            var rand = new Random(DateTime.Now.Millisecond);
            var nbComparisons = _lib.QuickSort(input, (ints, l, r) => rand.Next(l, r));
            CollectionAssert.AreEqual(input, new[] { 1, 2, 3, 4, 5, 6, 7, 8 });
        }

        [TestMethod]
        public void TestQuickSortCountComparisons()
        {
            var input = new[] { 2, 4, 5, 1, 7, 8, 3, 6 };
            Func<int[], int, int, int> choosePivot = (ints, left, right) => left;
            var nbComparisons = _lib.QuickSort(input, choosePivot);
            CollectionAssert.AreEqual(input, new[] { 1, 2, 3, 4, 5, 6, 7, 8 });
            Assert.IsTrue(nbComparisons == 16);
        }

        [TestMethod]
        public void TestQuickSortCountComparisonsWithMedianPivot()
        {
            var inputTab = new[] { 2, 4, 5, 1, 7, 8, 3, 6 };
            Func<int[], int, int, int> choosePivot = (input, left, right) =>
            {
                var length = right - left + 1;
                var l = input[left];
                var r = input[right];
                var medIndex = length % 2 == 0 ? (length / 2) - 1 : length / 2;
                var m = input[medIndex];
                var tab = new int[] { l, r, m };
                tab = tab.OrderBy(t => t).ToArray();
                return tab[1] == l
                    ? left
                    : tab[1] == r
                        ? right
                        : medIndex;
            };
            var nbComparisons = _lib.QuickSort(inputTab, choosePivot);
            CollectionAssert.AreEqual(inputTab, new[] { 1, 2, 3, 4, 5, 6, 7, 8 });
            Assert.IsTrue(nbComparisons == 16);
        }

        [TestMethod]
        public void TestQuickSortCountComparisonsFromFileWithPivotFirstElement()
        {
            var filePath = @"c:\QuickSort.txt";
            Func<int[], int, int, int> choosePivot = (input, left, right) => left;
            var nbComparisons = _lib.CountComparisonInQuickSortFromFile(filePath, choosePivot);
            Assert.IsTrue(nbComparisons == 162085);
        }

        [TestMethod]
        public void TestQuickSortCountComparisonsFromFileWithPivotLastElement()
        {
            var filePath = @"c:\QuickSort.txt";
            Func<int[], int, int, int> choosePivot = (input, left, right) => right;
            var nbComparisons = _lib.CountComparisonInQuickSortFromFile(filePath, choosePivot);
            Assert.IsTrue(nbComparisons == 164123);
        }

        [TestMethod]
        public void TestQuickSortCountComparisonsFromFileWithPivotMedianElement()
        {
            var filePath = @"c:\QuickSort.txt";
            Func<int[], int, int, int> choosePivot = (input, left, right) =>
            {
                var length = right - left + 1;
                var l = input[left];
                var r = input[right];
                var medIndex = length % 2 == 0 ? (length / 2) - 1 : length / 2;
                var m = input[medIndex + left];
                var tab = new[] { l, r, m };
                tab = tab.OrderBy(t => t).ToArray();
                return tab[1] == l
                    ? left
                    : tab[1] == r
                        ? right
                        : medIndex + left;
            };
            var nbComparisons = _lib.CountComparisonInQuickSortFromFile(filePath, choosePivot);
            Assert.IsTrue(nbComparisons == 138382);
        }

        [TestMethod]
        public void TestFindMedian()
        {
            Func<int[], int, int, int> choosePivot = (input, left, right) =>
            {
                var length = right - left + 1;
                var l = input[left];
                var r = input[right];
                var medIndex = length % 2 == 0 ? (length / 2) - 1 : length / 2;
                var m = input[medIndex + left];
                var tab = new[] { l, r, m };
                tab = tab.OrderBy(t => t).ToArray();
                return tab[1] == l
                    ? left
                    : tab[1] == r
                        ? right
                        : medIndex + left;
            };
            var inputA = new[] { 8, 2, 4, 5, 7, 1 };
            var inputB = new[] { 4, 2, 8, 5, 7, 1 };
            var inputE = new[] { 1, 2, 8, 5, 7, 4 };
            var inputC = new[] { 8, 2, 4, 5, 7, 1, 3 };
            var inputD = new[] { 1, 2, 4, 3, 7, 1, 2 };
            var inputF = new[] { 2, 2, 4, 1, 7, 1, 3 };
            var inputI = new[] { 3, 2, 4, 1};
            Assert.IsTrue(choosePivot(inputA, 0, inputA.Length - 1) == 2);
            Assert.IsTrue(choosePivot(inputB, 0, inputB.Length - 1) == 0);
            Assert.IsTrue(choosePivot(inputE, 0, inputE.Length - 1) == 5);
            Assert.IsTrue(choosePivot(inputE, 2, inputE.Length - 1) == 3);
            Assert.IsTrue(choosePivot(inputC, 0, inputC.Length - 1) == 3);
            Assert.IsTrue(choosePivot(inputD, 0, inputD.Length - 1) == 6);
            Assert.IsTrue(choosePivot(inputF, 0, inputF.Length - 1) == 0);
            Assert.IsTrue(choosePivot(inputI, 0, inputI.Length - 1) == 1);
        }

        [TestMethod]
        public void TestQuickSortBis()
        {
            var rand = new Random(DateTime.Now.Millisecond);
            var fixture = new Fixture();
            var input = fixture.CreateMany<int>(1000).ToArray();
            var watch = new Stopwatch();
            watch.Start();
            var nbComparisons = _lib.QuickSort(input, (ints, l, r) => rand.Next(l, r));
            watch.Stop();
            var quickSortTime = watch.ElapsedMilliseconds;
            CollectionAssert.AreEqual(input, input.OrderBy(i => i).ToArray());
        }

        [TestMethod]
        public void TestMergeSortBis()
        {
            var input = new[] { 2, 4, 5, 9, 1, 7, 8, 3, 6 };
            var result = _lib.MergeSort(input);
            CollectionAssert.AreEqual(result, new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 });
        }

        [TestMethod]
        public void TestCountInvInFile()
        {
            var filePath = @"c:\IntegerArray.txt";
            var result = _lib.CountInvInFile(filePath);
            Assert.IsTrue(result > 0);
        }

        [TestMethod]
        public void TestInvCount()
        {
            var input = new long[] { 2, 4, 5, 1, 7, 8, 3, 6 };
            var result = _lib.CountInv(input);
            Assert.AreEqual(result, 9);
        }

        [TestMethod]
        public void TestInvCountBis()
        {
            var input = new long[] { 3, 2, 1, 4, 6, 5, 7 };
            var result = _lib.CountInv(input);
            Assert.AreEqual(result, 4);
        }

        [TestMethod]
        public void TestBubbleSort()
        {
            var input = new[] { 2, 4, 5, 1, 7, 8, 3, 6 };
            var result = _lib.BubbleSort(input);
            CollectionAssert.AreEqual(result, new[] { 1, 2, 3, 4, 5, 6, 7, 8 });
        }

        [TestMethod]
        public void TestLargeSort()
        {
            var fixture = new Fixture();
            var input_1 = fixture.CreateMany<int>(1000).ToArray();
            var input_2 = fixture.CreateMany<int>(1000).ToArray();
            var input_3 = fixture.CreateMany<int>(1000).ToArray();
            var watch = new Stopwatch();
            watch.Start();
            var result = _lib.MergeSort(input_1);
            watch.Stop();
            var mergeSortTime = watch.ElapsedMilliseconds;
            CollectionAssert.AreEqual(result, input_1.OrderBy(i => i).ToArray());
            watch.Reset();
            
            var rand = new Random(DateTime.Now.Millisecond);
            watch.Start();
            var nbComparisons = _lib.QuickSort(input_2, (ints, l, r) => rand.Next(l, r));
            watch.Stop();
            var qsResult = new int[1000];
            Array.Copy(input_2, qsResult, 1000);
            var quickSortTime = watch.ElapsedMilliseconds;
            CollectionAssert.AreEqual(qsResult, input_2.OrderBy(i => i).ToArray());
            watch.Reset();

            watch.Start();
            result = _lib.BubbleSort(input_3);
            watch.Stop();
            var bubbleSortTime = watch.ElapsedMilliseconds;
            CollectionAssert.AreEqual(result, input_3.OrderBy(i => i).ToArray());
            Assert.IsTrue(bubbleSortTime > mergeSortTime && bubbleSortTime > quickSortTime);

            watch.Start();
        }

        [TestMethod]
        public void CompareMergesortQuicksort()
        {
            var fixture = new Fixture();
            var input_1 = fixture.CreateMany<int>(100000).ToArray();
            var input_2 = fixture.CreateMany<int>(100000).ToArray();
            var watch = new Stopwatch();
            watch.Start();
            var result = _lib.MergeSort(input_1);
            watch.Stop();
            var mergeSortTime = watch.ElapsedMilliseconds;
            CollectionAssert.AreEqual(result, input_1.OrderBy(i => i).ToArray());
            watch.Reset();

            var rand = new Random(DateTime.Now.Millisecond);
            watch.Start();
            var nbComparisons = _lib.QuickSort(input_2, (ints, l, r) =>  rand.Next(l, r));
            watch.Stop();
            var qsResult = new int[100000];
            Array.Copy(input_2, qsResult, 100000);
            var quickSortTime = watch.ElapsedMilliseconds;
            CollectionAssert.AreEqual(qsResult, input_2.OrderBy(i => i).ToArray());
            watch.Reset();
        }
    }
}
