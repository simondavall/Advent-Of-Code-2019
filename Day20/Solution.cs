namespace Day20;

internal static partial class Program
{
  private const string Title = "\n## Day 20: Donut Maze ##";
  private const string AdventOfCode = "https://adventofcode.com/2019/day/20";
  private const long ExpectedPartOne = 400;
  private const long ExpectedPartTwo = 4986;

  private static readonly (int dx, int dy)[] _directions = [(0, -1), (1, 0), (0, 1), (-1, 0)];

  private static long PartOne(string data)
  {
    var map = GetMap(data);
    var height = map.Length;
    var width = map[0].Length;

    var (start, end, portals) = GetPortals(map);

    var seen = new HashSet<(int, int)>();
    var q = new PriorityQueue<(int x, int y), int>();
    q.Enqueue(start, 0);

    long shortestPath = 0;
    while (q.Count > 0) {
      if (!q.TryDequeue(out var current, out var depth))
        break;

      if (current == end) {
        shortestPath = depth;
        break;
      }

      foreach (var (dx, dy) in _directions) {
        var (nx, ny) = (current.x + dx, current.y + dy);
        if (!IsInBounds(nx, ny, height, width) || map[ny][nx] != '.' || seen.Contains((nx, ny)))
          continue;
        seen.Add((nx, ny));
        // if we hit a portal. Jump to the 'other side' and add extra jump step
        if (portals.TryGetValue((nx, ny), out var exit))
          q.Enqueue(exit, depth + 2);
        else
          q.Enqueue((nx, ny), depth + 1);
      }
    }
    return shortestPath;
  }

  private static long PartTwo(string data)
  {
    var map = GetMap(data);
    var height = map.Length;
    var width = map[0].Length;

    // this optimization doctors the map to remove all the dead ends and
    // leave only the paths between portals. It's not really needed, but it 
    // makes the solution ~50% faster.
    OptimizeMap(map);

    var (start, end, portals) = GetPortals(map);

    var seen = new HashSet<(int, int, int)>();
    var q = new PriorityQueue<((int x, int y), int level), int>();
    q.Enqueue((start, 0), 0);

    long shortestPath = 0;
    while (q.Count > 0) {
      if (!q.TryDequeue(out var item, out var depth))
        break;

      var (current, level) = item;
      if (current == end && level == 0) {
        shortestPath = depth;
        break;
      }

      foreach (var (dx, dy) in _directions) {
        var (nx, ny) = (current.x + dx, current.y + dy);
        if (!IsInBounds(nx, ny, height, width) || map[ny][nx] != '.' || seen.Contains((nx, ny, level)))
          continue;
        seen.Add((nx, ny, level));
        // if we hit a portal. Jump to the 'other side' and add extra jump step
        if (portals.TryGetValue((nx, ny), out var exit)) {
          var isOuterPortal = IsOuterEdgePortal(nx, ny, height, width);

          if (isOuterPortal && level == 0) // outer portals at level 0 are walls
            continue;

          var newLevel = isOuterPortal ? level - 1 : level + 1;
          q.Enqueue((exit, newLevel), depth + 2);
        } else
          q.Enqueue(((nx, ny), level), depth + 1);
      }
    }
    return shortestPath;
  }

  private static bool IsOuterEdgePortal(int x, int y, int height, int width)
  {
    var offset = 2;
    return x == offset || x == width - (offset + 1) || y == offset || y == height - (offset + 1);
  }

  private static char[][] GetMap(string data)
  {
    var lines = data.Split('\n', StringSplitOptions.RemoveEmptyEntries);
    var map = new char[lines.Length][];
    for (var y = 0; y < lines.Length; y++) {
      map[y] = lines[y].ToCharArray();
    }
    return map;
  }

  private static ((int x, int y) start, (int x, int y) end, Dictionary<(int x, int y), (int x, int y)> portals) GetPortals(char[][] map)
  {
    var start = (-1, -1);
    var end = (-1, -1);
    var height = map.Length;
    var width = map[0].Length;
    var seen = new HashSet<(int, int)>();
    var portalLocations = new Dictionary<string, List<(int x, int y)>>();
    for (var y = 0; y < height; y++) {
      for (var x = 0; x < width; x++) {
        if (seen.Contains((x, y)))
          continue;
        var ch1 = map[y][x];
        if (char.IsUpper(ch1)) {
          seen.Add((x, y));
          var (neighbour, entrance) = FindNeighbourAndPortalAddress(x, y, map);
          if (neighbour == (-1, -1))
            throw new ApplicationException($"Something went terribly wrong looking for portal neighbour to {ch1}");
          seen.Add(neighbour);
          if (entrance == (-1, -1)) {
            (_, entrance) = FindNeighbourAndPortalAddress(neighbour.x, neighbour.y, map);
          }
          var ch2 = map[neighbour.y][neighbour.x];
          var portalName = $"{ch1}{ch2}";
          if (portalName == "AA")
            start = entrance;
          else if (portalName == "ZZ")
            end = entrance;
          else {
            if (portalLocations.ContainsKey(portalName))
              portalLocations[portalName].Add(entrance);
            else
              portalLocations.Add(portalName, [entrance]);
          }
        }
      }
    }
    var portals = new Dictionary<(int, int), (int, int)>();
    foreach (var (name, locations) in portalLocations) {
      if (locations.Count() != 2)
        throw new ApplicationException($"Expected 2 locations, found:{locations.Count()} for portal:{name}");
      // add portals in both directions
      portals.Add(locations[0], locations[1]);
      portals.Add(locations[1], locations[0]);
    }
    return (start, end, portals);
  }

  private static ((int x, int y), (int x, int y)) FindNeighbourAndPortalAddress(int x, int y, char[][] map)
  {
    var neighbour = (-1, -1);
    var entrance = (-1, -1);
    foreach (var (dx, dy) in _directions) {
      var (nx, ny) = (x + dx, y + dy);
      if (!IsInBounds(nx, ny, map.Length, map[0].Length))
        continue;
      if (char.IsUpper(map[ny][nx])) {
        neighbour = (nx, ny);
      } else if (map[ny][nx] == '.') {
        entrance = (nx, ny);
      }
    }
    return (neighbour, entrance);
  }

  private static bool IsInBounds(int x, int y, int height, int width)
  {
    return 0 <= x && x < width && 0 <= y && y < height;
  }

  private static void OptimizeMap(char[][] map)
  {
    var height = map.Length;
    var width = map[0].Length;

    var hasChanged = true;
    while (hasChanged) {
      hasChanged = false;
      for (var y = 0; y < height; y++) {
        for (var x = 0; x < width; x++) {
          if (map[y][x] == '.') {
            int wallCount = 0;
            foreach (var (dx, dy) in _directions) {
              if (map[y + dy][x + dx] == '#')
                wallCount++;
            }
            if (wallCount >= 3) {
              map[y][x] = '#';
              hasChanged = true;
            }
          }
        }
      }
    }
  }
}
