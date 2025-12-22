
using AocHelper;

namespace Day18;

internal static partial class Program
{
  private const string Title = "\n## Day 18: Many-Worlds Interpretation ##";
  private const string AdventOfCode = "https://adventofcode.com/2019/day/18";
  private const long ExpectedPartOne = 0;
  private const long ExpectedPartTwo = 0;

  private static (int dx, int dy)[] _directions = [(0, -1), (1, 0), (0, 1), (-1, 0)];

  private static int maxKeys = 0;


  private static long PartOne(string[] data)
  {

    var (map, start, _) = GetMap(data);

    var q = new Queue<(int x, int y, Keys keys, int depth)>();
    q.Enqueue((start.x, start.y, new Keys(), 0));

    var seen = new HashSet<(int, int, string)>();
    while (q.Count > 0) {
      var (x, y, keys, depth) = q.Dequeue();

      if (map[y][x] == '#') {
        continue;
      }

      var cacheKey = (x, y, keys.ToString());
      if (seen.Contains(cacheKey)) {
        continue;
      }
      seen.Add(cacheKey);

      var ch = map[y][x];
      if (ch >= 'A' && ch < +'Z' && !keys.Contains(char.ToLower(ch))) {
        continue;
      }

      Keys? newKeys = null;
      if (ch >= 'a' && ch <= 'z') {
        newKeys = keys.Clone();
        newKeys.Add(ch);
        if (newKeys.Length == maxKeys)
          return depth;
      }

      foreach (var (dx, dy) in _directions) {
        var (nx, ny) = (x + dx, y + dy);
        var nkeys = newKeys ?? keys;
        q.Enqueue((nx, ny, nkeys, depth + 1));
      }
    }
    return -1;
  }

  private static long PartTwo(string[] data)
  {
    var (map, start, _) = GetMap(data);
    var starts = AlterMap(map, start);
    var n = starts.Length;

    var bots = new List<(int id, int x, int y, Keys keys, int depth)>();
    var seen = new HashSet<(int, int, string)>();
    var counter = 0;
    while (true) {
      var currentBot = counter % n;
      var currentList = bots.Where(x => x.id == currentBot).ToList();
      // add any new key combinations
      var q = new Queue<(int x, int y, Keys keys, int depth)>(currentList.Select(x => (x.x, x.y, x.keys, x.depth)).ToArray());
      while (q.Count > 0) {
        var (x, y, keys, depth) = q.Dequeue();

        if (map[y][x] == '#') {
          continue;
        }

        var cacheKey = (x, y, keys.ToString() ?? "");
        if (seen.Contains(cacheKey)) {
          continue;
        }
        seen.Add(cacheKey);

        var ch = map[y][x];
        if (ch >= 'A' && ch < +'Z' && !keys.Contains(char.ToLower(ch))) {
          bots.Add((currentBot, x, y, keys, depth));
          continue;
        }

        Keys? newKeys = null;
        if (ch >= 'a' && ch <= 'z') {
          newKeys = keys.Clone();
          newKeys.Add(ch, depth);
          if (newKeys.Length == maxKeys)
            return depth;
          // add entry for new keys in bot list for each entry in other quadrant.
          var otherBots = bots.Where(x => x.id != currentBot);
          foreach(var o in otherBots){
            currentList.Add((o.id, o.x, o.y, newKeys, o.depth + depth));
          }
        }

        foreach (var (dx, dy) in _directions) {
          var (nx, ny) = (x + dx, y + dy);
          var nkeys = newKeys ?? keys;
          q.Enqueue((nx, ny, nkeys, depth + 1));
        }
      }
      // print status
      foreach(var bot in bots){
        Console.WriteLine($"{bot}");
      }
    }
  }

  private static (int id, int x, int y)[] AlterMap(char[][] map, (int x, int y) start)
  {
    var (x, y) = start;
    map[y - 1][x - 1] = '.';
    map[y - 1][x] = '#';
    map[y - 1][x + 1] = '.';
    map[y][x - 1] = '#';
    map[y][x] = '#';
    map[y][x + 1] = '#';
    map[y + 1][x - 1] = '.';
    map[y + 1][x] = '#';
    map[y + 1][x + 1] = '.';

    return [(0, x - 1, y - 1), (1, x + 1, y - 1), (2, x + 1, y + 1), (3, x - 1, y + 1)];
  }

  private static (char[][] map, (int x, int y) start, Keys keys) GetMap(string[] data)
  {
    var map = new List<char[]>();
    var keys = new Keys();
    var start = (-1, -1);
    foreach (var (y, line) in data.Index()) {
      foreach (var ch in line) {
        if (ch >= 'a' && ch <= 'z')
          keys.Add(ch);
        if (ch == '@')
          start = (line.IndexOf('@'), y);
      }
      map.Add(line.ToCharArray());
    }
    map[start.Item2][start.Item1] = '.';
    maxKeys = keys.Length;
    return (map.ToArray(), start, keys);
  }

  private class Keys()
  {
    private List<char> _keys = [];
    private int _depth = 0;
    private string _strKeys = ""; // used for ToString(). Called many times.

    internal int Length => _keys.Count;
    internal void Add(char key, int depth = 0)
    {
      if (!_keys.Contains(key)) {
        _keys.Add(key);
        _keys.Sort();
        _strKeys = string.Join("", _keys) + $",{depth}";
        _depth = depth;
      }
    }
    internal bool Contains(char key) => _keys.Contains(key);
    internal Keys Clone()
    {
      var clone = new Keys() {
        _keys = _keys.ToList(),
        _strKeys = _strKeys,
        _depth = _depth
      };
      return clone;
    }
    
    public override int GetHashCode(){
      return _strKeys.GetHashCode();
    }

    public override string ToString()
    {
      return _strKeys;
    }
  }

  private static void PrintMap(char[][] map)
  {
    Console.WriteLine();
    foreach (var line in map) {
      foreach (var ch in line) {
        Console.Write(ch);
      }
      Console.WriteLine();
    }
    Console.WriteLine();
  }
}
