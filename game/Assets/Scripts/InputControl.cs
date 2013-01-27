using UnityEngine;
using System.Collections;

public class InputControl : MonoBehaviour {
	
	protected Animator animator;	
	public float DirectionDampTime = .25f;
	public bool ApplyGravity = true;
	public int HeartRate = 80;
	public int ForwardSpeed = 50;
	public int LeftSideSpeed = 0;
	public int RightSideSpeed = 0;
	public int HeartRateChangeRate = 0;
	[SerializeField] private int mDistanceTravelled = 0;
	
	// Use this for initialization
	void Start() 
	{
		animator = GetComponent<Animator>();
		
		if(animator.layerCount >= 2)
			animator.SetLayerWeight(1, 1);
	}
		
	// Update is called once per frame
	void Update() 
	{
		if (animator)
		{	
			float v = 1.0f;
			float h = 0.0f;
			
			animator.SetFloat("Speed", h*h+v*v);
			
			AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);			
			
			if (stateInfo.IsName("Base Layer.Run"))
			{
				if (Input.GetKey(KeyCode.Space))
				{
					animator.SetBool("Jump", true);
				}
			}
			else
			{
				animator.SetBool("Jump", false);
			}
						
			if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
			{
				animator.SetFloat("Direction", -1, DirectionDampTime, Time.deltaTime);	
				LeftSideSpeed = LeftSideSpeed + 2;
				RightSideSpeed = 0;
			}
			else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
			{
				animator.SetFloat("Direction", 1, DirectionDampTime, Time.deltaTime);
				RightSideSpeed = RightSideSpeed + 2;
				LeftSideSpeed = 0;
			}
			else if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow))
			{				
				LeftSideSpeed = 0;
				RightSideSpeed = 0;
				animator.SetFloat("Direction", 0, 0, Time.deltaTime);				
			}
			
			mDistanceTravelled = Mathf.FloorToInt(transform.position.z);
			rigidbody.MovePosition(transform.position + (transform.forward * ForwardSpeed + transform.right * RightSideSpeed + (-transform.right) * LeftSideSpeed) * Time.deltaTime);
		}   		  
	}
}
