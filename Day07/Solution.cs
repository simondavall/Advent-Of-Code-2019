using AocHelper;
using AocHardware;

namespace Day07;

internal static partial class Program {
  private const string Title = "\n## Day 7: Amplification Circuit ##";
  private const string AdventOfCode = "https://adventofcode.com/2019/day/7";

  private const long ExpectedPartOne = 880726;
  private const long ExpectedPartTwo = 4931744;

  private static long PartOne(string input) {
    var (min, max) = (0, 4);
    long[] program = input.Split(',', StringSplitOptions.RemoveEmptyEntries).ToLongArray();
    long maxSignal = 0;

    foreach (var phase in GetAllCombinationsOfNumbersBetween(min, max)) {
      long lastOutput = 0;
      bool phaseInput = true;
      for (var i = 0; i < phase.Length; i++) {
        var amp = new Computer(program);
        while (!amp.IsHalted) {
          if (amp.IsAwaitingInput) {
            if (phaseInput)
              amp.SetInput(phase[i]);
            else
              amp.SetInput(lastOutput);
            phaseInput = !phaseInput;
          }
          amp.Execute();
        }
        lastOutput = amp.GetOutput()[^1];
      }
      maxSignal = Math.Max(maxSignal, lastOutput);
    }

    return maxSignal;
  }

  private static long PartTwo(string input) {
    var(min, max) = (5, 9);
    long[] program = input.Split(',', StringSplitOptions.RemoveEmptyEntries).ToLongArray();
    long maxSignal = 0;

    foreach (var phase in GetAllCombinationsOfNumbersBetween(min, max)) {
      bool[] phaseInput = Helper.CreateArray(phase.Length, true);

      var amps = new Computer[phase.Length];
      for (var i = 0; i < phase.Length; i++)
        amps[i] = new Computer(program);

      var cur = 0;
      while (!amps[^1].IsHalted) {
        if (amps[cur].IsAwaitingInput) {
          if (phaseInput[cur]) {
            amps[cur].SetInput(phase[cur]);
            phaseInput[cur] = false;
          } else {
            var prevAmpOutput = amps[(cur + phase.Length - 1) % phase.Length].GetOutput();
            if (prevAmpOutput.Length > 0)
              amps[cur].SetInput(prevAmpOutput[^1]);
            else
              amps[cur].SetInput(0);
          }
        }
        amps[cur].Execute();
        cur = (cur + 1) % phase.Length;
      }
      maxSignal = Math.Max(maxSignal, amps[^1].GetOutput()[^1]);
    }

    return maxSignal;
  }

  private static int[][] GetAllCombinationsOfNumbersBetween(int min, int max) {
    var combos = new List<int[]>();
    for (int i = min; i <= max; i++)
      for (int j = min; j <= max; j++)
        if (i != j)
          for (int k = min; k <= max; k++)
            if (k != i && k != j)
              for (int l = min; l <= max; l++)
                if (l != i && l != j && l != k)
                  for (int m = min; m <= max; m++)
                    if (m != i && m != j && m != k && m != l) {
                      combos.Add([i, j, k, l, m]);
                    }
    return combos.ToArray();
  }
}
