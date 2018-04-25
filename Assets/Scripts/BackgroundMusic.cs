using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.CorgiEngine;

public class BackgroundMusic : MonoBehaviour {

	public AudioSource audio;

	// Use this for initialization
	void Start () {
		SoundManager sm = FindObjectOfType<SoundManager>();
		sm.PlayBackgroundMusic(audio);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
