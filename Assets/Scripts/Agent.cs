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
    public Vector2Int move(int[][] mapWeights)
    {
        return position;
    }
}
