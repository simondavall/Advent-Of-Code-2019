using AocHardware;
using AocHelper;

namespace Day21;

internal static partial class Program
{
  private const string Title = "\n## Day 21: Springdroid Adventure ##";
  private const string AdventOfCode = "https://adventofcode.com/2019/day/21";
  private const long ExpectedPartOne = 19350938;
  private const long ExpectedPartTwo = 1142986901;

  private const char A = 'A';
  private const char B = 'B';
  private const char C = 'C';
  private const char D = 'D';
  private const char E = 'E';
  private const char H = 'H';
  private const char T = 'T';
  private const char J = 'J';

  private static long PartOne(string data)
  {
    string script = "";
    script += Not(A, J);
    script += Not(B, T);
    script += Or(T, J);
    script += Not(C, T);
    script += Or(T, J);
    script += And(D, J);
    script += "WALK\n";

    return RunScript(script, data);
  }

  private static long PartTwo(string data)
  {
    string script = "";
    script += Not(A, J);
    script += Not(B, T);
    script += Or(T, J);
    script += Not(C, T);
    script += Or(T, J);
    script += Not(E, T);
    script += Not(T, T);
    script += Or(H, T);
    script += And(T, J);
    script += And(D, J);
    script += "RUN\n";
    
    return RunScript(script, data);
  }

  private static long RunScript(string script, string data)
  {
    var program = data.Split(',').ToLongArray();
    var computer = new Computer(program);

    var q = new Queue<char>(script.ToCharArray());

    long result = 0;
    while (!computer.IsHalted) {
      if (computer.IsAwaitingInput) {
        if (q.Count() == 0)
          throw new ApplicationException("Run out of script commands.");
        long input = q.Dequeue();
        computer.SetInput(input);
      }
      computer.Execute();
      var output = computer.GetOutput();
      if (output.Length > 0) {
        if (output.Last() > char.MaxValue)
          result = output.Last();
        else
          foreach (var ch in output) {
            Console.Write((char)ch);
          }
      }
    }
    return result;
  }

  private static string And(char x, char y)
  {
    return $"AND {x} {y}\n";
  }

  private static string Or(char x, char y)
  {
    return $"OR {x} {y}\n";
  }

  private static string Not(char x, char y)
  {
    return $"NOT {x} {y}\n";
  }
}
