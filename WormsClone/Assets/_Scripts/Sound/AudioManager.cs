using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	#region ShowInEditor
	[SerializeField] Sound[] sounds;
    #endregion
    #region HideInEditor
    public static AudioManager Instance;
    #endregion

    #region MagicFunctions
    void Awake ()
	{
		foreach (Sound s in sounds)
		{
			s.source = gameObject.AddComponent<AudioSource>();
			s.source.volume = s.volume;
			s.source.pitch = s.pitch;
			s.source.loop = s.loop;
		}
	}
	private void Start()
	{
		if(Instance == null)
		{
			Instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(gameObject);
		}

		PlaySound("MainTheme");
	}
    #endregion

    /// <summary>
    /// Plays the sound with the given name, if it's not found an error will be logged to the console.
    /// </summary>
    /// <param name="audioName"></param>
    public void PlaySound(string audioName)
	{
		bool found = false;
		for(var i = 0; i < sounds.Length; i++)
		{
			if(sounds[i].name == audioName)
			{
				sounds[i].Play();
				found = true;
			}
		}
		//if (!found) print("Sound not found: " + audioName);
	}
}
