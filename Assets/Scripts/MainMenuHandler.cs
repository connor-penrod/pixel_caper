using MoreMountains.CorgiEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuHandler : MonoBehaviour {

    private bool instructionsOn = false;
    public GameObject instuctionsWindow;
    private bool controlsOn = false;
    public GameObject controlsWindow;

    public void goToLevel(string Level)
    {
        LoadingSceneManager.LoadScene(Level);
    }

    public void quit()
    {
        Application.Quit();//Exits
    }

    public void instructions()
    {
        if(!instructionsOn)
        {
            instuctionsWindow.SetActive(true);
            instructionsOn = true;
        }
        else
        {
            instuctionsWindow.SetActive(false);
            instructionsOn = false;
        }//Toggles instructions on click
    }

    public void controls()
    {
        if (!controlsOn)
        {
            controlsWindow.SetActive(true);
            controlsOn = true;
        }
        else
        {
            controlsWindow.SetActive(false);
            controlsOn = false;
        }//Toggles instructions on click
    }
}
