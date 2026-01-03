using System.Diagnostics;
using AocHardware;
using AocHelper;

namespace Day23;

internal static partial class Program
{
  private const string Title = "\n## Day 23: Category Six ##";
  private const string AdventOfCode = "https://adventofcode.com/2019/day/23";
  private const long ExpectedPartOne = 17949;
  private const long ExpectedPartTwo = 12326;

  private class NetworkComputer(Computer c)
  {
    public Computer Computer { get; set; } = c;
    public Queue<long> InputBuffer { get; set; } = [];
  }

  private static NetworkComputer[] _network = [];
  private static long PartOne(string data)
  {
    var program = data.Split(',', StringSplitOptions.RemoveEmptyEntries).ToLongArray();
    _network = new NetworkComputer[50];

    for (var i = 0; i < _network.Length; i++) {
      InitializeComputer(i, program);
    }

    var scheduler = 0;
    while (true) {
      var output = CheckinOnComupter(scheduler);
      if (output.Length > 0) {
        Debug.Assert(output.Length % 3 == 0, $"Expected output in mutliples of 3. Found:{output.Length}, Details:{output.Print(30)}");
        for (var i = 0; i < output.Length; i += 3) {
          if (output[i] == 255) {
            return output[i + 2];
          }
          _network[output[i]].InputBuffer.Enqueue(output[i + 1]);
          _network[output[i]].InputBuffer.Enqueue(output[i + 2]);
        }
      }
      scheduler = ++scheduler % _network.Length;
    }
  }

  private static long PartTwo(string data)
  {
    (long x, long y) nat = (-1, -1);
    long previousY = 0;
    var program = data.Split(',', StringSplitOptions.RemoveEmptyEntries).ToLongArray();
    _network = new NetworkComputer[50];

    for (var i = 0; i < _network.Length; i++) {
      InitializeComputer(i, program);
    }

    var scheduler = 0;
    while (true) {
      var output = CheckinOnComupter(scheduler);
      if (output.Length > 0) {
        Debug.Assert(output.Length % 3 == 0, $"Expected output in mutliples of 3. Found:{output.Length}, Details:{output.Print(max: 30)}");
        for (var i = 0; i < output.Length; i += 3) {
          if (output[i] == 255) {
            nat = (output[i + 1], output[i + 2]);
          } else {
            _network[output[i]].InputBuffer.Enqueue(output[i + 1]);
            _network[output[i]].InputBuffer.Enqueue(output[i + 2]);
          }
        }
      }
      scheduler = ++scheduler % _network.Length;
      if (scheduler == 0){
        var inputCount = _network.Sum(x => x.InputBuffer.Count());
        if (inputCount == 0){
          if (nat.y == previousY)
            return nat.y;
          previousY = nat.y;
          _network[0].InputBuffer.Enqueue(nat.x);
          _network[0].InputBuffer.Enqueue(nat.y);
        }
      }
    }
  }

  private static void InitializeComputer(int addr, long[] program)
  {
    var computer = new Computer(program);
    var networkComputer = new NetworkComputer(computer);
    _network[addr] = networkComputer;
    computer.Execute();
    if (computer.IsAwaitingInput)
      computer.SetInput(addr);
  }


  private static long[] CheckinOnComupter(int addr)
  {
    var computer = _network[addr].Computer;

    if (computer.IsAwaitingInput) {
      long inputData = -1;
      if (_network[addr].InputBuffer.Count() > 0)
        inputData = _network[addr].InputBuffer.Dequeue();
      computer.SetInput(inputData);
    }

    computer.Execute();
    return computer.GetOutput();
  }
}
