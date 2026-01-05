using System.Numerics;

namespace AocHelper;

public static class VarDump
{
  public enum Params{
    NonPadded
  }

  public static void Dump<T>(T[] arr)
  {
    Console.Write("[");
    for (var i = 0; i < arr.Length; i++) {
      Console.Write($"{arr[i]}, ");
    }
    Console.Write("\b\b]\n");
    Console.WriteLine();
  }

  public static void Dump(char[][] arr, params Params[] p){
    DumpNonNumeric(arr, p); //calling with type:char
  }

  private static void DumpNonNumeric<T>(T[][] arr, params Params[] p)
  {
    var len = arr.Length;
    int idxLen = len.ToString().Length;

    if (p.Contains(Params.NonPadded)) { // no padding
      for (var i = 0; i < len; i++) {
        for (var j = 0; j < arr[i].Length; j++) {
          Console.Write($"\u001b[32m{arr[i][j]}\u001b[0m");
        }
        Console.Write("\n");
      }
    } else {
      for (var i = 0; i < len; i++) {
        PaddedString("[", i.ToString(), '0', idxLen, "] = [");
        for (var j = 0; j < arr[i].Length; j++) {
          Console.Write($"\u001b[32m{arr[i][j]}\u001b[0m, ");
        }
        Console.Write("\b\b]\n");
      }
    }
    Console.WriteLine();
  }

  public static void Dump<T>(T[][] arr, params Params[] p) where T : INumber<T>
  {
    var len = arr.Length;
    int idxLen = len.ToString().Length;
    if (p.Contains(Params.NonPadded)) { // no padding
      for (var i = 0; i < len; i++) {
        for (var j = 0; j < arr[i].Length; j++) {
          if (arr[i][j] < T.Zero)
            Console.Write($"\u001b[31m{T.Abs(arr[i][j])}\u001b[0m");
          else
            Console.Write($"\u001b[32m{arr[i][j]}\u001b[0m");
        }
        Console.Write("\n");
      }
    } else {
      for (var i = 0; i < len; i++) {
        PaddedString("[", i.ToString(), '0', idxLen, "] = [");
        for (var j = 0; j < arr[i].Length; j++) {
          if (arr[i][j] < T.Zero)
            Console.Write($"\u001b[31m{T.Abs(arr[i][j])}\u001b[0m, ");
          else
            Console.Write($"\u001b[32m{arr[i][j]}\u001b[0m, ");
        }
        Console.Write("\b\b]\n");
      }
    }
    Console.WriteLine();
  }

  private static void PaddedString(string prefix, string value, char format, int padding, string postfix)
  {
    Console.Write(prefix);
    Console.Write(string.Format("{0," + padding + "}", value));
    Console.Write(postfix);
  }

  public static List<T> List<T>(List<T> list)
  {
    return list;
  }
}

