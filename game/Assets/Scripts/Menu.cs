using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {
	
	public bool isQuit = false;
	public string loadLevelName = "";
	
	void OnMouseEnter()
	{
		renderer.material.color = Color.red;
	}
	
	void OnMouseExit()
	{
		renderer.material.color = Color.white;
	}
	void OnMouseDown()
	{
		if(isQuit)
		{
			Application.Quit();
		}else{
			Application.LoadLevel(loadLevelName);
		}
	}
}
