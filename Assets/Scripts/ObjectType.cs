using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectType
{
    public string name;

    //these constants really suck, but they are used to determine how the weight calculator works
    //see the weight calculator to determine how they are used.
    //sign is more described because it determines if the weight is increased or decreased;
    //max radius determines how far the effect goes, -1 for infinite

    public int sign, maxRadius, baseEffect, a, b;


}