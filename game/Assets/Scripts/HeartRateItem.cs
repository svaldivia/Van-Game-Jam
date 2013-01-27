using UnityEngine;
using System.Collections;

public class HeartRateItem : MonoBehaviour {
	
	public bool HeartRateChange;
	public float RespawnTime;
	public ParticleSystem ParticleSys;
	public Collider Coll;
	
	
	private void OnTriggerEnter(Collider other)
	{
		InputControl player = other.transform.parent.GetComponent<InputControl>();
		if(player != null)
		{
			if(player.HeartRate > 0.5 && !HeartRateChange)
			{
				player.HeartRate = 0.45f;
			}
			else if(player.HeartRate < 0.5 && HeartRateChange)
			{
				player.HeartRate = 0.55f;
			}
			
			Coll.enabled = false;
			ParticleSys.enableEmission = false;
			ParticleSys.Clear();
			StartCoroutine(Respawn());
		}
	}
				
	private IEnumerator Respawn()
	{
		yield return new WaitForSeconds(RespawnTime);
		Coll.enabled = true;
		ParticleSys.enableEmission = true;
	}
}
