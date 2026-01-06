using System.Text;

namespace AocHelper;

public static class Helper
{
  public static List<T> List<T>(List<T> list)
  {
    return list;
  }

  public static IEnumerable<int> Range(int n)
  {
    return Enumerable.Range(0, n);
  }

  public static IEnumerable<int> Range(int start, int n)
  {
    return Enumerable.Range(start, n);
  }

  public static int ToInt(this string str)
  {
    if (int.TryParse(str, out var value))
      return value;

    throw new InvalidCastException($"Not a valid integer: {str}");
  }

  public static long ToLong(this string str)
  {
    if (long.TryParse(str, out var value))
      return value;

    throw new InvalidCastException($"Not a valid integer: {str}");
  }

  public static T[] CreateArray<T>(int size, T defaultValue)
  {
    T[] array = new T[size];
    for (var i = 0; i < size; i++) array[i] = defaultValue;
    return array;
  }

  public static List<T> ToSortedList<T>(this HashSet<T> set)
  {
    var list = set.ToList();
    list.Sort();
    return list;
  }

  public static List<T> Sorted<T>(this List<T> list)
  {
    list.Sort();
    return list;
  }

  public static List<T> SortedDesc<T>(this List<T> list)
  {
    return list.OrderDescending().ToList();
  }

  public static string Print<T>(this T[] arr, int max = 10)
  {
    if (arr.Length > max) {
      return $"[{string.Join(", ", arr[..max])} ... ]";
    } else {
      return $"[{string.Join(", ", arr)}]";
    }
  }

  public static string Print<TKey, TValue>(this Dictionary<TKey, TValue> dict, int max = 10) where TKey : notnull
  {
    var str = new StringBuilder();
    foreach (var (k, v) in dict) {
      str.Append($"[{k}:{v}], ");
      if (--max == 0) {
        str.Append($"... max {max} shown");
        break;
      }
    }
    return str.ToString();
  }

}
