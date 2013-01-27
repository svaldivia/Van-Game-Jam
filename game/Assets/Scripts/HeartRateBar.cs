using UnityEngine;
using System.Collections;

public class HeartRateBar : MonoBehaviour {
	
	private float heartRate = 0.5f;
    public Vector2 pos = new Vector2(20,10);
    public Vector2 size = new Vector2(200,20);
    public Texture2D progressBarBackground;
	public Texture line;
	public Rect position;
	public InputControl control;
	
    void OnGUI()
	{
		if (GetComponent<PhotonView>().isMine)
		{
			position = new Rect(0.0f, 0.0f, size.x, size.y);
			
			// draw the background:
		    GUI.BeginGroup(new Rect(pos.x, pos.y, size.x, size.y));
		    GUI.Box(position, progressBarBackground);
		     
		    // draw the filled-in part:
		    GUI.BeginGroup(new Rect(heartRate * size.x, 0.0f, 1.0f, size.y));
		    GUI.Box(position, line);
		    GUI.EndGroup();
			GUI.EndGroup();
			GUILayout.BeginArea(new Rect(pos.x + size.x + 2.0f, 10.0f, 50.0f, 20.0f));
			if( heartRate > 0.25 && heartRate < 0.75 )
			{
				GUI.contentColor = Color.green;
			}
			else
			{
				GUI.contentColor = Color.red;
			}
			GUILayout.Label(string.Format("{0:0}", Mathf.Clamp(heartRate*100.0f, 0.0f, 100.0f)));
		    GUILayout.EndArea();
			GUI.contentColor = Color.white;
			GUILayout.BeginArea(new Rect(pos.x + 2.0f, 30.0f, 200.0f, 20.0f));
			GUILayout.Label(string.Format ("Score: {0:0}", Mathf.FloorToInt(control.Timer*1000).ToString()));
			GUILayout.EndArea();
		}
    }
     
    void Update()
    {
	    if (control.HeartRate > 0 && control.HeartRate < 1)
		{
			heartRate = control.HeartRate;
		}
    }
}
