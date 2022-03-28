using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void clickPlay()
    {
        SceneManager.LoadScene("IntroScene", LoadSceneMode.Single);
    }
    public void clickStartActualGame()
    {
        SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
    }
    public void clickBackToMainMenu()
    {
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }
    public void clickExit()
    {
        Application.Quit();
    }
}
