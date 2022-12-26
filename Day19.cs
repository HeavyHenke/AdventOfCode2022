using System.Text.RegularExpressions;

class Day19
{
    public object A()
    {
        var data = File.ReadLines("Day19.txt")
            .Select(l => Regex.Match(l, @"Blueprint (?<blueprint>\d+): Each ore robot costs (?<oreBotOreCost>\d+) ore. Each clay robot costs (?<clayBotOreCost>\d+) ore. Each obsidian robot costs (?<obsidianBotOreCost>\d+) ore and (?<obsidianBotClayCost>\d+) clay. Each geode robot costs (?<geodeBotOreCost>\d+) ore and (?<geodBotObsidianCost>\d+) obsidian."))
            .Select(m => (bpNum: byte.Parse(m.Groups["blueprint"].Value),
                oreBotOreCost: byte.Parse(m.Groups["oreBotOreCost"].Value),
                clayBotOreCost: byte.Parse(m.Groups["clayBotOreCost"].Value),
                obsidianBotOreCost: byte.Parse(m.Groups["obsidianBotOreCost"].Value),
                obsidianBotClayCost: byte.Parse(m.Groups["obsidianBotClayCost"].Value),
                geodeBotOreCost: byte.Parse(m.Groups["geodeBotOreCost"].Value),
                geodBotObsidianCost: byte.Parse(m.Groups["geodBotObsidianCost"].Value)));

        int result = 0;
        foreach (var bluePrint in data)
        {
            int obsidian = CalcMostObsidianByBluePrint(bluePrint, 24);
            Console.WriteLine($"BP {bluePrint.bpNum}: {obsidian}");
            result += obsidian * bluePrint.bpNum;
        }

        // 1522 too low
        return result;
    }

    public object B()
    {
        var data = File.ReadLines("Day19.txt")
            .Select(l => Regex.Match(l, @"Blueprint (?<blueprint>\d+): Each ore robot costs (?<oreBotOreCost>\d+) ore. Each clay robot costs (?<clayBotOreCost>\d+) ore. Each obsidian robot costs (?<obsidianBotOreCost>\d+) ore and (?<obsidianBotClayCost>\d+) clay. Each geode robot costs (?<geodeBotOreCost>\d+) ore and (?<geodBotObsidianCost>\d+) obsidian."))
            .Select(m => (bpNum: byte.Parse(m.Groups["blueprint"].Value),
                oreBotOreCost: byte.Parse(m.Groups["oreBotOreCost"].Value),
                clayBotOreCost: byte.Parse(m.Groups["clayBotOreCost"].Value),
                obsidianBotOreCost: byte.Parse(m.Groups["obsidianBotOreCost"].Value),
                obsidianBotClayCost: byte.Parse(m.Groups["obsidianBotClayCost"].Value),
                geodeBotOreCost: byte.Parse(m.Groups["geodeBotOreCost"].Value),
                geodBotObsidianCost: byte.Parse(m.Groups["geodBotObsidianCost"].Value)));

        int result = data.Take(3)
            .AsParallel()
            .Select(d => CalcMostObsidianByBluePrint(d, 32))
            .Aggregate(1, (a, b) => a * b);

        return result;
    }

    private int CalcMostObsidianByBluePrint((byte bpNum, byte oreBotOreCost, byte clayBotOreCost, byte obsidianBotOreCost, byte obsidianBotClayCost, byte geodeBotOreCost, byte geodBotObsidianCost) recepie, int maxTime)
    {
        var state = (oreBots: (byte)1, clayBots: (byte)0, obsidianBots: (byte)0, geoidBots: (byte)0, ores: (byte)0, clays: (byte)0, obsidians: (byte)0, geoids: (byte)0, time: (byte)0);
        var searchTree = new Stack<(byte oreBots, byte clayBots, byte obsidianBots, byte geoidBots, byte ores, byte clays, byte obsidians, byte geoids, byte time)>();
        searchTree.Push(state);

        var maxOreCost = new[] { recepie.oreBotOreCost, recepie.clayBotOreCost, recepie.obsidianBotOreCost, recepie.geodeBotOreCost }.Max();
        var minOreCost = new[] { recepie.oreBotOreCost, recepie.clayBotOreCost, recepie.obsidianBotOreCost, recepie.geodeBotOreCost }.Min();
        
        int maxGeoidesMined = 0;
        var visitedAtTime = new Dictionary<(byte oreBots, byte clayBots, byte obsidianBots, byte geoidBots, byte ores, byte clays, byte obsidians, byte geoids), byte>();

        while (searchTree.Count > 0)
        {
            state = searchTree.Pop();

            // Collect ores
            var nextState = state;
            nextState.ores += state.oreBots;
            nextState.clays += state.clayBots;
            nextState.obsidians += state.obsidianBots;
            nextState.geoids += state.geoidBots;
            nextState.time++;

            if (nextState.geoids > maxGeoidesMined)
                maxGeoidesMined = nextState.geoids;
            if (nextState.time == maxTime)
                continue;

            int timeLeft = maxTime - nextState.time;
            int maxGeoidesForThiState = MaxCollectableGeoidesForState(nextState, recepie, timeLeft);
            if (maxGeoidesForThiState <= maxGeoidesMined)
                continue; // cant be more efficient than best found so far
            
            // Build robots
            int botsThatCanBeBuild = 0;
            if (state.ores >= recepie.geodeBotOreCost && state.obsidians >= recepie.geodBotObsidianCost)
            {
                botsThatCanBeBuild++;

                var s2 = nextState;
                s2.ores -= recepie.geodeBotOreCost;
                s2.obsidians -= recepie.geodBotObsidianCost;
                s2.geoidBots++;
                var key = (s2.oreBots, s2.clayBots, s2.obsidianBots, s2.geoidBots, s2.ores, s2.clays, s2.obsidians, s2.geoids);
                if (visitedAtTime.TryGetValue(key, out var visitedTime) == false || nextState.time < visitedTime)
                {
                    visitedAtTime[key] = s2.time;
                    searchTree.Push(s2);
                }
            }

            if (state.ores >= recepie.obsidianBotOreCost && state.clays >= recepie.obsidianBotClayCost)
            {
                botsThatCanBeBuild++;

                if (nextState.obsidianBots < recepie.geodBotObsidianCost && nextState.obsidians < timeLeft * recepie.geodBotObsidianCost && timeLeft >= 3)
                {
                    var s2 = nextState;
                    s2.ores -= recepie.obsidianBotOreCost;
                    s2.clays -= recepie.obsidianBotClayCost;
                    s2.obsidianBots++;
                    var key = (s2.oreBots, s2.clayBots, s2.obsidianBots, s2.geoidBots, s2.ores, s2.clays, s2.obsidians, s2.geoids);
                    if (visitedAtTime.TryGetValue(key, out var visitedTime) == false || nextState.time < visitedTime)
                    {
                        visitedAtTime[key] = s2.time;
                        searchTree.Push(s2);
                    }
                }
            }

            if (state.ores >= recepie.clayBotOreCost)
            {
                botsThatCanBeBuild++;

                if (nextState.clays < timeLeft * recepie.obsidianBotClayCost && timeLeft >= 5 && nextState.clayBots < recepie.obsidianBotClayCost)
                {
                    var s2 = nextState;
                    s2.ores -= recepie.clayBotOreCost;
                    s2.clayBots++;
                    var key = (s2.oreBots, s2.clayBots, s2.obsidianBots, s2.geoidBots, s2.ores, s2.clays, s2.obsidians, s2.geoids);
                    if (visitedAtTime.TryGetValue(key, out var visitedTime) == false || nextState.time < visitedTime)
                    {
                        visitedAtTime[key] = s2.time;
                        searchTree.Push(s2);
                    }
                }
            }

            if (state.ores >= recepie.oreBotOreCost)
            {
                botsThatCanBeBuild++;

                if (timeLeft >= 5 && state.oreBots < maxOreCost && nextState.ores < timeLeft * minOreCost)
                {
                    var s2 = nextState;
                    s2.ores -= recepie.oreBotOreCost;
                    s2.oreBots++;
                    var key = (s2.oreBots, s2.clayBots, s2.obsidianBots, s2.geoidBots, s2.ores, s2.clays, s2.obsidians, s2.geoids);
                    if (visitedAtTime.TryGetValue(key, out var visitedTime) == false || nextState.time < visitedTime)
                    {
                        visitedAtTime[key] = s2.time;
                        searchTree.Push(s2);
                    }
                }
            }

            if (botsThatCanBeBuild < 4) // Dont do nothing if we can build all robot types
            {
                var key = (nextState.oreBots, nextState.clayBots, nextState.obsidianBots, nextState.geoidBots, nextState.ores, nextState.clays, nextState.obsidians, nextState.geoids);
                if (visitedAtTime.TryGetValue(key, out var visitedTime) == false || nextState.time < visitedTime)
                {
                    visitedAtTime[key] = nextState.time;
                    searchTree.Push(nextState);
                }
            }
        }

        Console.WriteLine($"BP {recepie.bpNum}: Found {maxGeoidesMined}, visited nodes " + visitedAtTime.Count);
        return maxGeoidesMined;
    }
    
    private static int MaxCollectableGeoidesForState((int oreBots, int clayBots, int obsidianBots, int geoidBots, int ores, int clays, int obsidians, int geoids, int time) state, (int bpNum, int oreBotOreCost, int clayBotOreCost, int obsidianBotOreCost, int obsidianBotClayCost, int geodeBotOreCost, int geodBotObsidianCost) recepie, int timeLeft)
    {
        while (timeLeft >= 0)
        {
            if (state.ores >= recepie.geodeBotOreCost && state.obsidians >= recepie.geodBotObsidianCost)
            {
                state.ores -= recepie.geodeBotOreCost;
                state.obsidians -= recepie.geodBotObsidianCost;
                state.geoidBots++;
            }
            else
            {
                // build both robots to lesser cost
                state.oreBots++;
                state.obsidianBots++;
                state.ores -= Math.Min(recepie.oreBotOreCost, recepie.obsidianBotOreCost);
            }

            state.ores += state.oreBots;
            state.obsidians += state.obsidianBots;
            state.geoids += state.geoidBots;

            timeLeft--;
        }

        return state.geoids;
    }
}