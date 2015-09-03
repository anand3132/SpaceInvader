using UnityEngine;
using System.Collections;

//called when audio clip is needed
public class AudioManager : MonoBehaviour
{
	public AudioClip[] audioClip;
	public bool clipFound;
	AudioSource audioComponent;
	// Use this for initialization
	void Start ()
	{
		audioComponent = GetComponent<AudioSource>();
	}
	
	public void play(string _clipName)
	{
		for(int i=0;i<audioClip.Length;i++)
		{
			if (_clipName==audioClip[i].name)
			{			
				audioComponent.clip=audioClip[i];
				audioComponent.Play();
				return;
			}

		}//for loop

		Debug.Log("Audio File not found");
	}//play
}//MonoBehaviour
