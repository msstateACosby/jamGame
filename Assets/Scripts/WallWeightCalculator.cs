using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallWeightCalculator : IObjectWeightCalculator
{
    // Start is called before the first frame update
    public int getWeightAdjustment(Vector2Int distance, ObjectType objType)
    {
        if (objType.a == 1)
        {
            if (distance.y == 0 || distance.magnitude <= objType.maxRadius)
            {
                return objType.sign * objType.baseEffect;
            }
            else return 0;
        }
        else 
        {
            if (distance.x == 0 || distance.magnitude <= objType.maxRadius)
            {
                return objType.sign * objType.baseEffect;
            }
            else return 0;
        }
        
    }
}
