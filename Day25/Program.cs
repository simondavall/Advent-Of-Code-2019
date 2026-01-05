using System.Diagnostics;

namespace Day25;

internal static partial class Program
{
  public static int Main(string[] args)
  {
    Console.WriteLine(Title);
    Console.WriteLine(AdventOfCode);

    long resultPartOne = -1;
    long resultPartTwo = -1;

    foreach (var filePath in args) {
      Console.WriteLine($"\nFile: {filePath}\n");
      string input = GetData(filePath);
      string commands = GetData($"../{typeof(Program).Namespace}/commands");
      string inventory = GetData($"../{typeof(Program).Namespace}/inventory");
      var stopwatch = Stopwatch.StartNew();

      resultPartOne = PartOne(input, commands, inventory);
      PrintResult("1", resultPartOne.ToString(), stopwatch);

      resultPartTwo = PartTwo(input);
      PrintResult("2", resultPartTwo.ToString(), stopwatch);
    }

    return resultPartOne == ExpectedPartOne && resultPartTwo == ExpectedPartTwo ? 0 : 1;
  }

  private static string GetData(string filePath)
  {
    try
    {
      using var streamReader = new StreamReader(filePath);
        
      return streamReader.ReadToEnd();
    }
    catch
    {
        Console.WriteLine($"Error reading file:{filePath}");
    }
    return string.Empty;
  }

  private static void PrintResult(string partNo, string result, Stopwatch sw)
  {
    sw.Stop();
    Console.WriteLine($"Part {partNo} Result: {result} in {sw.Elapsed.TotalMilliseconds}ms");
    sw.Restart();
  }
}
