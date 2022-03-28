using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseHandler : MonoBehaviour
{
    public RectTransform pauseMenu;
    // Start is called before the first frame update
    
    public void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            pauseMenu.gameObject.SetActive(!pauseMenu.gameObject.activeSelf);

        }
    }
}
