using AocHardware;
using AocHelper;

namespace Day02;

internal static partial class Program {
  private const string Title = "\n## Day 2: 1202 Program Alarm ##";
  private const string AdventOfCode = "https://adventofcode.com/2019/day/2";

  private const long ExpectedPartOne = 6568671;
  private const long ExpectedPartTwo = 3951;

  private static long PartOne(string input) {
    var program = input.Split(',', StringSplitOptions.RemoveEmptyEntries).ToLongArray();
    var computer = new Computer(program);
    computer.SetMemory(1, 12);
    computer.SetMemory(2, 2);
    while (!computer.IsHalted) {
      if (computer.IsAwaitingInput)
        computer.SetInput(0);

      computer.Execute();
    }

    return computer.ReadMemory(0);
  }

  private static long PartTwo(string input) {
    long requiredOutput = 19690720;
    var program = input.Split(',', StringSplitOptions.RemoveEmptyEntries).ToLongArray();
    long result = 0;
    Computer computer;

    bool terminated = false;
    for (int noun = 0; noun < 100; noun++) {
      for (int verb = 0; verb < 100; verb++) {
        computer = new Computer(program);
        computer.SetMemory(1, noun);
        computer.SetMemory(2, verb);
        while (!computer.IsHalted) {
          if (computer.IsAwaitingInput)
            computer.SetInput(0);

          computer.Execute();
        }

        if (computer.ReadMemory(0) == requiredOutput) {
          terminated = true;
          result = 100 * noun + verb;
          break;
        }
      }
      if (terminated)
        break;
    }

    if (!terminated)
      throw new ApplicationException("Correct value was not found. Program terminated.");

    return result;
  }
}
