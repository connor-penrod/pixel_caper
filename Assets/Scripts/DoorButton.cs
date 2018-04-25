using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoreMountains.CorgiEngine
{
    public class DoorButton : ButtonActivated
    {
        override public void TriggerButtonAction()
        {
            if (GameObject.FindObjectOfType<PaintingButton>().stolenPainting == true)
            { 
                MMEventManager.TriggerEvent(new CorgiEngineEvent(CorgiEngineEventTypes.Pause));//Pause Game
                GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterPause>().AbilityPermitted = false;//Turns off Ability to Pause/Unpause
                GUIManager gui = FindObjectOfType<GUIManager>();
                gui.PauseScreen.SetActive(false);
                gui.Win.SetActive(true);
            }//If Stolen Painting   
        }
    }//Controls Door
}
