using UnityEngine;
using System.Collections;
using MoreMountains.Tools;

namespace MoreMountains.CorgiEngine
{	
	/// <summary>
	/// This persistent singleton handles sound playing
	/// </summary>
	[AddComponentMenu("Corgi Engine/Managers/Sound Manager")]
	public class SoundManager : PersistentSingleton<SoundManager>, MMEventListener<MMSfxEvent>
	{	
		[Header("Music")]
		/// true if the music is enabled	
		public bool MusicOn=true;
		/// the music volume
		[Range(0,1)]
		public float MusicVolume=0.3f;

		[Header("Sound Effects")]
		/// true if the sound fx are enabled
		public bool SfxOn=true;
		/// the sound fx volume
		[Range(0,1)]
		public float SfxVolume=1f;

	    protected AudioSource _backgroundMusic;	
					
		/// <summary>
		/// Plays a background music.
		/// Only one background music can be active at a time.
		/// </summary>
		/// <param name="Clip">Your audio clip.</param>
		public virtual void PlayBackgroundMusic(AudioSource Music)
		{
			// if the music's been turned off, we do nothing and exit
			if (!MusicOn)
				return;
			// if we already had a background music playing, we stop it
			if (_backgroundMusic!=null)
				_backgroundMusic.Stop();
			// we set the background music clip
			_backgroundMusic=Music;
			// we set the music's volume
			_backgroundMusic.volume=MusicVolume;
			// we set the loop setting to true, the music will loop forever
			_backgroundMusic.loop=true;
			// we start playing the background music
			_backgroundMusic.Play();		
		}	
		
		/// <summary>
		/// Plays a sound
		/// </summary>
		/// <returns>An audiosource</returns>
		/// <param name="sfx">The sound clip you want to play.</param>
		/// <param name="location">The location of the sound.</param>
		/// <param name="loop">If set to true, the sound will loop.</param>
		public virtual AudioSource PlaySound(AudioClip sfx, Vector3 location, bool loop=false)
		{
			if (!SfxOn)
				return null;
			// we create a temporary game object to host our audio source
			GameObject temporaryAudioHost = new GameObject("TempAudio");
			// we set the temp audio's position
			temporaryAudioHost.transform.position = location;
			// we add an audio source to that host
			AudioSource audioSource = temporaryAudioHost.AddComponent<AudioSource>() as AudioSource; 
			// we set that audio source clip to the one in paramaters
			audioSource.clip = sfx; 
			// we set the audio source volume to the one in parameters
			audioSource.volume = SfxVolume;
			// we set our loop setting
			audioSource.loop = loop;
			// we start playing the sound
			audioSource.Play(); 

			if (!loop)
			{
				// we destroy the host after the clip has played
				Destroy(temporaryAudioHost, sfx.length);
			}

			// we return the audiosource reference
			return audioSource;
		}

		/// <summary>
		/// Stops the looping sounds if there are any
		/// </summary>
		/// <param name="source">Source.</param>
		public virtual void StopLoopingSound(AudioSource source)
		{
			if (source != null)
			{
				Destroy(source.gameObject);
			}
		}

		public virtual void OnMMEvent(MMSfxEvent sfxEvent)
		{
			PlaySound (sfxEvent.ClipToPlay, this.transform.position);
		} 

		protected virtual void OnEnable()
		{
			this.MMEventStartListening<MMSfxEvent>();
		}

		protected virtual void OnDisable()
		{
			if (_enabled)
			{
				this.MMEventStopListening<MMSfxEvent>();	
			}
		}
	}
}