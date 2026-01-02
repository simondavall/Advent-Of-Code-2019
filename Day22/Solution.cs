using System.Diagnostics;
using AocHelper;

namespace Day22;

internal static partial class Program
{
  private const string Title = "\n## Day 22: Slam Shuffle ##";
  private const string AdventOfCode = "https://adventofcode.com/2019/day/22";
  private const long ExpectedPartOne = 6696;
  private const long ExpectedPartTwo = 0;

  private static long PartOne(string data)
  {
    // var deckSize = 10; // use this for sample
    //long deckSize = 119315717514047;
    long deckSize = 10007;
    long cardIndex = 2019; // use a number between 0 and 9 for sample.
    //long cardIndex = 5403293420776;

    var sequence = GetSequence(data, deckSize);

    foreach (var (type, amount) in sequence) {
      cardIndex = type switch {
        ShuffleTypes.Cut => (cardIndex + (deckSize - amount)) % deckSize,
        ShuffleTypes.DealWithIncrement => amount * cardIndex % deckSize,
        ShuffleTypes.DealIntoStack => deckSize - cardIndex - 1,
        _ => throw new ApplicationException($"Unknown shuffle type")
      };
    }

    return cardIndex;
  }

  private enum ShuffleTypes
  {
    Cut,
    DealWithIncrement,
    DealIntoStack
  }


  private static long PartTwo(string data)
  {
    //long deckSize = 119315717514047;
    long deckSize = 10007;
    long cardIndex = 2019; // produces 2020 on next iteration
    long repetitions = 10007;
    //long repetitions = 1;

    var sequence = GetSequence(data, deckSize);
    // sequence.Reverse();

    long counter = 0;
    while (counter++ < repetitions) {
      foreach (var (type, amount) in sequence) {
        cardIndex = type switch {
          ShuffleTypes.Cut => (cardIndex + (deckSize - amount)) % deckSize,
          ShuffleTypes.DealWithIncrement => amount * cardIndex % deckSize,
          ShuffleTypes.DealIntoStack => deckSize - cardIndex - 1,
          _ => throw new ApplicationException($"Unknown shuffle type")
        };
      }
      // foreach (var (type, amount) in sequence) {
      // cardIndex = type switch {
      //   ShuffleTypes.Cut => (cardIndex + amount) % deckSize,
      //   ShuffleTypes.DealWithIncrement => InverseDealWith(cardIndex, deckSize, amount),
      //   ShuffleTypes.DealIntoStack => deckSize - cardIndex - 1,
      //   _ => throw new ApplicationException($"Unknown shuffle type")
      // };
      // }
      Console.WriteLine($"{counter,8}{cardIndex,18}");
      // if (cardIndex == 2020) {
      //   Console.WriteLine($"We have a repeat of {cardIndex} at {counter}");
      //   return counter;
      // }

      // if (counter % 1000000 == 0)
      //   Console.WriteLine($"Counter:{counter}");
    }
    // 5676820183992 too low
    // 5403293420776 too low
    return cardIndex;
  }

  private static long InverseDealWith(long cardIndex, long deckSize, long increment)
  {
    while (cardIndex % increment != 0) {
      cardIndex += deckSize;
    }
    return cardIndex / increment;
  }

  public static long ModInverse(long a, long m)
  {
    if (m == 1) return 0;
    long m0 = m;
    long x = 1, y = 0;

    while (a > 1) {
      long q = a / m;
      (a, m) = (m, a % m);
      (x, y) = (y, x - q * y);
    }

    return x < 0 ? x + m0 : x;
  }

  private static List<(ShuffleTypes type, long amount)> GetSequence(string data, long deckSize)
  {
    var completeOrder = data.Split('\n', StringSplitOptions.RemoveEmptyEntries);
    var sequence = new List<(ShuffleTypes type, long amount)>();

    foreach (var shuffle in completeOrder) {
      if (shuffle.StartsWith("cut")) {
        long amount = int.Parse(shuffle.Split(' ').Last());
        while (amount < 0)
          amount += deckSize;
        sequence.Add((ShuffleTypes.Cut, amount));
      } else if (shuffle.StartsWith("deal with")) {
        long amount = int.Parse(shuffle.Split(' ').Last());
        sequence.Add((ShuffleTypes.DealWithIncrement, amount));
      } else if (shuffle.StartsWith("deal into")) {
        sequence.Add((ShuffleTypes.DealIntoStack, 0));
      } else
        throw new UnreachableException($"Unknown shuffle:{shuffle}");
    }

    return sequence;

  }
}
