
using System;

public class MiniAStar
{
    public Node[][] Grid { get; private set; }
    public bool PreferStraightPath = false;
    public bool AllowCorners = true;

    public MiniAStar(Node[][] grid)
    {
        Grid = grid;
    }

    public MiniAStar(int X, int Y)
    {
        Grid = new Node[X][];
        for (int i = 0; i < X; i++)
        {
            Grid[i] = new Node[Y];
            for (int n = 0; n < Y; n++)
            {
                Grid[i][n] = new Node((i, n));
            }
        }
    }

    public void SetNode((int, int) pos, Node node)
    {
        Grid[pos.Item1][pos.Item2] = node;
    }

    public Node GetNode((int, int) pos)
    {
        return Grid[pos.Item1][pos.Item2];
    }

    public Node GetEndNodeWithPath((int, int) start, (int, int) end)
    {
        Node current = GetNode(start);
        current.UpdateData(null, 0, dist(start, end));
        UpdateAround(current, end, (0, 0));
        return null; //TODO
    }

    public void ResetCosts()
    {
        for (int i = 0; i < Grid.Length; i++)
        {
            for (int n = 0; n < Grid[i].Length; n++)
            {
                Grid[i][n].ResetCosts();
            }
        }
    }

    void UpdateAround(Node from, (int, int) endpos, (int, int) dir)
    {
        for (int x = from.Pos.Item1 - 1; x < from.Pos.Item1 + 2; x++)
        {
            for (int y = from.Pos.Item2 - 1; y < from.Pos.Item2 + 2; y++)
            {
                if (x == from.Pos.Item1 && y == from.Pos.Item2) continue;
                if (x >= 0 && x < Grid.Length && y >= 0 && y < Grid[x].Length)
                {
                    var node = GetNode((x, y));
                    if (x != from.Pos.Item1 && y != from.Pos.Item2)
                    {
                        if (!AllowCorners) continue;
                        CheckAndUpdateNode(from, node, 14, endpos);
                    }
                    else
                    {
                        if (PreferStraightPath)
                        {
                            int xDiff = x - from.Pos.Item1;
                            int yDiff = y - from.Pos.Item2;
                            if (xDiff == dir.Item1 && yDiff == dir.Item2)
                            {
                                CheckAndUpdateNode(from, node, 8, endpos);
                            }
                            else
                            {
                                CheckAndUpdateNode(from, node, 10, endpos);
                            }
                        }
                        else
                        {
                            CheckAndUpdateNode(from, node, 10, endpos);
                        }
                    }
                }
            }
        }
    }

    void CheckAndUpdateNode(Node prev, Node next, float gcost, (int, int) endpos)
    {
        if (prev.GCost + gcost < next.GCost)
        {
            next.UpdateData(prev, prev.GCost + 10, dist(prev.Pos, endpos));
        }
    }

    float dist((int, int) start, (int, int) end)
    {
        var sqr = Math.Pow(end.Item1 - start.Item1, 2) + Math.Pow(end.Item2 - start.Item2, 2);
        return (float)Math.Sqrt(sqr);
    }
}

public class Node
{
    public Node PrevNode { get; private set; } = null;
    public float GCost { get; private set; } = float.PositiveInfinity;
    public float HCost { get; private set; } = float.PositiveInfinity;
    public float AddCost { get; private set; } = 0;
    public (int, int) Pos { get; private set; }

    public float SumCost => GCost + HCost + AddCost;

    public Node((int, int) pos)
    {
        Pos = pos;
    }

    public void UpdateData(Node prev, float gcost, float hcost, float addcost = float.NegativeInfinity, bool corner = false)
    {
        if (prev == null)
            GCost = gcost;
        else
        {
            PrevNode = prev;
            if (corner)
                GCost = prev.GCost + 14;
            else
                GCost = prev.GCost + 10;
        }
        HCost = hcost;
        if (!float.IsInfinity(addcost))
            AddCost = addcost;
    }

    public void UpdatePrevNode(Node prev, bool corner = false)
    {
        PrevNode = prev;
        if (corner)
            GCost = prev.GCost + 14;
        else
            GCost = prev.GCost + 10;
    }

    public void ResetCosts()
    {
        PrevNode = null;
        GCost = float.PositiveInfinity;
        HCost = float.PositiveInfinity;
    }

    public void UpdateAddCost(float addcost)
    {
        AddCost = addcost;
    }
}
