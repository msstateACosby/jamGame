using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;
public class Manager : MonoBehaviour
{
    Dictionary<Vector2Int,GameObject> placedObjs;
    HashSet<Vector2Int> bounds;
    List<Agent> agents;
    List<Vector2Int> agentPositions;
    
    int[,] mapWeights;
    public int startingMapRadius = 3;
    int mapRadius;

    public List<MapObj> mapObjs;

    public GameObject ground;
    public MapObj chasm;

    public RectTransform signPanel;
    public RectTransform lifePanel;
    
    int? chosenSignNum;
    public int lives = 3;
    

    public SpriteRenderer highlightObj;

    

    Dictionary<Vector2Int, Tile> tiles;

    public Agent prefabAgent;

    int turn;

    Vector2Int[,] paths;
    bool placedObjThisTurn = false;
    bool gameEnding =false;
    float endingTimer;

    void Start()
    {
        mapRadius = startingMapRadius;
        placedObjs = new Dictionary<Vector2Int, GameObject>();
        tiles = new Dictionary<Vector2Int, Tile>();
        bounds = new HashSet<Vector2Int>();
        agents = new List<Agent>();   
        agentPositions = new List<Vector2Int>();
        createInitialMap();
        turn = 1;
        updateAvailableSigns();
        
        setUpPathing();
    }
    public void updateLives()
    {
        for (int i = 0; i < 3; i++)
        {
            if (i < lives)
            {
                lifePanel.GetChild(i).GetComponent<Image>().color = Color.white;
            }
            else lifePanel.GetChild(i).GetComponent<Image>().color = Color.gray;
        }
        if (lives <= 0)
        {
            gameEnding = true;
            endingTimer = 1.0f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (gameEnding)
        {
            endingTimer -= Time.deltaTime;
            if (endingTimer <= 0)
            {
                ScenePassInfo.turnReached = turn;
                SceneManager.LoadScene("EndGame", LoadSceneMode.Single);

            }
        }
    }
    void createInitialMap()
    {
        for (int x = -startingMapRadius; x<=  startingMapRadius; x++)
        {
            for (int y  = -startingMapRadius; y<=  startingMapRadius; y++)
            {
                createTile(new Vector2Int(x,y));
            }

        }
        GameObject chasmObj = Instantiate(chasm.gameObject, new Vector3(0,0), Quaternion.identity, this.transform);
        placedObjs.Add(Vector2Int.zero, chasmObj);
        foreach (Vector2Int neighbor in getNeighborsWorldSpace(0, 0))
            {
                bounds.Add(neighbor);
            }
    }
    void expandMap()
    {
        mapRadius += 1;
        
        for (int x = -mapRadius; x<=  mapRadius; x++)
        {
            
            createTile(new Vector2Int(x,-mapRadius));
            createTile(new Vector2Int(x, mapRadius));

        }
        for (int y  = -mapRadius+ 1; y<=  mapRadius - 1; y++)
        {
            createTile(new Vector2Int(-mapRadius, y));
            createTile(new Vector2Int( mapRadius, y));
        }
    }
    public void createTile(Vector2Int pos)
    {
        Tile newTile = Instantiate(ground, ((Vector3Int)pos), Quaternion.identity, this.transform).GetComponent<Tile>();
       
        newTile.initialize(this, pos, mapRadius);
        tiles.Add(pos, newTile);
    }
    public void endTurn()
    {
        if (placedObjThisTurn == false || gameEnding) return;
        
        placedObjThisTurn = false;
        processAgents();
        expandMap();
        setUpPathing();
        for (int x = 0; x <= (int)(Mathf.Sqrt((float)mapRadius)/2); x++)
        {
            createNewAgent();
        }
        
        updateAvailableSigns();
        
        turn += 1;
        lifePanel.GetChild(3).GetChild(1).GetComponent<Text>().text = turn.ToString();
    }
    public void createNewAgent()
    {
        int linearPosition = Random.Range(0, mapRadius * 4 + mapRadius * 4+1);
        
        Vector3 pos;
        if (linearPosition < mapRadius * 2 +1)
        {
            pos = new Vector3(linearPosition -mapRadius, -mapRadius);

        }
        else if (linearPosition < mapRadius * 4 + 1)
        {
            pos = new Vector3((linearPosition % (mapRadius * 2)) -mapRadius, mapRadius);
        }
        if (linearPosition < mapRadius * 6 +1)
        {
            pos = new Vector3( -mapRadius, (linearPosition % (mapRadius * 2))-mapRadius);

        }
        else
        {
            pos = new Vector3(mapRadius, (linearPosition % (mapRadius * 2))-mapRadius);
        }
        Agent newObj = Instantiate(prefabAgent, pos, Quaternion.identity, this.transform);
        agents.Add(newObj);
        agentPositions.Add(new Vector2Int((int)pos.x, (int)pos.y));
        newObj.initialize(mapRadius *2);
    }
    
    public void updateAvailableSigns()
    {
        chosenSignNum = null;
        //randomize the mapObjs
        for (int i = 0; i < mapObjs.Count; i++) {
            MapObj temp = mapObjs[i];
            int randomIndex = Random.Range(i, mapObjs.Count);
            mapObjs[i] = mapObjs[randomIndex];
            mapObjs[randomIndex] = temp;
        }
        
        int j = 0;
        foreach(Transform child in signPanel.GetComponentInChildren<Transform>())
        {
            
            child.GetChild(0).GetComponent<Image>().sprite = mapObjs[j].GetComponent<SpriteRenderer>().sprite;
            child.GetComponent<Button>().interactable = true;
            j ++;
        }

    }
    public void setUpPathing()
    {
        mapWeights = new int[mapRadius* 2+1,mapRadius*2+1];
        for (int i = 0; i < mapWeights.GetLength(0); i ++)
        {
            for (int j = 0; j < mapWeights.GetLength(1); j++)
            {
                mapWeights[i,j] = mapRadius;
                
            }
        }
        
        foreach (GameObject obj in placedObjs.Values)
        {
            
            
            MapObj mapObj = obj.GetComponent<MapObj>();
            
            mapObj.addToMapWeights(mapWeights, mapRadius);

        }
        
        for (int i = 0; i < mapWeights.GetLength(0); i ++)
        {
            for (int j = 0; j < mapWeights.GetLength(1); j++)
            {
                
                tiles[new Vector2Int(i-mapRadius, j-mapRadius)].updateWeight(mapWeights[i,j], mapRadius);
            }
        }
        paths = djikstras(mapWeights);
        foreach(Vector2Int loc in tiles.Keys)
        {
            Tile tile = tiles[loc];
            
            tile.setPathImage(paths[loc.x+mapRadius, loc.y+mapRadius]-new Vector2Int(loc.x+mapRadius, loc.y+mapRadius));
        }
    }
    public void processAgents()
    {
        HashSet<int> expiringIndices = new HashSet<int>();
        int iterator = 0;
        foreach(Agent agent in agents)
        {
            bool expired;
            Vector2Int movedTo = agent.move(paths, agentPositions, mapRadius, out expired);
            if (expired) expiringIndices.Add(iterator);

            agentPositions[iterator] = movedTo;
            
           
            if (movedTo == Vector2Int.zero)
            {
                expiringIndices.Add(iterator);
                lives-= 1;
            }
            else if (placedObjs.ContainsKey(movedTo))
            {
                if (placedObjs[movedTo].GetComponent<MapObj>().isCollecting())
                {
                    
                    expiringIndices.Add(iterator);
                }
            }
            iterator ++;
        }
        int destroyAccounter = 0;
        foreach(int index in expiringIndices)
        {
            Destroy(agents[index-destroyAccounter].gameObject);
            agents.RemoveAt(index-destroyAccounter);

            destroyAccounter ++;
        }
        
        updateLives();
    }
    public void clickSignButton(int x)
    {
        if (chosenSignNum.HasValue)
        {
            foreach(Button button in signPanel.GetComponentsInChildren<Button>())
            {
                button.interactable = true;
            }
        }
        signPanel.GetChild(x).GetComponent<Button>().interactable = false;
        
        chosenSignNum = x;

        highlightObj.sprite = getChosenSign().GetComponent<SpriteRenderer>().sprite;
    }
    public MapObj getChosenSign()
    {
        if (chosenSignNum != null)
        {   
            return mapObjs[chosenSignNum.GetValueOrDefault()];
        }
        else return null;
    }
    public void highlightTileWithSign(Vector2Int loc)
    {
        if (chosenSignNum != null)
        {
            highlightObj.enabled = true;
            SpriteRenderer childSprite = highlightObj.transform.GetChild(0).GetComponent<SpriteRenderer>();
            
            childSprite.enabled = true;
            if (placedObjs.ContainsKey(loc) || bounds.Contains(loc)) childSprite.color = Color.red;
            else childSprite.color = Color.green;
            highlightObj.transform.position = new Vector3(loc.x, loc.y);
            
        }
    }
    public void hideHighlight()
    {
        highlightObj.enabled = false;
        SpriteRenderer childSprite = highlightObj.transform.GetChild(0).GetComponent<SpriteRenderer>();
        childSprite.enabled = false;
    }
    public void clickTile(Vector2Int loc)
    {
        if (chosenSignNum != null && !placedObjs.ContainsKey(loc) && !bounds.Contains(loc))
        {
            GameObject newObj = Instantiate(mapObjs[(int)chosenSignNum].gameObject, new Vector3(loc.x, loc.y), Quaternion.identity, transform);
            placedObjs.Add(loc, newObj);
            foreach (Vector2Int neighbor in getNeighborsWorldSpace(loc.x, loc.y))
            {
                bounds.Add(neighbor);
            }
            //updateAvailableSigns();
            setUpPathing();
            disableSigns();
            placedObjThisTurn = true;
        }
    }
    public void disableSigns()
    {
        foreach(Button button in signPanel.GetComponentsInChildren<Button>())
            {
                button.interactable = false;
            }
    }
    public Vector2Int[,] djikstras(int[,] map)
    {
        //Dictionary<Vector2Int, Vector2Int> completedMap = new Dictionary<Vector2Int, Vector2Int>();
        Vector2Int[,] completeMap = new Vector2Int[map.GetLength(0),map.GetLength(0)];
        int[,] completeMapWeights = new int[map.GetLength(0),map.GetLength(0)];
        for (int i = 0; i < mapWeights.GetLength(0); i ++)
        {
            for (int j = 0; j < mapWeights.GetLength(1); j++)
            {
                completeMapWeights[i,j] = -1;
            }
        }
        
        PriorityQueue queue = new PriorityQueue();
        queue.insert(new Vector2Int(mapRadius, mapRadius), map[mapRadius, mapRadius]);
        completeMap[mapRadius,mapRadius] = new Vector2Int(mapRadius, mapRadius);
        completeMapWeights[mapRadius, mapRadius] = map[mapRadius, mapRadius];
        while (queue.isQueueEmpty() != true)
        {
            int currentWeight;
            Vector2Int currentLoc = queue.getNext(out currentWeight);
            List<Vector2Int> neighbors = getNeighbors(currentLoc.x, currentLoc.y, map.GetLength(0));
            foreach(Vector2Int neighbor in neighbors)
            {
                int newWeight =currentWeight + map[neighbor.x, neighbor.y];
                if (completeMapWeights[neighbor.x,neighbor.y] == -1 || newWeight < completeMapWeights[neighbor.x,neighbor.y])
                {
                    completeMap[neighbor.x, neighbor.y] = currentLoc;
                    completeMapWeights[neighbor.x, neighbor.y] = newWeight;
                    queue.insert(neighbor, newWeight);
                }
            }
        }
        
        return completeMap;


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
    List<Vector2Int> getNeighborsWorldSpace(int x, int y)
    {
        List<Vector2Int> neighbors = new List<Vector2Int>();
        neighbors.Add(new Vector2Int(x+1, y));
        neighbors.Add(new Vector2Int(x-1, y));
        neighbors.Add(new Vector2Int(x, y+1));
        neighbors.Add(new Vector2Int(x, y-1));
        return neighbors;
    }
}
