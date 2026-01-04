using System.Diagnostics;
using AocHelper;

namespace Day24;

internal static partial class Program
{
  private const string Title = "\n## Day 24: Planet of Discord ##";
  private const string AdventOfCode = "https://adventofcode.com/2019/day/24";
  private const long ExpectedPartOne = 24662545;
  private const long ExpectedPartTwo = 2063;

  private static readonly (int dx, int dy)[] _directions = [(0, -1), (1, 0), (0, 1), (-1, 0)];

  private static long PartOne(string data)
  {
    var lines = data.Split('\n', StringSplitOptions.RemoveEmptyEntries);
    var height = lines.Length;
    var width = lines[0].Length;
    var grid = new char[height][];
    var tileBioValue = 0;
    for (var y = height - 1; y >= 0; y--) {
      grid[y] = new char[width];
      for (var x = width - 1; x >= 0; x--) {
        var ch = lines[y][x];
        tileBioValue <<= 1;
        if (ch == '#') {
          tileBioValue++;
        }
        grid[y][x] = ch;
      }
    }

    var seen = new HashSet<long>(tileBioValue);
    var t = 0;
    while (true) {
      var newGrid = new char[height][];
      tileBioValue = 0;
      for (var y = height - 1; y >= 0; y--) {
        newGrid[y] = new char[width];
        for (var x = width - 1; x >= 0; x--) {
          var ch = grid[y][x];
          tileBioValue <<= 1;
          if (ch == '#') {
            tileBioValue++;
          }
          var adjacent = 0;
          foreach (var (dx, dy) in _directions) {
            var (nx, ny) = (x + dx, y + dy);
            if (IsInBounds(nx, ny, width, height) && grid[ny][nx] == '#')
              adjacent++;
          }
          if (ch == '#')
            newGrid[y][x] = adjacent == 1 ? '#' : '.';
          else
            newGrid[y][x] = adjacent == 1 || adjacent == 2 ? '#' : '.';
        }
      }
      if (seen.Contains(tileBioValue))
        return tileBioValue;
      seen.Add(tileBioValue);
      grid = newGrid;
      t++;
    }
  }

  private static long PartTwo(string data)
  {
    var minutes = 200; // change to 10 for sample comparison to aoc example.
    var grids = new Dictionary<int, char[][]>();
    grids.Add(0, CreateGrid(data));
    var height = grids[0].Length;
    var width = grids[0][0].Length;
    grids[0][height / 2][width / 2] = '?';
    grids.Add(-1, GetNewEmptyGrid(height, width));
    grids.Add(1, GetNewEmptyGrid(height, width));
    var midPoint = (width / 2, height / 2);

    var t = 0;
    while (t++ < minutes) {
      var newGrids = new Dictionary<int, char[][]>();
      var (low, high) = (grids.Keys.Min(), grids.Keys.Max());

      for (var z = low; z <= high; z++) {
        var grid = grids[z];
        var isCurrentGridEmpty = IsGridEmpty(grid);

        var newGrid = GetNewEmptyGrid(height, width);
        for (var y = 0; y < height; y++) {
          for (var x = 0; x < width; x++) {
            if ((x, y) == midPoint)
              continue;
            var ch = grid[y][x];
            var adjacent = 0;
            foreach (var (dx, dy) in _directions) {
              var (nx, ny) = (x + dx, y + dy);
              var level = GetLevel(nx, ny, width, height);
              switch (level) {
                case -1: // check outside grid
                  if (z <= grids.Keys.Min()) {
                    if (!isCurrentGridEmpty) {
                      grids[z - 1] = GetNewEmptyGrid(height, width);
                      newGrids[z - 1] = GetNewEmptyGrid(height, width);
                    }
                  } else {
                    adjacent += CheckOuterGrid(nx, ny, height, width, grids[z - 1]);
                  }
                  break;
                case 1: // check inside grid
                  if (z >= grids.Keys.Max()) {
                    if (!isCurrentGridEmpty) {
                      grids[z + 1] = GetNewEmptyGrid(height, width);
                      newGrids[z + 1] = GetNewEmptyGrid(height, width);
                    }
                  } else {
                    adjacent += CheckInnerGrid(x, y, height, width, grids[z + 1]);
                  }
                  break;
                case 0:
                  if (IsInBounds(nx, ny, width, height) && grid[ny][nx] == '#')
                    adjacent++;
                  break;
                default:
                  throw new UnreachableException("Can't reach here!");
              }
              if ((ch == '#' && adjacent > 1) || adjacent > 2)
                break; // optimization, break early of already know outcome
            }
            if (ch == '#')
              newGrid[y][x] = adjacent == 1 ? '#' : '.';
            else
              newGrid[y][x] = adjacent == 1 || adjacent == 2 ? '#' : '.';
          }
        }
        newGrids[z] = newGrid;
      }
      grids = newGrids;
    }

    long tally = 0;
    foreach (var (idx, grid) in grids) {
      for (var y = 0; y < height; y++)
        for (var x = 0; x < width; x++)
          if (grid[y][x] == '#')
            tally++;
    }
    return tally;
  }

  private static int CheckInnerGrid(int x, int y, int height, int width, char[][] grid)
  {
    var midPoint = height / 2;
    var bugs = 0;
    if (y < midPoint) // top
      foreach (char ch in grid[0]) {
        bugs += ch == '#' ? 1 : 0;
      }
    else if (y > midPoint) // bottom
      foreach (var ch in grid[height - 1])
        bugs += ch == '#' ? 1 : 0;
    else if (x < midPoint) // left
      for (var i = 0; i < height; i++)
        bugs += grid[i][0] == '#' ? 1 : 0;
    else // right
      for (var i = 0; i < height; i++)
        bugs += grid[i][width - 1] == '#' ? 1 : 0;

    return bugs;
  }

  private static int CheckOuterGrid(int x, int y, int height, int width, char[][] grid)
  {
    var midPoint = height / 2;
    if (y < 0) // top
      return grid[midPoint - 1][midPoint] == '#' ? 1 : 0;
    else if (y >= height) // bottom
      return grid[midPoint + 1][midPoint] == '#' ? 1 : 0;
    else if (x < 0) // left
      return grid[midPoint][midPoint - 1] == '#' ? 1 : 0;
    else // right
      return grid[midPoint][midPoint + 1] == '#' ? 1 : 0;
  }

  private static int GetLevel(int x, int y, int width, int height)
  {
    var midPoint = height / 2;
    if (!IsInBounds(x, y, width, height))
      return -1;
    else if (x == midPoint && y == midPoint)
      return 1;
    else
      return 0;
  }

  private static char[][] GetNewEmptyGrid(int height, int width)
  {
    var emptyGrid = new char[height][];
    for (var y = 0; y < height; y++) {
      emptyGrid[y] = Helper.CreateArray(width, '.');
    }
    emptyGrid[height / 2][width / 2] = '?';
    return emptyGrid;

  }

  private static bool IsGridEmpty(char[][] grid)
  {
    for (var y = 0; y < grid.Length; y++)
      if (grid[y].IndexOf('#') > -1)
        return false;
    return true;
  }

  private static bool IsInBounds(int x, int y, int width, int height)
  {
    return 0 <= x && x < width && 0 <= y && y < height;
  }

  private static char[][] CreateGrid(string data)
  {
    var lines = data.Split('\n', StringSplitOptions.RemoveEmptyEntries);
    var grid = new char[lines.Length][];
    for (var y = 0; y < lines.Length; y++)
      grid[y] = lines[y].ToCharArray();

    return grid;
  }
}
