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
    public static AudioManager Instance { get; private set; }
    #endregion

    #region UnityFunctions
    void Awake ()
	{
        if (Instance == null)
        {
            Initialize();
        }
        else
        {
            Destroy(gameObject);
        }
	}
    #endregion

    private void Initialize()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
        PlaySound("MainTheme");
    }
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
		if (!found) print("Sound not found: " + audioName);
	}
}
