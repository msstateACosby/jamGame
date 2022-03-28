using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    // Start is called before the first frame update
    Vector2Int position;
    int timeleft;
    void Awake()
    {
        position = new Vector2Int(((int)transform.position.x), (int)transform.position.y);
        timeleft = 5;
        GetComponent<SpriteRenderer>().color = Color.Lerp(Color.magenta, Color.green, Random.Range(0.0f, 1.0f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void initialize(int timeleft)
    {
        this.timeleft = timeleft;
        //Debug.Log(this.timeleft);
    }
    public Vector2Int move(Vector2Int[,] paths, List<Vector2Int> otherAgentPositions, int mapRadius, out bool expiring)
    {
        Vector2Int nextMove =paths[position.x+ mapRadius, position.y+mapRadius];
        
        Vector2Int nextposition = new Vector2Int(nextMove.x - mapRadius, nextMove.y -mapRadius);
        //if (!otherAgentPositions.Contains(nextposition))
            transform.position = new Vector3(nextMove.x - mapRadius, nextMove.y -mapRadius);
            position = nextposition;
        timeleft -= 1;
        if (timeleft == 0) expiring= true;
        
        else expiring = false;
        
        if (expiring) Debug.Log("Experinging");
        return position;
    }
    
}
