using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    // Start is called before the first frame update
    Vector2Int position;
    void Start()
    {
        position = new Vector2Int(((int)transform.position.x), (int)transform.position.y);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public Vector2Int move(int[,] mapWeights, int mapRadius)
    {
        return position;
    }
    public void djikstras(int[,] map, int adjustedX, int adjustedY)
    {
        Dictionary<Vector2Int, Vector2Int> completedMap = new Dictionary<Vector2Int, Vector2Int>();
        //Fibonn
        //PriorityQueue<Vector2Int, int> frontier = new PriorityQueue<Vector2Int, int>();
        int[,] distances = new int[map.GetLength(0), map.GetLength(1)];
        bool foundCenter = false;
        while (foundCenter != true)
        {
            //foreach (Vector2Int node in frontier)
            //{

            //}
        }


    }
    List<Vector2Int> getNeighbors(int x, int y, int mapSize)
    {
        List<Vector2Int> neighbors = new List<Vector2Int>();
        if (x+1 < mapSize) neighbors.Add(new Vector2Int(x+1, y));
        if (x-1 >= 0) neighbors.Add(new Vector2Int(x-1, y));
        if (y+1 < mapSize) neighbors.Add(new Vector2Int(x, y+1));
        if (y-1 >= 0) neighbors.Add(new Vector2Int(x, y-1));
        return neighbors;
    }
}
