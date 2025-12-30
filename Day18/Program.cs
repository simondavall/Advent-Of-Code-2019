using System.Diagnostics;

namespace Day18;

internal static partial class Program {
 public static int Main(string[] args) {
    Console.WriteLine(Title);
    Console.WriteLine(AdventOfCode);

    long resultPartOne = -1;
    long resultPartTwo = -1;

    foreach (var filePath in args) {
      Console.WriteLine($"\nFile: {filePath}\n");
      var (data, input) = GetData(filePath);
      var stopwatch = Stopwatch.StartNew();

      resultPartOne = PartOne(data);
      PrintResult("1", resultPartOne.ToString(), stopwatch);

      resultPartTwo = PartTwo(data, input);
      PrintResult("2", resultPartTwo.ToString(), stopwatch);
    }

    return resultPartOne == ExpectedPartOne && resultPartTwo == ExpectedPartTwo ? 0 : 1;
  }

  private static (string[] data, string input) GetData(string filePath) {
    using var streamReader = new StreamReader(filePath);
    var input = streamReader.ReadToEnd();
    var data = input.Split('\n', StringSplitOptions.RemoveEmptyEntries);
    return (data, input);
  }

  private static void PrintResult(string partNo, string result, Stopwatch sw) {
    sw.Stop();
    Console.WriteLine($"Part {partNo} Result: {result} in {sw.Elapsed.TotalMilliseconds}ms");
    sw.Restart();
  }
}
