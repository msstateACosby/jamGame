using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System;
public class PriorityQueue
{
    public List<Vector2Int> locs;
    public List<int> priorities;

    public PriorityQueue()
    {
        locs = new List<Vector2Int>();
        priorities = new List<int>();
    }
    public Vector2Int getNext(out int priority)
    {
        
        int lowestIndex = 0;
        int lowest = priorities[0];
        for (int i = 0; i < priorities.Count; i++)
        {
            if (priorities[i] < lowest) 
            {
                lowestIndex = i;
                lowest = priorities[i];
            }
        }
        Vector2Int loc = locs[lowestIndex];
        locs.RemoveAt(lowestIndex);
        priorities.RemoveAt(lowestIndex);
        priority = lowest;
        return loc;
    }
    public bool isQueueEmpty()
    {
        return locs.Count == 0;
    }
    public void insert(Vector2Int loc, int priority)
    {
        locs.Add(loc);
        priorities.Add(priority);
    }
}
