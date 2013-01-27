using UnityEngine;
using System.Collections;

public class HeartRateItem : MonoBehaviour {
	
	public float HeartRateChange;
	public float RespawnTime;
	public ParticleSystem ParticleSys;
	public Collider Coll;
	
	
	private void OnTriggerEnter(Collider other)
	{
		InputControl player = other.transform.parent.GetComponent<InputControl>();
		if(player != null)
		{
			player.HeartRate += HeartRateChange;
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
