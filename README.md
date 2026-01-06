# Advent Of Code 2019 #
Advent of Code 2019 written using C# (.Net 10.0)

<img src="./Aoc2019.png" alt="Christmas is on track!!" width="600" />

### To run all 25 days: ###
1. Install .Net 10.0 sdk/runtime.
2. Navigate to the project root.
3. Execute the make file: 
```bash
make
```
or directly from the root using 
```bash
cd Aoc && dotnet build -c Release && ./bin/Release/net10.0/Aoc
```
This will build all days' projects in Release mode, and execute them.
The results will be displayed in the terminal window, with execution timings for each.

### To run an individual day: ###
1. Install .Net 10.0 sdk/runtime.
2. Navigate to the day (e.g. /Day01)
3. Execute the make file to build (Debug mode) and run: 
```bash
make
```
or directly from within a day root using (change Day$$ to the required day. E.g Day01) 
```bash
dotnet build -c Debug && ./bin/Debug/net10.0/Day$$ input.txt
```
Individual days are built and run in Debug mode to trigger assert statements that are not available in Release mode.


# All results with execution timings #

## Day 1: The Tyranny of the Rocket Equation ##
https://adventofcode.com/2019/day/1

File: ../Day01/input.txt

Part 1 Result: 3226822 in 0.5252ms\
Part 2 Result: 4837367 in 0.1916ms\
Great Success!!!


## Day 2: 1202 Program Alarm ##
https://adventofcode.com/2019/day/2

File: ../Day02/input.txt

Part 1 Result: 6568671 in 2.7601ms\
Part 2 Result: 3951 in 41.0049ms\
Great Success!!!


## Day 3: Crossed Wires ##
https://adventofcode.com/2019/day/3

File: ../Day03/input.txt

Part 1 Result: 1431 in 39.3447ms\
Part 2 Result: 48012 in 43.9056ms\
Great Success!!!


## Day 4: Secure Container ##
https://adventofcode.com/2019/day/4

Part 1 Result: 2050 in 23.3894ms\
Part 2 Result: 1390 in 71.7987ms\
Great Success!!!


## Day 5: Sunny with a Chance of Asteroids ##
https://adventofcode.com/2019/day/5

File: ../Day05/input.txt

Full output: [0, 0, 0, 0, 0, 0, 0, 0, 0, 10987514]\
Part 1 Result: 10987514 in 5.0869ms\
Part 2 Result: 14195011 in 0.7343ms\
Great Success!!!


## Day 6: Universal Orbit Map ##
https://adventofcode.com/2019/day/6

File: ../Day06/input.txt

Part 1 Result: 119831 in 1.3913ms\
Part 2 Result: 322 in 2.0319ms\
Great Success!!!


## Day 7: Amplification Circuit ##
https://adventofcode.com/2019/day/7

File: ../Day07/input.txt

Part 1 Result: 880726 in 5.6173ms\
Part 2 Result: 4931744 in 7.4754ms\
Great Success!!!


## Day 8: Space Image Format ##
https://adventofcode.com/2019/day/8

File: ../Day08/input.txt

Part 1 Result: 2016 in 5.1619ms
```
##    ##  ########    ####    ########  ##    ##  
##    ##        ##  ##    ##        ##  ##    ##  
########      ##    ##            ##    ##    ##  
##    ##    ##      ##          ##      ##    ##  
##    ##  ##        ##    ##  ##        ##    ##  
##    ##  ########    ####    ########    ####    
```
Part 2 Result: 0 in 0.9754ms\
Great Success!!!


## Day 9: Sensor Boost ##
https://adventofcode.com/2019/day/9

File: ../Day09/input.txt

Part 1 Result: 3638931938 in 0.4511ms\
Part 2 Result: 86025 in 19.4365ms\
Great Success!!!


## Day 10: Monitoring Station ##
https://adventofcode.com/2019/day/10

File: ../Day10/input.txt

Part 1 Result: 227 in 18.8736ms\
Part 2 Result: 604 in 12.1254ms\
Great Success!!!


## Day 11: Space Police ##
https://adventofcode.com/2019/day/11

File: ../Day11/input.txt

Part 1 Result: 1885 in 25.3172ms

```
    ######    ########  ########    ####      ####    ##    ##    ####    ########        
    ##    ##  ##        ##        ##    ##  ##    ##  ##    ##  ##    ##  ##              
    ######    ######    ######    ##    ##  ##        ########  ##    ##  ######          
    ##    ##  ##        ##        ########  ##  ####  ##    ##  ########  ##              
    ##    ##  ##        ##        ##    ##  ##    ##  ##    ##  ##    ##  ##              
    ######    ##        ########  ##    ##    ######  ##    ##  ##    ##  ##              
```

Part 2 Result: 0 in 4.3133ms\
Great Success!!!


## Day 12: The N-Body Problem ##
https://adventofcode.com/2019/day/12

File: ../Day12/input.txt

Part 1 Result: 7471 in 5.1714ms\
Part 2 Result: 376243355967784 in 109.4836ms\
Great Success!!!


## Day 13: Care Package ##
https://adventofcode.com/2019/day/13

File: ../Day13/input.txt

Part 1 Result: 273 in 1.2203ms\
Part 2 Result: 13140 in 112.4864ms\
Great Success!!!


## Day 14: Space Stoichiometry ##
https://adventofcode.com/2019/day/14

File: ../Day14/input.txt

Part 1 Result: 443537 in 8.222ms\
Part 2 Result: 2910558 in 99.8669ms\
Great Success!!!


## Day 15: Oxygen System ##
https://adventofcode.com/2019/day/15

File: ../Day15/input.txt

Part 1 Result: 294 in 25.2493ms\
Part 2 Result: 388 in 64.8695ms\
Great Success!!!


## Day 16: Flawed Frequency Transmission ##
https://adventofcode.com/2019/day/16

File: ../Day16/input.txt

Part 1 Result: 82525123 in 701.9967ms\
Part 2 Result: 49476260 in 2248.8246ms\
Great Success!!!


## Day 17: Set and Forget ##
https://adventofcode.com/2019/day/17

File: ../Day17/input.txt

```
....................................#########
....................................#.......#
....................................#.......#
....................................#.......#
........................#######.....#.......#
........................#.....#.....#.......#
........................#.....#.####O########
........................#.....#.#...#........
........................#.....#.#...#######..
........................#.....#.#.........#..
........................#.....#.#.........#..
........................#.....#.#.........#..
........................######O##.........#..
..............................#...........#..
..............................#...........#..
..............................#...........#..
..............................######^.....#..
..........................................#..
..........................................#..
..........................................#..
..................................#########..
..................................#..........
............#########.............#..........
............#.......#.............#..........
..........##O#######O##.........##O####......
..........#.#.......#.#.........#.#...#......
....#####.#.#.......#.#.........#.#...#......
....#...#.#.#.......#.#.........#.#...#......
####O###O#O##.......#.#...#####.#.#####......
#...#...#.#.........#.#...#...#.#............
#...####O##.........##O###O###O##............
#.......#.............#...#...#..............
#.......#.............####O####..............
#.......#.................#..................
#########.................#######............
................................#............
................................#............
................................#............
..............................##O######......
................................#.....#......
................................#.....#......
................................#.....#......
................................#.....#......
................................#.....#......
................................#.....#......
................................#.....#......
................................#######......
```

Part 1 Result: 7404 in 17.4756ms\
Part 2 Result: 929045 in 9.7739ms\
Great Success!!!


## Day 18: Many-Worlds Interpretation ##
https://adventofcode.com/2019/day/18

File: ../Day18/input.txt

Part 1 Result: 4844 in 1004.4028ms\
Part 2 Result: 1784 in 3811.7261ms\
Great Success!!!


## Day 19: Tractor Beam ##
https://adventofcode.com/2019/day/19

File: ../Day19/input.txt

Part 1 Result: 226 in 231.4161ms\
Found Square at x:790, y:946\
Part 2 Result: 7900946 in 160.9286ms\
Great Success!!!


## Day 20: Donut Maze ##
https://adventofcode.com/2019/day/20

File: ../Day20/input.txt

Part 1 Result: 400 in 39.4271ms\
Part 2 Result: 4986 in 135.6287ms\
Great Success!!!


## Day 21: Springdroid Adventure ##
https://adventofcode.com/2019/day/21

File: ../Day21/input.txt

Part 1 Result: 19350938 in 4.9955ms\
Part 2 Result: 1142986901 in 45.2506ms\
Great Success!!!


## Day 22: Slam Shuffle ##
https://adventofcode.com/2019/day/22

File: ../Day22/input.txt

Part 1 Result: 6696 in 2.2104ms\
Part 2 Result: 93750418158025 in 66.4942ms\
Great Success!!!


## Day 23: Category Six ##
https://adventofcode.com/2019/day/23

File: ../Day23/input.txt

Part 1 Result: 17949 in 4.693ms\
Part 2 Result: 12326 in 38.012ms\
Great Success!!!


## Day 24: Planet of Discord ##
https://adventofcode.com/2019/day/24

File: ../Day24/input.txt

Part 1 Result: 24662545 in 3.5533ms\
Part 2 Result: 2063 in 490.5132ms\
Great Success!!!


## Day 25: Cryostasis ##
https://adventofcode.com/2019/day/25

File: ../Day25/input.txt


A loud, robotic voice says "Analysis complete! You may proceed." and you enter the cockpit.\
Santa notices your small droid, looks puzzled for a moment, realizes what has happened, and radios your ship directly.\
"Oh, hello! You should be able to get in by typing 4206594 on the keypad at the main airlock."\
Part 1 Result: 4206594 in 24.2829ms\
Part 2 Result: 0 in 0.0342ms\
Great Success!!!


All solutions ran in (ms): 9996
25/25 solutions passed successfully!.
