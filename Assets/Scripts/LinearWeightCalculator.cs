using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class LinearWeightCalculator : IObjectWeightCalculator
{
    public int getWeightAdjustment(Vector2Int distance, ObjectType objType)
    {
        if (objType.maxRadius == -1 || distance.sqrMagnitude <= Math.Pow(objType.maxRadius, 2))
        {
            if (objType.baseEffect == 0) 
            {
                
                return objType.sign * (int)(distance.magnitude * objType.a);
            }
            else 
            {
                float step;
                if (objType.maxRadius != -1)
                    step = objType.baseEffect / objType.maxRadius;
                else step = objType.a;
                
                return objType.sign * Math.Max((objType.baseEffect - (int) (distance.magnitude * step)),0);
            }
        }
        else 
        {
            
            return 0;
        }
    }


    
}