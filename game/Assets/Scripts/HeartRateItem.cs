using UnityEngine;
using System.Collections;

public class HeartRateItem : MonoBehaviour {
	
	public float HeartRateChange;
	
	private void OnTriggerEnter(Collider other)
	{
		InputControl player = other.transform.parent.GetComponent<InputControl>();
		if(player != null)
		{
			player.HeartRate += HeartRateChange;
			Destroy(gameObject);
		}
	}
}
