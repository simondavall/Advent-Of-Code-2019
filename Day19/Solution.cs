using AocHardware;
using AocHelper;

namespace Day19;

internal static partial class Program
{
  private const string Title = "\n## Day 19: Tractor Beam ##";
  private const string AdventOfCode = "https://adventofcode.com/2019/day/19";
  private const long ExpectedPartOne = 226;
  private const long ExpectedPartTwo = 7900946;

  private static long PartOne(string data)
  {
    var size = 50;
    var program = data.Split(',').ToLongArray();
    var orig_computer = new Computer(program);
    long tally = 0;
    for (var y = 0; y < size; y++) {
      for (var x = 0; x < size; x++) {
        var counter = 0;
        var computer = orig_computer.Clone();
        while (!computer.IsHalted) {
          if (computer.IsAwaitingInput) {
            long input = counter++ % 2 == 0 ? x : y;
            computer.SetInput(input);
          }
          computer.Execute();
          var output = computer.GetOutput();
          if (output.Length > 0)
            tally += output[0];
        }
      }
    }
    return tally;
  }

  private static long PartTwo(string data)
  {
    var squareSize = 100; // find 100 x 100 square
    var beam = new HashSet<(int, int)>();
    var program = data.Split(',').ToLongArray();
    var orig_computer = new Computer(program);
    long tally = 0;
    int xCount = 0;
    int xStart = 0;
    int y = 0;

    bool squareFits = false;
    while (!squareFits) {
      int xSkip = xCount - 1; // set the amount of skipped x's to the xcount from the previous row less 1
      int xStop = xStart > 0 ? xStart + xCount + 3 : y + 1; // set the value to stop on the x axis for this row
      xCount = 0;
      bool firstX = true;
      for (var x = xStart; x < xStop; x++) {
        var counter = 0;
        var computer = orig_computer.Clone();
        while (!computer.IsHalted) {
          if (computer.IsAwaitingInput) {
            long input = counter++ % 2 == 0 ? x : y;
            computer.SetInput(input);
          }
          computer.Execute();
          var output = computer.GetOutput();
          if (output.Length > 0 && output[0] == 1) {
            if (firstX) { // if this is the first # on the row, check if the square fits in the beam
              firstX = false;
              squareFits = SquareFits(x, y, squareSize, beam, out tally);
            }
            if (xSkip > 0) { // if we can skip some x values as we know they are in the beam
              if (x > 0)
                xStart = x - 1; // increase the starting value of x 
              for (var i = 0; i < xSkip; i++)
                beam.Add((y, x + i)); // add the skipped values to the beam
              xCount += xSkip; // increase the count of x's for the row by the skipped amount
              x += xSkip - 1; // increament x by the skipped amount
              xSkip = 0; // reset the skipped amount so we don't skip again on this row.
            } else {
              beam.Add((y, x));
              xCount++;
            }
          }
        }
      }
      y++;
    }
    return tally;
  }

  private static bool SquareFits(int x, int y, int size, HashSet<(int, int)> beam, out long result)
  {
    result = -1;
    if (y > size && beam.Contains((y - (size - 1), x + (size - 1)))) {
      Console.WriteLine($"Found Square at x:{x}, y:{y - (size - 1)}");
      result = x * 10000 + y - (size - 1);
      return true;
    }
    return false;
  }
}
