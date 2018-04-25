using MoreMountains.CorgiEngine;
using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LasersHandler : MonoBehaviour {


    private void OnTriggerEnter2D(Collider2D collision)
    {
        //print("Collided with" + collision.gameObject);
        if (collision.gameObject.CompareTag("Player")==true)
        {
            MMEventManager.TriggerEvent(new CorgiEngineEvent(CorgiEngineEventTypes.Pause));//Pause Game
            GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterPause>().AbilityPermitted = false;//Turns off Ability to Pause/Unpause
            GUIManager gui= FindObjectOfType<GUIManager>();
            gui.PauseScreen.SetActive(false);
            gui.Death.SetActive(true);//Gets GUI, turns off pause menu and turns on death menu
            //Character character = FindObjectOfType<Character>();
            //character.GetComponent<GrapplingManager>().enabled = false;
            //character.GetComponent<FlashlightManager>().flashlight.GetComponent<MeshRenderer>().enabled = false;
            //character.GetComponent<FlashlightManager>().enabled = false;//Turns off Grappling hook and flashlight while dead
            //LoadingSceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }//If Player Enters Lasers
    }
}
