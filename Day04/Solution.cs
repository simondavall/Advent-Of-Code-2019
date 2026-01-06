namespace Day04;

internal static partial class Program
{
  private const string Title = "\n## Day 4: Secure Container ##";
  private const string AdventOfCode = "https://adventofcode.com/2019/day/4";

  private const long ExpectedPartOne = 2050;
  private const long ExpectedPartTwo = 1390;

  private static long PartOne()
  {
    (int min, int max) = (128392, 643281);

    int validPasswordCount = 0;

    for (var i = min; i <= max; i++) {
      var password = i.ToDigitArray();
      if (HasTwoAdjacentNumbers(password) && IsNeverDecreasing(password))
        validPasswordCount++;
    }

    return validPasswordCount;
  }

  private static long PartTwo()
  {
    (int min, int max) = (128392, 643281);

    int validPasswordCount = 0;

    for (var i = min; i <= max; i++) {
      var password = i.ToDigitArray();
      if (HasOnlyTwoAdjacentNumbers(password) && IsNeverDecreasing(password))
        validPasswordCount++;
    }

    return validPasswordCount;
  }

  private static int[] ToDigitArray(this int n)
  {
    var arr = new int[n.Length()];
    for (int i = arr.Length - 1; i >= 0; i--) {
      arr[i] = n % 10;
      n /= 10;
    }
    return arr;
  }

  private static bool HasOnlyTwoAdjacentNumbers(int[] code)
  {
    int[] arr = [-1, .. code, -1];
    for (var i = 1; i < code.Length; i++) {
      var current = arr[i];
      if (current == arr[i + 1]
          && current != arr[i - 1]
          && current != arr[i + 2])
        return true;
    }
    return false;
  }

  private static bool HasTwoAdjacentNumbers(int[] code)
  {
    for (var i = 0; i < code.Length - 1; i++) {
      if (code[i] == code[i + 1])
        return true;
    }
    return false;
  }

  private static bool IsNeverDecreasing(int[] code)
  {
    for (var i = 0; i < code.Length - 1; i++) {
      if (code[i] > code[i + 1])
        return false;
    }
    return true;
  }

  private static int Length(this int n)
  {
    n = Math.Abs(n);
    var count = 0;
    while (n > 0) {
      n /= 10;
      count++;
    }
    return count;
  }
}
