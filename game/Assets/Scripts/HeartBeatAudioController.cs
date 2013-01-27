using UnityEngine;
using System.Collections;

public class HeartBeatAudioController : MonoBehaviour {

	public InputControl inputControl;
	public AudioSource HeartBeatSource;
	public float HeartBeatFrequency;
	private float timer = 1;
	public AnimationCurve HeartBeatFovCurve;
	public AnimationCurve HeartBeatPitchCurve;
	
	private void Update()
	{
		Camera.mainCamera.fov = Mathf.Clamp(50 + 5 * HeartBeatFovCurve.Evaluate(timer), 50, 60);
		HeartBeatSource.pitch = HeartBeatPitchCurve.Evaluate(inputControl.EffectiveHeartRate);
		if(timer <= 0)
		{
			timer = HeartBeatFrequency;	
		}
		timer -= HeartBeatSource.pitch / HeartBeatSource.clip.length * Time.deltaTime;
		
		
	}
	
}
