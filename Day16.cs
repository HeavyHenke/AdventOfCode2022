using System.Text.RegularExpressions;
using MoreLinq;

class Day16
{
    public object A()
    {
        var valves = File.ReadLines("Day16.txt")
            .Select(l => Regex.Match(l, @"Valve (?<valve>\w+) has flow rate=(?<rate>\d+); tunnels? leads? to valves? (?<next>\w+)(, (?<next>\w+))*"))
            .Select(m => (valve: m.Groups["valve"].Value, flow: int.Parse(m.Groups["rate"].ValueSpan), next: m.Groups["next"].Captures.Select(c => c.Value).ToArray()))
            .ToDictionary(key => key.valve);

        var orderedValveFlow = valves.Values.Select(v => (v.valve, v.flow)).OrderByDescending(v => v.flow).ToList();

        var search = new Stack<(HashSet<string> openValves, string pos, int flow, int time, string prevPos)>();
        search.Push(new(new HashSet<string>(), "AA", 0, 0, ""));
        int bestSolution = 0;
        int numVisited = 0;

        while (search.Any())
        {
            numVisited++;
            var node = search.Pop();
            if (node.flow > bestSolution)
            {
                bestSolution = node.flow;
                Console.WriteLine("Max flow " + bestSolution);
            }
            
            if (node.time == 30)
                continue;

            var currValvePos = valves[node.pos];

            // Open valve if not open
            if (node.openValves.Contains(node.pos) == false && currValvePos.flow > 0)
            {
                var flowToAdd = currValvePos.flow * (30 - node.time - 1);
                var openValves = Enumerable.ToHashSet(node.openValves);
                openValves.Add(node.pos);

                search.Push((openValves, node.pos, node.flow + flowToAdd, node.time + 1, ""));
            }

            // Is it possible to beat max score from here?
            var timeLeft = 30 - node.time - 1;
            var maxNumValvesToOpen = timeLeft / 2;
            var maxFlowToAdd = orderedValveFlow
                .Where(v => !node.openValves.Contains(v.valve))
                .Take(maxNumValvesToOpen)
                .Index()
                .Sum(v => v.Value.flow * (30 - node.time - 1 - v.Key * 2));
            if(node.flow + maxFlowToAdd <= bestSolution)
                continue;

            // Move
            foreach (var next in currValvePos.next)
            {
                if (next != node.prevPos)
                    search.Push((node.openValves, next, node.flow, node.time + 1, node.pos));
            }
        }

        Console.WriteLine($"Visited {numVisited} nodes");

        return bestSolution;
    }

    public object B()
    {
        var valves = File.ReadLines("Day16.txt")
            .Select(l => Regex.Match(l, @"Valve (?<valve>\w+) has flow rate=(?<rate>\d+); tunnels? leads? to valves? (?<next>\w+)(, (?<next>\w+))*"))
            .Select(m => (valve: m.Groups["valve"].Value, flow: int.Parse(m.Groups["rate"].ValueSpan), next: m.Groups["next"].Captures.Select(c => c.Value).ToArray()))
            .ToDictionary(key => key.valve);

        var orderedValveFlow = valves.Values.Select(v => (v.valve, v.flow)).OrderByDescending(v => v.flow).ToList();

        var search = new Stack<(HashSet<string> openValves, string pos, int flow, int time, string prevPos, string pos2, string prev2)>();
        search.Push(new(new HashSet<string>(), "AA", 0, 4, "", "AA", ""));
        int bestSolution = 0;
        int numVisited = 0;

        while (search.Any())
        {
            numVisited++;
            var node = search.Pop();
            if (node.flow > bestSolution)
            {
                bestSolution = node.flow;
                Console.WriteLine("Max flow " + bestSolution);
            }
            
            if (node.time == 30)
                continue;

            // Is it possible to beat max score from here?
            var timeLeft = 30 - node.time - 2;
            var maxNumValvesToOpen = timeLeft;
            var maxFLowThisTurn = (node.openValves.Contains(node.pos) ? 0 : valves[node.pos].flow * (30 - node.time)) + (node.openValves.Contains(node.pos2) ? 0 : valves[node.pos2].flow * (30 - node.time));
            var maxFlowToAdd = maxFLowThisTurn + orderedValveFlow
                .Where(v => !node.openValves.Contains(v.valve))
                .Take(maxNumValvesToOpen)
                .Index()
                .Sum(v => v.Value.flow * (30 - node.time - 2 - v.Key * 2));
            if(node.flow + maxFlowToAdd <= bestSolution)
                continue;

            var currValvePos = valves[node.pos];
            var nextMovesForP1 = new List<(HashSet<string> openValves, string pos, int flow, string prevPos)>();
            
            // Open valve if not open
            if (node.openValves.Contains(node.pos) == false && currValvePos.flow > 0)
            {
                var flowToAdd = currValvePos.flow * (30 - node.time - 1);
                var openValves = Enumerable.ToHashSet(node.openValves);
                openValves.Add(node.pos);

                nextMovesForP1.Add((openValves, node.pos, node.flow + flowToAdd, ""));
            }
            
            // Move
            foreach (var next in currValvePos.next)
            {
                if (next != node.prevPos)
                    nextMovesForP1.Add((node.openValves, next, node.flow, node.pos));
            }
            
            // Next move for the elephant
            currValvePos = valves[node.pos2];
            foreach (var m in nextMovesForP1)
            {
                // Open valve if not open
                if (m.openValves.Contains(node.pos2) == false && currValvePos.flow > 0)
                {
                    var flowToAdd = currValvePos.flow * (30 - node.time - 1);
                    var openValves = Enumerable.ToHashSet(m.openValves);
                    openValves.Add(node.pos2);

                    search.Push((openValves, m.pos, m.flow + flowToAdd, node.time + 1, m.prevPos, node.pos2, ""));
                }
                
                // Move
                foreach (var next in currValvePos.next)
                {
                    if (next != node.prev2 && !(m.prevPos == node.pos2 && m.prevPos != m.pos && string.Compare(m.pos, next, StringComparison.Ordinal) > 0))
                        search.Push((m.openValves, m.pos, m.flow, node.time + 1, m.prevPos, next, node.pos2));
                }
            }
        }

        Console.WriteLine($"Visited {numVisited} nodes");
        return bestSolution;
    }
}