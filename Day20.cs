class Day20
{
    public object A()
    {
        var list = new LinkedList<int>();
        var nodeInOrder = new List<LinkedListNode<int>>();
        LinkedListNode<int> node0 = null;
        
        foreach (var line in File.ReadAllLines("Day20.txt"))
        {
            var value = int.Parse(line);
            var node = list.AddLast(value);
            nodeInOrder.Add(node);
            if (value == 0)
                node0 = node;
        }
        
        var wrapAroundLength = nodeInOrder.Count - 1;

        foreach (var node in nodeInOrder)
        {
            var moveDist = node.Value;
            moveDist %= wrapAroundLength;
            
            var dest = node;
            if (moveDist > 0)
            {
                while (moveDist > 0)
                {
                    dest = dest.Next ?? list.First;
                    moveDist--;
                }

                if (node != dest)
                {
                    list.Remove(node);
                    list.AddAfter(dest, node);
                }
            }
            else
            {
                while (moveDist < 0)
                {
                    dest = dest.Previous ?? list.Last;
                    moveDist++;
                }

                if (node != dest)
                {
                    list.Remove(node);
                    list.AddBefore(dest, node);
                }
            }
        }
        
        return GetNumAt(node0, 1000) + GetNumAt(node0, 2000) + GetNumAt(node0, 3000);
    }

    public object B()
    {
        var list = new LinkedList<long>();
        var nodeInOrder = new List<LinkedListNode<long>>();
        LinkedListNode<long> node0 = null;
        
        foreach (var line in File.ReadAllLines("Day20.txt"))
        {
            var value = int.Parse(line) * 811589153L;
            var node = list.AddLast(value);
            nodeInOrder.Add(node);
            if (value == 0)
                node0 = node;
        }
        
        var wrapAroundLength = nodeInOrder.Count - 1;
        var halfWrapLength = wrapAroundLength / 2;

        for (int i = 0; i < 10; i++)
        {
            foreach (var node in nodeInOrder)
            {
                var moveDist = node.Value;
                moveDist %= wrapAroundLength;
                if (moveDist > halfWrapLength)
                    moveDist -= wrapAroundLength;
                if (moveDist < -halfWrapLength)
                    moveDist += wrapAroundLength;

                var dest = node;
                if (moveDist > 0)
                {
                    while (moveDist > 0)
                    {
                        dest = dest.Next ?? list.First;
                        moveDist--;
                    }

                    if (node != dest)
                    {
                        list.Remove(node);
                        list.AddAfter(dest, node);
                    }
                }
                else
                {
                    while (moveDist < 0)
                    {
                        dest = dest.Previous ?? list.Last;
                        moveDist++;
                    }

                    if (node != dest)
                    {
                        list.Remove(node);
                        list.AddBefore(dest, node);
                    }
                }
            }
        }

        return GetNumAt(node0, 1000) + GetNumAt(node0, 2000) + GetNumAt(node0, 3000);
    }

    private T GetNumAt<T>(LinkedListNode<T> node, int pos)
    {
        while (pos > 0)
        {
            node = node!.Next ?? node.List!.First!;
            pos--;
        }

        return node.Value;
    }
}