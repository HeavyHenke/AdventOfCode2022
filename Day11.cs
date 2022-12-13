using MoreLinq;

class Day11
{
    class Monkey
    {
        private readonly Queue<int> _items;
        private readonly Func<long, long> _op;
        private readonly int _throwTrue;
        private readonly int _throwFalse;
        private readonly bool _doDivByThree;
        private int _commonDiv;

        public int DivTest { get; }
        public int NumInspected { get; private set; }
        
        public Monkey(IEnumerable<string> rows, bool doDivByThree)
        {
            _doDivByThree = doDivByThree;
            var r = rows.ToList();
            
            _items = new Queue<int>(r[1].Split(":")[1].Split(',').Select(int.Parse));

            var opTest = r[2].Split(" = ").Last();
            if (opTest == "old * old")
                _op = old => old * old;
            else if (opTest.Contains('*'))
            {
                int operand = int.Parse(opTest.Split('*').Last());
                _op = old => old * operand;
            }
            else if (opTest.Contains('+'))
            {
                int operand = int.Parse(opTest.Split('+').Last());
                _op = old => old + operand;
            }
            else
                throw new Exception("Unknown operations");

            DivTest = int.Parse(r[3].Split(' ').Last());

            _throwTrue = int.Parse(r[4].Split(' ').Last());
            _throwFalse = int.Parse(r[5].Split(' ').Last());
        }

        public void ThrowAll(List<Monkey> monkeys)
        {
            while(_items.Count > 0)
            {
                long item = _items.Dequeue();
                NumInspected++;

                item = _op(item);
                if (_doDivByThree)
                    item /= 3;
                while (item > _commonDiv)
                    item -= _commonDiv;

                var inspected = (int)item;


                if (inspected % DivTest == 0)
                    monkeys[_throwTrue].AddItem(inspected);
                else
                    monkeys[_throwFalse].AddItem(inspected);
            }
        }

        private void AddItem(int item)
        {
            _items.Enqueue(item);
        }

        public void SetCommonDiv(int commonDiv)
        {
            _commonDiv = commonDiv;
        }
    }
    
    public object A()
    {
        var monkeys = File.ReadLines("Day11.txt")
            .Batch(7)
            .Select(b => new Monkey(b, true))
            .ToList();

        var commonDiv = monkeys.Select(m => m.DivTest).Aggregate(1, (a, b) => a * b);
        foreach (var m in monkeys)
            m.SetCommonDiv(commonDiv);
        

        for (int round = 0; round < 20; round++)
        {
            foreach (var m in monkeys)
                m.ThrowAll(monkeys);
        }

        var twoMostActive = monkeys.OrderByDescending(m => m.NumInspected).Take(2).ToArray();
        return twoMostActive[0].NumInspected * twoMostActive[1].NumInspected; 
    }

    public object B()
    {
        var monkeys = File.ReadLines("Day11.txt")
            .Batch(7)
            .Select(b => new Monkey(b, false))
            .ToList();

        var commonDiv = monkeys.Select(m => m.DivTest).Aggregate(1, (a, b) => a * b);
        foreach (var m in monkeys)
            m.SetCommonDiv(commonDiv);

        for (int round = 0; round < 10_000; round++)
        {
            foreach(var m in monkeys)
                m.ThrowAll(monkeys);

            if ((round & 0xFF) == 0)
                Console.WriteLine(round);
        }
        
        foreach(var m in monkeys)
            Console.WriteLine($"Monkey inspected {m.NumInspected}");

        var twoMostActive = monkeys.OrderByDescending(m => m.NumInspected).Take(2).ToArray();
        return (long)twoMostActive[0].NumInspected * twoMostActive[1].NumInspected;
    }
}