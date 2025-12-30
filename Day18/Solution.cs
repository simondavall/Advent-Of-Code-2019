using AocHelper;

namespace Day18;

internal static partial class Program
{
  private const string Title = "\n## Day 18: Many-Worlds Interpretation ##";
  private const string AdventOfCode = "https://adventofcode.com/2019/day/18";
  private const long ExpectedPartOne = 4844;
  private const long ExpectedPartTwo = 1784;

  private static (int dx, int dy)[] _directions = [(0, -1), (1, 0), (0, 1), (-1, 0)];

  private static int _maxKeys = 0;
  private static Dictionary<char, (int r, int c)> _keys = [];
  private static int _mapHeight;
  private static int _mapWidth;

  private static long PartOne(string[] data)
  {
    var (map, start) = ProcessData(data);
    map[start.r, start.c] = '.';

    var q = new Queue<(int x, int y, Keys keys, int depth)>();
    q.Enqueue((start.r, start.c, new Keys(), 0));

    var seen = new HashSet<(int, int, string)>();
    while (q.Count > 0) {
      var (r, c, keys, depth) = q.Dequeue();

      var ch = map[r, c];
      if (ch >= 'A' && ch < +'Z' && !keys.Contains(char.ToLower(ch))) {
        continue;
      }

      Keys? newKeys = null;
      if (ch >= 'a' && ch <= 'z') {
        newKeys = keys.Clone();
        newKeys.Add(ch);
        if (newKeys.Length == _maxKeys)
          return depth;
      }

      var nkeys = newKeys ?? keys;
      foreach (var (dr, dc) in _directions) {
        var (nr, nc) = (r + dr, c + dc);
        if (map[nr, nc] == '#') {
          continue;
        }

        var cacheKey = (nr, nc, nkeys.ToString());
        if (seen.Contains(cacheKey)) {
          continue;
        }
        seen.Add(cacheKey);

        q.Enqueue((nr, nc, nkeys, depth + 1));
      }
    }
    return -1;
  }

  private static long PartTwo(string[] data, string input)
  {
    var (map, start) = ProcessData(data);

    int minSteps = int.MaxValue;

    var robots = AlterMap(map, start);
    Dictionary<char, bool> foundKeys = _keys.ToDictionary(kl => kl.Key, kl => false);

    var stepsFromKeyToKeys = new Dictionary<int, Dictionary<char, Dictionary<char, (int Steps, string DoorsBetween)>>>();

    for (int i = 0; i < robots.Count; i++) {
      var stepsBetweenKeys = new Dictionary<char, Dictionary<char, (int Steps, string DoorsBetween)>>();

      var q = new Queue<(int r, int c)>();
      q.Enqueue(robots[i]);

      while (q.Count > 0) {
        (int r, int c) keyPosition = q.Dequeue();
        char key = map[keyPosition.r, keyPosition.c];

        var reachableKeys = new Dictionary<char, (int Steps, string DoorsBetween)>();

        GetStepsBetweenKeys(map, keyPosition, 0, string.Empty, new Dictionary<string, int>(), reachableKeys);

        reachableKeys.Remove(key);
        stepsBetweenKeys[key] = reachableKeys;

        foreach (char reachableKey in reachableKeys.Keys) {
          if (!stepsBetweenKeys.ContainsKey(reachableKey)) {
            q.Enqueue(_keys[reachableKey]);
          }
        }
      }

      stepsFromKeyToKeys[i] = stepsBetweenKeys;
    }

    GetShortestPath(map, 0, robots, stepsFromKeyToKeys, new Dictionary<string, int>(), foundKeys, 0, ref minSteps);

    return minSteps;
  }

  private static void GetShortestPath(
      char[,] tunnelsMap,
      int currentRobot,
      List<(int, int)> robots,
      Dictionary<int, Dictionary<char, Dictionary<char, (int Steps, string DoorsBetween)>>> stepsFromKeyToKeys,
      Dictionary<string, int> statesCache,
      Dictionary<char, bool> foundKeys,
      int steps,
      ref int minSteps
  )
  {
    if (steps >= minSteps) {
      return;
    }

    string remainingKeys = string.Concat(foundKeys.Where(fk => fk.Value == false).Select(fk => fk.Key));
    string state = StringifyState(remainingKeys, currentRobot, robots);

    if (statesCache.ContainsKey(state) && steps >= statesCache[state]) {
      return;
    }
    statesCache[state] = steps;

    if (!foundKeys.Where(fk => fk.Value == false).Any()) {
      minSteps = steps;
      return;
    }

    var (r, c) = robots[currentRobot];
    char currentKey = tunnelsMap[r, c];

    var reachableKeys = GetReachableKeys(stepsFromKeyToKeys[currentRobot][currentKey], foundKeys);

    if (reachableKeys.Count > 0) {
      foreach (KeyValuePair<char, int> reachableKey in reachableKeys) {
        char nextKey = reachableKey.Key;
        int newSteps = steps + reachableKey.Value;

        Dictionary<char, bool> nextFoundKeys = foundKeys.ToDictionary(fk => fk.Key, fk => fk.Value);
        nextFoundKeys[nextKey] = true;

        List<(int r, int c)> nextRobotsPositions = robots.ToList();
        nextRobotsPositions[currentRobot] = _keys[nextKey];

        GetShortestPath(
            tunnelsMap,
            currentRobot,
            nextRobotsPositions,
            stepsFromKeyToKeys,
            statesCache,
            nextFoundKeys,
            newSteps,
            ref minSteps
        );
      }
    }

    for (int nextRobot = 0; nextRobot < robots.Count; nextRobot++) {
      if (nextRobot != currentRobot) {
        GetShortestPath(
            tunnelsMap,
            nextRobot,
            robots.ToList(),
            stepsFromKeyToKeys,
            statesCache,
            foundKeys.ToDictionary(fk => fk.Key, fk => fk.Value),
            steps,
            ref minSteps
        );
      }
    }
  }

  private static Dictionary<char, int> GetReachableKeys(Dictionary<char, (int Steps, string DoorsBetween)> stepsToKey, Dictionary<char, bool> foundKeys)
  {
    var reachableKeys = new Dictionary<char, int>();

    foreach (var (key, (steps, doorsBetween)) in stepsToKey) {
      if (foundKeys[key])
        continue;

      bool doorLocked = false;
      foreach (char door in doorsBetween) {
        if (!foundKeys[door]) {
          doorLocked = true;
          break;
        }
      }

      if (!doorLocked)
        reachableKeys.Add(key, steps);
    }

    return reachableKeys;
  }

  private static void GetStepsBetweenKeys(
      char[,] map,
      (int, int) position,
      int steps,
      string doors,
      Dictionary<string, int> statesCache,
      Dictionary<char, (int Steps, string DoorsBetween)> reachableKeys
  )
  {
    var (r, c) = position;
    string state = $"{r},{c}";

    if (statesCache.ContainsKey(state) && steps >= statesCache[state]) {
      return;
    }
    statesCache[state] = steps;

    if (char.IsLower(map[r, c])) {
      reachableKeys[map[r, c]] = (steps, doors.ToLower());
    }

    steps++;
    foreach (var (dr, dc) in _directions) {
      var (nr, nc) = (r + dr, c + dc);

      if (nr >= 0 && nr < _mapHeight && nc >= 0 && nc < _mapWidth && map[nr, nc] != '#') {
        string newDoors = doors;
        if (char.IsUpper(map[nr, nc])) {
          newDoors += map[nr, nc];
        }

        GetStepsBetweenKeys(map, (nr, nc), steps, newDoors, statesCache, reachableKeys);
      }
    }
  }

  private static string StringifyState(string remainingKeys, int currentRobot, List<(int, int)> robots)
  {
    return $"({remainingKeys}),({currentRobot}),({string.Join(",", robots.ToArray())})";
  }

  private static List<(int r, int c)> AlterMap(char[,] map, (int r, int c) start)
  {
    var (r, c) = start;
    map[r - 1, c - 1] = '@';
    map[r - 1, c] = '#';
    map[r - 1, c + 1] = '@';
    map[r, c - 1] = '#';
    map[r, c] = '#';
    map[r, c + 1] = '#';
    map[r + 1, c - 1] = '@';
    map[r + 1, c] = '#';
    map[r + 1, c + 1] = '@';

    return new List<(int, int)>([(r - 1, c - 1), (r - 1, c + 1), (r + 1, c + 1), (r + 1, c - 1)]);
  }

  private static (char[,] map, (int r, int c) start) ProcessData(string[] data)
  {
    _maxKeys = 0;
    _keys = [];
    _mapHeight = data.Length;
    _mapWidth = data[0].Length;
    var map = new char[_mapHeight, _mapWidth];
    var start = (-1, -1);
    foreach (var (r, line) in data.Index()) {
      foreach (var (c, ch) in line.Index()) {
        map[r, c] = ch;
        if (ch >= 'a' && ch <= 'z') {
          _keys.Add(ch, (r, c));
          _maxKeys++;
        } else if (ch == '@')
          start = (r, c);
      }
    }
    return (map, start);
  }

  private class Keys()
  {
    private List<char> _keys = [];
    private List<(char, int)> _unsortedKeys = [];
    private int _depth = 0;
    private string _strKeys = ""; // used for ToString(). Called many times.

    internal int Length => _keys.Count;
    internal void Add(char key, int depth = 0)
    {
      if (!_keys.Contains(key)) {
        _unsortedKeys.Add((key, depth));
        _keys.Add(key);
        _keys.Sort();
        _strKeys = string.Join("", _keys);
        _depth = depth;
      }
    }
    internal List<char> Items => _keys;
    internal List<(char, int)> UnsortedItems => _unsortedKeys;
    internal bool Contains(char key) => _keys.Contains(key);
    internal bool Contains(Keys keys)
    {
      foreach (var key in keys.Items) {
        if (!_keys.Contains(key))
          return false;
      }
      return true;
    }
    internal Keys Clone()
    {
      var clone = new Keys() {
        _unsortedKeys = _unsortedKeys.ToList(),
        _keys = _keys.ToList(),
        _strKeys = _strKeys,
        _depth = _depth
      };
      return clone;
    }

    public override int GetHashCode()
    {
      return _strKeys.GetHashCode();
    }

    public override string ToString()
    {
      return _strKeys;
    }
  }
}
