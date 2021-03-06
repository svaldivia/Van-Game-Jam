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
	public GUIText TextObject = null;
	public string Text = "Press enter to ready up";
	private int ReadyCount = 0;
	
    public void Awake()
    {
		if (TextObject != null)
		{
			TextObject.text = Text;
			if (!photonView.isMine)
			{
				TextObject.gameObject.SetActive(false);
			}
		}
		
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
	
	public void OnDisable()
	{
		if (Characters.ContainsKey(photonView.viewID.ID))
		{
			Characters.Remove(photonView.viewID.ID);
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
					TextObject.text = "";
					TextObject.gameObject.SetActive(false);
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
			ReadyCount = 0;
			int playerCount = Characters.Count;
			
			foreach (NetworkCharacter character in Characters.Values)
			{
				if (character.mIsReady)
				{
					ReadyCount++;
				}
			}
			TextObject.text = string.Format("Waiting for {0} players. Press enter to ready", (Characters.Count - ReadyCount).ToString());
			if (ReadyCount == playerCount)
			{
				TextObject.text = "";
				TextObject.gameObject.SetActive(false);
				mGameOn = true;
				Time.timeScale = 1f;
				
			}
		}
    }
}
