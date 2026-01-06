namespace AocHelper;

public static class InputExtensions
{
  public static int[] ToIntArray(this string[] array)
  {
    var intArray = new int[array.Length];
    for (var i = 0; i < array.Length; i++) {
      intArray[i] = array[i].ToInt();
    }

    return intArray;
  }

  public static int[] ToIntArray(this Span<string> array)
  {
    var intArray = new int[array.Length];
    for (var i = 0; i < array.Length; i++) {
      intArray[i] = array[i].ToInt();
    }

    return intArray;
  }

  public static long[] ToLongArray(this string[] array)
  {
    var longArray = new long[array.Length];
    for (var i = 0; i < array.Length; i++) {
      longArray[i] = array[i].ToLong();
    }

    return longArray;
  }

  public static char[][] To2DCharArray(this string[] array)
  {
    var charArr = new char[array.Length][];
    for (var i = 0; i < array.Length; i++)
      charArr[i] = array[i].ToCharArray();

    return charArr;
  }

  public static int[][] To2DIntArray(this string[] array)
  {
    var intArr = new int[array.Length][];
    for (var i = 0; i < array.Length; i++) {
      intArr[i] = new int[array[i].Length];
      for (var j = 0; j < array[i].Length; j++) {
        intArr[i][j] = array[i][j] - '0';
      }
    }

    return intArr;
  }

  public static (T first, T second) ToTuplePair<T>(this T[] array)
  {
    return array.Length switch {
      > 2 => throw new ArgumentException(
          $" Too many array members.{array.Length} This method requires an array of length 2."),
      < 2 => throw new ArgumentException(
          $" Too few array members.{array.Length} This method requires an array of length 2."),
      _ => (array[0], array[1])
    };
  }

  public static (int first, int second) ToIntTuplePair(this string[] array)
  {
    if (array.Length > 2)
      throw new ArgumentException(
          $" Too many array members.{array.Length} This method requires an array of length 2.");
    if (array.Length < 2)
      throw new ArgumentException(
          $" Too few array members.{array.Length} This method requires an array of length 2.");

    return (array[0].ToInt(), array[1].ToInt());
  }

  public static (int first, int second, int third) ToIntTupleTriple(this string[] array)
  {
    return array.Length switch {
      > 3 => throw new ArgumentException(
          $" Too many array members.{array.Length} This method requires an array of length 3."),
      < 3 => throw new ArgumentException(
          $" Too few array members.{array.Length} This method requires an array of length 3."),
      _ => (array[0].ToInt(), array[1].ToInt(), array[2].ToInt())
    };
  }

  public static (long first, long second, long third) ToLongTupleTriple(this string[] array)
  {
    return array.Length switch {
      > 3 => throw new ArgumentException(
          $" Too many array members.{array.Length} This method requires an array of length 3."),
      < 3 => throw new ArgumentException(
          $" Too few array members.{array.Length} This method requires an array of length 3."),
      _ => (array[0].ToLong(), array[1].ToLong(), array[2].ToLong())
    };
  }
}
