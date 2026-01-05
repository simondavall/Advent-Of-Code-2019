namespace AocHelper;

public static class Algorithms
{
  ///Returns all combinations of the provided array.
  ///This does not consider the order of the returned combinations.
  ///Therefore it returns 2^arr.Length combinations.
  public static T[][] Combinations<T>(T[] arr)
  {
    var maxLength = sizeof(long) * 8;
    if (arr.Length > maxLength)
      throw new ArgumentOutOfRangeException(nameof(arr), OutOfRangeErrorMessage(arr.Length, maxLength));

    var indicies = new long[arr.Length + 1];
    for (var i = 0; i <= arr.Length; i++)
      indicies[i] = 1 << i;

    var combos = new List<T[]>();
    for (long filter = 0; filter < 1 << arr.Length; filter++) {
      var newArray = new List<T>();
      for (var i = 0; i <= arr.Length; i++) {
        var index = indicies[i];
        if ((filter & index) == index)
          newArray.Add(arr[i]);
      }
      combos.Add(newArray.ToArray());
    }
    return combos.ToArray();
  }

  private static string OutOfRangeErrorMessage(int arrLength, int maxLength) => "\n" +
    "For this implementation, max array size is {maxLength}.\n" +
          $"Current array size:{arrLength}.\n" +
          "Need to provide an implementation with " +
          "larger range using (long or BigInteger)\n";
}

