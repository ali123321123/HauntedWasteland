using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

enum Direction
{
    LEFT = 0,
    RIGHT
}

class Map
{
    public List<Direction> Instructions { get; set; } = new List<Direction>();
    public Dictionary<string, Tuple<string, string>> Nodes { get; set; } = new Dictionary<string, Tuple<string, string>>();

    public void SetInstr(string instr)
    {
        Instructions.Clear();
        Instructions.Capacity = instr.Length;
        foreach (char c in instr)
        {
            if (c == 'R')
            {
                Instructions.Add(Direction.RIGHT);
            }
            else
            {
                Instructions.Add(Direction.LEFT);
            }
        }
    }

    public void Add(string line)
    {
        Nodes[line.Substring(0, 3)] = new Tuple<string, string>(line.Substring(7, 3), line.Substring(12, 3));
    }

    public int Steps(string pos, bool ghost)
    {
        int step = 0;

        while ((!ghost && pos != "ZZZ") || (ghost && pos[2] != 'Z'))
        {
            if (Instructions[step % Instructions.Count] == Direction.LEFT)
            {
                pos = Nodes[pos].Item1;
            }
            else
            {
                pos = Nodes[pos].Item2;
            }
            step++;
        }

        return step;
    }
}

class Program
{
    static void Main()
    {
        string test = File.ReadAllText(@"C:\testutvikling\Haunted Wasteland\Haunted Wasteland\input.txt");
        Solve(test);
    }

    static void Solve(string test)
    {
        using (StringReader reader = new StringReader(test))
        {
            string line;
            Map map = new Map();
            line = reader.ReadLine();
            map.SetInstr(line);
            line = reader.ReadLine();

            while ((line = reader.ReadLine()) != null)
            {
                map.Add(line);
            }

            Console.WriteLine("Part 1: " + Part1(map));
            Console.WriteLine("Part 2: " + Part2(map));
        }
    }

    static int Part1(Map map)
    {
        return map.Steps("AAA", false);
    }

    static long Part2(Map map)
    {
        List<int> steps = map.Nodes.Where(kv => kv.Key[2] == 'A').Select(kv => map.Steps(kv.Key, true)).ToList();

        long multiple = 1;
        foreach (int step in steps)
        {
            multiple = LCM(multiple, step);
        }

        return multiple;
    }

    static long GCD(long a, long b)
    {
        while (b != 0)
        {
            long temp = b;
            b = a % b;
            a = temp;
        }

        return a;
    }

    static long LCM(long a, long b)
    {
        return Math.Abs(a * b) / GCD(a, b);
    }
}
