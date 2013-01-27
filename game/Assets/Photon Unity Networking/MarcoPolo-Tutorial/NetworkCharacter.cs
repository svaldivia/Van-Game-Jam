using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NetworkCharacter : Photon.MonoBehaviour
{
    private Vector3 correctPlayerPos = Vector3.zero; // We lerp towards this
    private Quaternion correctPlayerRot = Quaternion.identity; // We lerp towards this
	private bool mIsControllable = false;
		
    public void Awake()
    {
	//ThirdPersonController myC = GetComponent<ThirdPersonController>();
       // myC.isControllable = photonView.isMine;
		mIsControllable = photonView.isMine;
		gameObject.name = gameObject.name + photonView.viewID.ID; 
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

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            // We own this player: send the others our data
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            // Network player, receive data
            this.correctPlayerPos = (Vector3)stream.ReceiveNext();
            this.correctPlayerRot = (Quaternion)stream.ReceiveNext();
        }
    }
}
