using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObjectWeightCalculator
{
    public int getWeightAdjustment(Vector2Int distance, ObjectType objType);


    
}