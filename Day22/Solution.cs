using System.Diagnostics;
using System.Numerics;

namespace Day22;

internal static partial class Program
{
  private const string Title = "\n## Day 22: Slam Shuffle ##";
  private const string AdventOfCode = "https://adventofcode.com/2019/day/22";
  private const long ExpectedPartOne = 6696;
  private const long ExpectedPartTwo = 93750418158025;

  private static long PartOne(string data)
  {
    // var deckSize = 10; // use this for sample
    long deckSize = 10007;
    long cardIndex = 2019; // use a number between 0 and 9 for sample.

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
    long deckSize = 119315717514047;
    long startIndex = 2020;
    long cardIndex = startIndex;
    long repetitions = 101741582076661;

    var sequence = GetSequence(data, deckSize);
    sequence.Reverse();

    long result = 0;
    long counter = 0;
    while (counter++ < repetitions) {
      long prevIndex = cardIndex;
      (BigInteger A, BigInteger B) = (1, 0);

      foreach (var (type, amount) in sequence) {
        Debug.Assert(0 <= cardIndex && cardIndex < deckSize, $"Assertion 1 failed. CardIndex:{cardIndex}");
        switch (type) {

          case ShuffleTypes.Cut:
            var n = amount;
            if (n < 0)
              n = deckSize + n;
            cardIndex = (cardIndex + n) % deckSize;
            B += n;
            break;

          case ShuffleTypes.DealIntoStack:
            cardIndex = deckSize - cardIndex - 1;
            (A, B) = (-A, deckSize - B - 1);
            break;
          
          case ShuffleTypes.DealWithIncrement:
            var modInv = ModInv(amount, deckSize);
            BigInteger multiple = modInv * (BigInteger)cardIndex;
            cardIndex = (long)(multiple % deckSize);
            (A, B) = (A * modInv, B * modInv);
            break;
          
          default:
            throw new ApplicationException("Unknown shuffle type");
        }
      }

      (A, B) = (A % deckSize, B % deckSize);
      if (A < 0)
        A += deckSize;
      if (B < 0)
        B += deckSize;

      Debug.Assert(cardIndex == (A * prevIndex + B) % deckSize, "Assertion 2 failed.");

      if (counter > 10) {
        BigInteger modPowA = ModPow(A, repetitions, deckSize);
        result = (long)((startIndex * modPowA + (modPowA - 1) * B * ModInv(A - 1, deckSize)) % deckSize);
        break;
      }
    }

    return result;
  }

  private static BigInteger ModInv(BigInteger a, long m)
  {
    return ModPow(a, m - 2, m);
  }

  private static BigInteger ModPow(BigInteger b, long e, long m)
  {
    if (e == 0)
      return 1;
    else if (e == 1)
      return (long)(b % m);
    else if (e % 2 == 0)
      return ModPow(b * b % m, e / 2, m);
    else
      return (long)(b * ModPow(b, e - 1, m) % m);
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
