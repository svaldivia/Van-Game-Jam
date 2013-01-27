using UnityEngine;
using System.Collections;

public class RandomMatchmaker : Photon.MonoBehaviour
{
	public string CharacterPrefabName = "Teddy";
	public GameObject CharacterPrefab = null;
	
    // Use this for initialization
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings("0.1");
    }

    void OnJoinedLobby()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    void OnPhotonRandomJoinFailed()
    {
        PhotonNetwork.CreateRoom(null, true, true, 4);  // no name (gets a guid), visible and open with 4 players max
    }

    void OnJoinedRoom()
    {
        PhotonNetwork.Instantiate("Teddy", CharacterPrefab.transform.position, Quaternion.identity, 0);
    }

}