using MoreMountains.CorgiEngine;
using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PauseHandler : MonoBehaviour, MMEventListener<CorgiEngineEvent>
{
    private bool paused = false;

    GameObject player;
    GrapplingManager grapple;
    FlashlightManager flashlightManager;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        grapple = player.GetComponent<GrapplingManager>();
        flashlightManager = player.GetComponent<FlashlightManager>();//gets flashlight and grapple
    }

    private void OnEnable()
    {
        this.MMEventStartListening<CorgiEngineEvent>();
    }

    private void OnDisable()
    {
        this.MMEventStopListening<CorgiEngineEvent>();
    }

    public void OnMMEvent(CorgiEngineEvent pauseEvent)
    {
        if (pauseEvent.EventType == CorgiEngineEventTypes.Pause)
        {
            if (!paused)
            {
                grapple.enabled = false;
                flashlightManager.enabled = false;
                paused = true;
            }
            else
            {
                grapple.enabled = true;
                flashlightManager.enabled = true;
                paused = false;
            }
        }
        if (pauseEvent.EventType == CorgiEngineEventTypes.UnPause)
        {
            grapple.enabled = true;
            flashlightManager.enabled = true;
        }
    }//Enables/Disables on Unpause/Pause
}
