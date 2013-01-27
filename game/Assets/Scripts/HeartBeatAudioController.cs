using UnityEngine;
using System.Collections;

public class HeartBeatAudioController : MonoBehaviour {

	public InputControl inputControl;
	public AudioSource HeartBeatSource;
	public float HeartBeatFrequency;
	public float HeartBeatSpeed;
	private float timer = 1;
	public AnimationCurve HeartBeatFovCurve;
	public AnimationCurve HeartBeatPitchCurve;
	
	private void Update()
	{
		Camera.mainCamera.fov = 50 + 5 * HeartBeatFovCurve.Evaluate(timer);
		HeartBeatSource.pitch = HeartBeatPitchCurve.Evaluate(inputControl.HeartRate);
		if(timer <= 0)
		{
			timer = HeartBeatFrequency;	
		}
		timer -= HeartBeatSource.pitch / HeartBeatSource.clip.length * Time.deltaTime;
		
		
	}
	
}
