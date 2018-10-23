using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
	public string name;
	[SerializeField] AudioClip [] sounds;
	[Range(0f, 1f)]
	public float volume;
	[Range(.1f, 3f)]
	public float pitch;
	public bool loop;
	[HideInInspector]
	public AudioSource source;

	public void Play()
	{
		source.clip = sounds[Random.Range(0, sounds.Length)];
		source.Play();
	}

	
}
