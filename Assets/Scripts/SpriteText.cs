using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 public class SpriteText : MonoBehaviour
 {
     public string sortingLayer;
     void Start()
    {
        //for some reason you cant set the sorting layer of text meshes in the editor
        Renderer renderer = GetComponent<Renderer>();
        renderer.sortingLayerID = SortingLayer.NameToID( sortingLayer);
        
        
    }
 }