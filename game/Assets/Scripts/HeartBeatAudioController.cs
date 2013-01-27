using UnityEngine;
using System.Collections;

public class HeartBeatAudioController : MonoBehaviour {

	public InputControl inputControl;
	public float BeatLength;
	public float BeatFrequency;
	public AudioSource HeartBeatSource;
	private float Scaledtimer = 0;
	private float Realtimer = 0;
	
	private void Update()
	{
		if(Realtimer >= BeatLength && HeartBeatSource.isPlaying)
		{
			HeartBeatSource.Stop();
			Realtimer = 0;
		}
		if(Scaledtimer >= BeatFrequency)
		{
			HeartBeatSource.Play();
			Realtimer = 0;
			Scaledtimer =0;

		}
		
		Scaledtimer += inputControl.HeartRate * Time.deltaTime;
		Realtimer += Time.deltaTime;
	}
	
}
