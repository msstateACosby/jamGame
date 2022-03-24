using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    List<MapObj> placedObjs;
    public int startingMapRadius = 3;
    int mapRadius;

    public GameObject ground;
    public MapObj chasm;

    void Start()
    {
        mapRadius = startingMapRadius;
        placedObjs = new List<MapObj>();
    
        createInitialMap();
    }
        

    // Update is called once per frame
    void Update()
    {
        
    }
    void createInitialMap()
    {
        for (int x = -startingMapRadius; x<=  startingMapRadius; x++)
        {
            for (int y  = -startingMapRadius; y<=  startingMapRadius; y++)
            {
            Instantiate(ground, new Vector3(x, y), Quaternion.identity, this.transform);
            }

        }
        Instantiate(chasm, new Vector3(0,0), Quaternion.identity, this.transform);
        placedObjs.Add(chasm);
    }
    void expandMap()
    {
        mapRadius += 1;
        for (int x = -mapRadius; x<=  mapRadius; x++)
        {
            Instantiate(ground, new Vector3(x, -mapRadius), Quaternion.identity, this.transform);
            Instantiate(ground, new Vector3(x, mapRadius), Quaternion.identity, this.transform);

        }
        for (int y  = -mapRadius+ 1; y<=  mapRadius - 1; y++)
        {
            Instantiate(ground, new Vector3(-mapRadius, y), Quaternion.identity, this.transform);
            Instantiate(ground, new Vector3(mapRadius, y), Quaternion.identity, this.transform);
        }
    }
    public void endTurn()
    {
       
        expandMap();
    }
}
