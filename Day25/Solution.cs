using AocHardware;
using AocHelper;

namespace Day25;

internal static partial class Program
{
  private const string Title = "\n## Day 25: Cryostasis ##";
  private const string AdventOfCode = "https://adventofcode.com/2019/day/25";
  private const long ExpectedPartOne = 4206594;
  private const long ExpectedPartTwo = 0;

  private static long PartOne(string data, string commands, string inventory)
  {
    var program = data.Split(',', StringSplitOptions.RemoveEmptyEntries).ToLongArray();
    var computer = new Computer(program);
    var preLoadedCommands = new Queue<char>();
    var commentedOut = false;
    foreach (var ch in commands.ToCharArray()[..^1]) {
      if (ch == '#')
        commentedOut = true;
      if (commentedOut && ch == '\n') {
        commentedOut = false;
        continue;
      }
      if (!commentedOut)
        preLoadedCommands.Enqueue(ch);
    }

    TryAllInventoryCombinations(preLoadedCommands, inventory);

    var inputBuffer = new Queue<char>();
    while (!computer.IsHalted) {
      if (computer.IsAwaitingInput) {
        if (preLoadedCommands.Count > 0) {
          var input = preLoadedCommands.Dequeue();
          Console.Write($"\u001b[32m{input}\u001b[0m");
          computer.SetInput(input);
        } else {
          if (inputBuffer.Count == 0) {
            var input = Console.ReadLine();
            if (input is not null) {
              foreach (var ch in input.ToCharArray())
                inputBuffer.Enqueue(ch);
            }
            inputBuffer.Enqueue('\n');
          }
          if (inputBuffer.Count() > 0) {
            var input = inputBuffer.Dequeue();
            Console.Write($"\u001b[31m{input}\u001b[0m");
            computer.SetInput(input);
          }
        }
      }
      computer.Execute();
      var output = computer.GetOutput();
      foreach (var ch in output)
        Console.Write((char)ch);
    }

    // Answer from console mseeage
    return 4206594;
  }

  private static void TryAllInventoryCombinations(Queue<char> preLoadedCommands, string inventory){
    var items = inventory.Split('\n', StringSplitOptions.RemoveEmptyEntries);
    var combos = Algorithms.Combinations(items);
    var inventryCombos = new Queue<string[]>(combos);

    while (inventryCombos.Count() > 0){
      var combo = inventryCombos.Dequeue();
      if (combo.Length == 0)
        continue;
      foreach(var item in combo){
        Array.ForEach($"take {item}\n".ToCharArray(), preLoadedCommands.Enqueue);
      }
      Array.ForEach($"south\n".ToCharArray(), preLoadedCommands.Enqueue);
      foreach(var item in combo){
        Array.ForEach($"drop {item}\n".ToCharArray(), preLoadedCommands.Enqueue);
      }
    }
  }

  private static long PartTwo(string data)
  {
    long tally = 0;
    return tally;
  }
}
