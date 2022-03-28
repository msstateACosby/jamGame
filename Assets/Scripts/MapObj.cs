using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MapObj : MonoBehaviour
{
    
    public Vector2Int loc;

    public ObjectType objectType;
    
    
    public IObjectWeightCalculator weightCalculator;
    public enum CalculatorType {Linear, Wall}

    public CalculatorType calculatorType;

    void Awake()
    {
        switch(calculatorType)
        {
        case CalculatorType.Linear:
            weightCalculator = new LinearWeightCalculator();
            break;
        case CalculatorType.Wall:
            weightCalculator = new WallWeightCalculator();
            break;
        }
        loc.x = (int)transform.position.x;
        loc.y = (int)transform.position.y;
        
    }
    public void addToMapWeights(int[,] mapWeights, int mapRadius)
    {
        //really ugly but lets me be kind of lazy
        //basically makes sure the chasm in the middles effect is equal to the outside.
        if (objectType.name == "chasm") objectType.baseEffect = mapRadius *  objectType.a;


        for (int i = 0; i < mapWeights.GetLength(0); i ++)
        {
            for (int j = 0; j < mapWeights.GetLength(1); j++)
            {
                
                mapWeights[i,j] += weightCalculator.getWeightAdjustment(new Vector2Int(i -mapRadius, j - mapRadius) -  loc, objectType);
                mapWeights[i,j] = Math.Max(mapWeights[i,j], 0);
            }
        }
    }
    public bool isCollecting()
    {
        return objectType.collecting;
    }
    
}
