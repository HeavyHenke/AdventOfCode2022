using System.Text.RegularExpressions;

class Day19
{
    public object A()
    {
        var data = File.ReadLines("Day19.txt")
            .Select(l => Regex.Match(l, @"Blueprint (?<blueprint>\d+): Each ore robot costs (?<oreBotOreCost>\d+) ore. Each clay robot costs (?<clayBotOreCost>\d+) ore. Each obsidian robot costs (?<obsidianBotOreCost>\d+) ore and (?<obsidianBotClayCost>\d+) clay. Each geode robot costs (?<geodeBotOreCost>\d+) ore and (?<geodBotObsidianCost>\d+) obsidian."))
            .Select(m => (bpNum: int.Parse(m.Groups["blueprint"].Value), 
                oreBotOreCost: int.Parse(m.Groups["oreBotOreCost"].Value), 
                clayBotOreCost: int.Parse(m.Groups["clayBotOreCost"].Value),
                obsidianBotOreCost: int.Parse(m.Groups["obsidianBotOreCost"].Value),
                obsidianBotClayCost: int.Parse(m.Groups["obsidianBotClayCost"].Value),
                geodeBotOreCost: int.Parse(m.Groups["geodeBotOreCost"].Value),
                geodBotObsidianCost: int.Parse(m.Groups["geodBotObsidianCost"].Value)));
        
        int result = 0;
        foreach (var bluePrint in data)
        {
            int obsidian = CalcMostObsidianByBluePrint(bluePrint);
            Console.WriteLine($"BP {bluePrint.bpNum}: {obsidian}");
            result += obsidian * bluePrint.bpNum;
        }
        
        // 1522 too low
        return result;
    }

    private int CalcMostObsidianByBluePrint((int bpNum, int oreBotOreCost, int clayBotOreCost, int obsidianBotOreCost, int obsidianBotClayCost, int geodeBotOreCost, int geodBotObsidianCost) recepie)
    {
        var state = (oreBots: 1, clayBots: 0, obsidianBots: 0, geoidBots: 0, ores: 0, clays: 0, obsidians: 0, geoids: 0, time: 0);
        var searchTree = new Stack<(int oreBots, int clayBots, int obsidianBots, int geoidBots, int ores, int clays, int obsidians, int geoids, int time)>();
        searchTree.Push(state);
        
        int maxGeoidesMined = 0;
        long visitedNodes = 0;
        
        while (searchTree.Count > 0)
        {
            visitedNodes++;
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
            if (nextState.time == 24)
                continue;
            
            int timeLeft = 24 - nextState.time;
            int maxGeoidesForThiState = MaxCollectableGeoidesForState(nextState, recepie, timeLeft); //nextState.geoids + nextState.geoidBots * timeLeft + MaxBotsToAndCollectInTime(timeLeft);
            if(maxGeoidesForThiState <= maxGeoidesMined)
                continue;   // cant be more efficient than best found so far

            // Build robots
            int botsBuildsAdded = 0;
            if (state.ores >= recepie.geodeBotOreCost && state.obsidians >= recepie.geodBotObsidianCost)
            {
                var s2 = nextState;
                s2.ores -= recepie.geodeBotOreCost;
                s2.obsidians -= recepie.geodBotObsidianCost;
                s2.geoidBots++;
                searchTree.Push(s2);
                botsBuildsAdded++;
            }
            if(state.ores >= recepie.obsidianBotOreCost && state.clays >= recepie.obsidianBotClayCost)
            {
                var s2 = nextState;
                s2.ores -= recepie.obsidianBotOreCost;
                s2.clays -= recepie.obsidianBotClayCost;
                s2.obsidianBots++;
                searchTree.Push(s2);
                botsBuildsAdded++;
            }
            if (state.ores >= recepie.clayBotOreCost && nextState.clays < timeLeft * recepie.obsidianBotClayCost && timeLeft >= 3)
            {
                var s2 = nextState;
                s2.ores -= recepie.clayBotOreCost;
                s2.clayBots++;
                searchTree.Push(s2);
                botsBuildsAdded++;
            }
            if (state.ores >= recepie.oreBotOreCost && timeLeft >= 3)
            {
                var s2 = nextState;
                s2.ores -= recepie.oreBotOreCost;
                s2.oreBots++;
                searchTree.Push(s2);
                botsBuildsAdded++;
            }

            if(botsBuildsAdded < 4)
                searchTree.Push(nextState); // Dont buy any bots and save for a more expensive one
        }

        Console.WriteLine("Visited nodes " +visitedNodes);
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
            state.clays += state.clayBots;
            state.obsidians += state.obsidianBots;
            state.geoids += state.geoidBots;

            timeLeft--;
        }

        return state.geoids;
    }
}