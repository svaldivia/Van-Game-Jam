using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NetworkCharacter : Photon.MonoBehaviour
{
    private Vector3 correctPlayerPos = Vector3.zero; // We lerp towards this
    private Quaternion correctPlayerRot = Quaternion.identity; // We lerp towards this
	public Camera cam = null;
	private bool mIsReady = false;
	private static bool mGameOn = false;
	public static Dictionary<int, NetworkCharacter> Characters = null;
	
		
    public void Awake()
    {
		
	//ThirdPersonController myC = GetComponent<ThirdPersonController>();
       // myC.isControllable = photonView.isMine;
		if (Characters == null)
		{
			Characters = new Dictionary<int, NetworkCharacter>();
		}
		
		gameObject.name = gameObject.name + photonView.viewID.ID; 
		Debug.Log("Creating char " + gameObject.name);
		this.correctPlayerPos = transform.position;
		this.correctPlayerRot = transform.rotation;
		
		Characters.Add(photonView.viewID.ID, this);
		if (!photonView.isMine)
		{
			cam.gameObject.SetActive(false);
		}
		else
		{
			Time.timeScale = 0f;
			StartCoroutine(WaitForStartInput());
		}
		
    }

    // Update is called once per frame
    public void Update()
    {
        if (!photonView.isMine)
        {
            transform.position = Vector3.Lerp(transform.position, this.correctPlayerPos, Time.deltaTime * 5);
            transform.rotation = Quaternion.Lerp(transform.rotation, this.correctPlayerRot, Time.deltaTime * 5);
        }
    }
	
	public IEnumerator WaitForStartInput()
	{
		while (!mIsReady)
		{
			if (Input.GetKeyDown(KeyCode.Return))
			{
				mIsReady = true;
				if (Characters != null && Characters.Count == 1)
				{
					Time.timeScale = 1f;
					mGameOn = true;
				}
			}
			yield return null;
		}
	}

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            // We own this player: send the others our data
			//stream.SendNext(photonView.viewID.ID);
			stream.SendNext(mIsReady);
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
			stream.SendNext(rigidbody.velocity);
			//stream.SendNext(GetComponent<InputControl>().HeartRate);
			
        }
        else
        {
            // Network player, receive data
			//int id = (int)stream.ReceiveNext();
			mIsReady = (bool)stream.ReceiveNext();
            this.correctPlayerPos = (Vector3)stream.ReceiveNext();
            this.correctPlayerRot = (Quaternion)stream.ReceiveNext();
			rigidbody.velocity = (Vector3)stream.ReceiveNext();
			/*
			if (Characters != null)
			{
				if (Characters.ContainsKey(id))
				{
					Characters[id].mIsReady = true;
				}
				else
				{
					Characters.Add(id);
					Characters[id].mIsReady = true;
				}
					
			}*/
			//GetComponent<InputControl>().HeartRate = (float)stream.ReceiveNext();
        }
		
		if (!mGameOn && Characters != null)
		{
			int readyCount = 0;
			int playerCount = Characters.Count;
			
			foreach (NetworkCharacter character in Characters.Values)
			{
				if (character.mIsReady)
				{
					readyCount++;
				}
			}
			Debug.Log(readyCount);
			if (readyCount == playerCount)
			{
				mGameOn = true;
				Time.timeScale = 1f;
			}
		}
    }
}
