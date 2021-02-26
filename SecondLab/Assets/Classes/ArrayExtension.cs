using System;

namespace Classes {
    public static class ArrayExtension {
        public static void CocktailSort<T>(this T[] arr, out long numOfOperations) where T : IComparable<T> {
            numOfOperations = 0;
            for (var i = 0; i < arr.Length / 2; i++) {
                var swapFlag = false;
                for (var j = i; j < arr.Length - i - 1; j++) {
                    numOfOperations += 1;
                    if (arr[j].CompareTo(arr[j + 1]) > 0) {
                        Swap(ref arr[j], ref arr[j + 1]);
                        swapFlag = true;
                    }
                }

                for (var j = arr.Length - 2 - i; j > i; j--) {
                    numOfOperations += 1;
                    if (arr[j].CompareTo(arr[j - 1]) < 0) {
                        Swap(ref arr[j - 1], ref arr[j]);
                        swapFlag = true;
                    }
                }

                if (!swapFlag) {
                    break;
                }
            }
        }

        private static void Swap<T>(ref T e1, ref T e2) {
            var temp = e1;
            e1 = e2;
            e2 = temp;
        }
    }
}