using UnityEngine;
using System.Collections;

public class HeartRateBar : MonoBehaviour {
	
	public float heartRate = 120.0f;
    public Vector2 pos = new Vector2(20,10);
    public Vector2 size = new Vector2(200,20);
    public Texture2D progressBarBackground;
	public Texture line;
	public Rect position;
	
    void OnGUI()
    {	     
	    position = new Rect(0.0f, 0.0f, size.x, size.y);
		
		// draw the background:
	    GUI.BeginGroup(new Rect(pos.x, pos.y, size.x, size.y));
	    GUI.Box(position, progressBarBackground);
	     
	    // draw the filled-in part:
	    GUI.BeginGroup(new Rect(heartRate, 0.0f, 1.0f, size.y));
	    GUI.Box(position, line);
	    GUI.EndGroup();
	    GUI.EndGroup();
    }
     
    void Update()
    {
	    // for this example, the bar display is linked to the current time,
	    // however you would set this value based on your desired display
	    // eg, the loading progress, the player's health, or whatever.
		heartRate++;
    }
}
