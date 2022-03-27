using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Manager : MonoBehaviour
{
    Dictionary<Vector2Int,GameObject> placedObjs;
    List<Agent> agents;
    int[,] mapWeights;
    public int startingMapRadius = 3;
    int mapRadius;

    public List<MapObj> mapObjs;

    public GameObject ground;
    public MapObj chasm;

    public RectTransform signPanel;
    
    int? chosenSignNum;
    

    public SpriteRenderer highlightObj;

    

    Dictionary<Vector2Int, Tile> tiles;

    int turn;

    void Start()
    {
        mapRadius = startingMapRadius;
        placedObjs = new Dictionary<Vector2Int, GameObject>();
        tiles = new Dictionary<Vector2Int, Tile>();
        agents = new List<Agent>();   
        createInitialMap();
        turn = 1;
        updateAvailableSigns();
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
                createTile(new Vector2Int(x,y));
            }

        }
        GameObject chasmObj = Instantiate(chasm.gameObject, new Vector3(0,0), Quaternion.identity, this.transform);
        placedObjs.Add(Vector2Int.zero, chasmObj);
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
       
        
        createMapWeights();
        processAgents();
        expandMap();
        createMapWeights();
        turn += 1;
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
    public void createMapWeights()
    {
        mapWeights = new int[mapRadius* 2+1,mapRadius*2+1];
        for (int i = 0; i < mapWeights.GetLength(0); i ++)
        {
            for (int j = 0; j < mapWeights.GetLength(1); j++)
            {
                mapWeights[i,j] = mapRadius;
                
            }
        }
        Debug.Log("this");
        foreach (GameObject obj in placedObjs.Values)
        {
            
            
            MapObj mapObj = obj.GetComponent<MapObj>();
            
            mapObj.addToMapWeights(mapWeights, mapRadius);

        }
        Debug.Log("that");
        for (int i = 0; i < mapWeights.GetLength(0); i ++)
        {
            for (int j = 0; j < mapWeights.GetLength(1); j++)
            {
                
                tiles[new Vector2Int(i-mapRadius, j-mapRadius)].updateWeight(mapWeights[i,j], mapRadius);
            }
        }
    }
    public void processAgents()
    {
        foreach(Agent agent in agents)
        {
            if ((agent.move(mapWeights, mapRadius)).sqrMagnitude <= 2 ) Debug.Log("close");
        }
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
            Debug.Log(childSprite.color);
            childSprite.enabled = true;
            if (placedObjs.ContainsKey(loc)) childSprite.color = Color.red;
            else childSprite.color = Color.green;
            highlightObj.transform.position = new Vector3(loc.x, loc.y);
            Debug.Log(loc);
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
        if (chosenSignNum != null && !placedObjs.ContainsKey(loc))
        {
            GameObject newObj = Instantiate(mapObjs[(int)chosenSignNum].gameObject, new Vector3(loc.x, loc.y), Quaternion.identity, transform);
            placedObjs.Add(loc, newObj);
            updateAvailableSigns();
        }
    }
}
