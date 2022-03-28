using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tile : MonoBehaviour
{
    public Manager manager;
    public Vector2Int coordinate;
    public int currentWeight = 0;
    // Start is called before the first frame update
    
    public void initialize(Manager manager, Vector2Int coordinate, int currentWeight)
    {
        this.manager = manager;
        this.coordinate = coordinate;
        this.currentWeight = currentWeight;
        updateDebugText();
    }
    
    public void OnMouseEnter()
    {
        
        manager.highlightTileWithSign(coordinate);
    }
    public void OnMouseUp()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            Debug.Log("Clicked on the UI");
        }
        else
        {
            manager.clickTile(coordinate);
        }
    }
    public void updateWeight(int newWeight, int mapRadius)
    {
        float percentOfMax = (float)(newWeight)/(float)(mapRadius);
        //GetComponent<SpriteRenderer>().color = new Color(percentOfMax, percentOfMax, 1.0f, 1.0f);
        currentWeight = newWeight;
        updateDebugText();
    
    }
    public void updateDebugText()
    {
         transform.GetChild(0).GetComponent<TextMesh>().text = currentWeight.ToString();
         
    }
    public void setPathImage(Vector2Int direction)
    {
        Transform pathImageTrans = transform.GetChild(1).transform;
        
        if (direction == Vector2Int.up)
        {
            pathImageTrans.rotation = Quaternion.Euler(0,0,90);
            
        }
        else if (direction == Vector2Int.left)
        {
            pathImageTrans.rotation = Quaternion.Euler(0,0,180 );
            
        }
        else if (direction == Vector2Int.down)
        {
            pathImageTrans.rotation = Quaternion.Euler(0,0,270);
            
        }
        else if (direction == Vector2Int.right)
        {
            pathImageTrans.rotation = Quaternion.Euler(0,0,0);
            
        }
    }
}
