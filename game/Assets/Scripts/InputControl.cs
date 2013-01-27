using UnityEngine;
using System.Collections;

public class InputControl : MonoBehaviour {
	
	protected Animator animator;
	public bool ApplyGravity = true;
	public float ForwardSpeed = 1.0f;
	public float LeftSideSpeed = 0.0f;
	public float RightSideSpeed = 0.0f;
	[SerializeField] private float DistanceTravelled = 0.0f;
	
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
			
			AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);			
			
			if (stateInfo.IsName("Base Layer.Run"))
			{
				if (Input.GetKey(KeyCode.Space))
				{
					animator.SetBool("Jump", true);
					if ((ForwardSpeed - 1) > 0)
					{
						ForwardSpeed = ForwardSpeed * 0.5f;
					}
				}
				else if (Input.GetKey(KeyCode.LeftShift))
				{
					animator.SetBool("Dive", true);
					if ((ForwardSpeed - 1) > 0)
					{
						ForwardSpeed = ForwardSpeed * 0.5f;
					}
				}		
			}
			else
			{
				animator.SetBool("Jump", false);
				animator.SetBool("Dive", false);
            }		
			
			if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
			{
				v = v * 1.1f;
				animator.SetFloat("Speed", h*h+v*v);
				
				ForwardSpeed = ForwardSpeed + 7.0f;
			}		
			/*
			else if (Input.GetKey(KeyCode.S))
			{
				v = v * 1.1f;			
				animator.SetFloat("Speed", h*h+v*v);
				
				if( ForwardSpeed - 5 > 0)
				{
					ForwardSpeed = ForwardSpeed - 5;
				}
			}
			*/
						
			if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
			{
				animator.SetFloat("Direction", 0, 0, Time.deltaTime);	
				LeftSideSpeed = LeftSideSpeed + 2.0f;
				RightSideSpeed = 0.0f;
				transform.position += (-transform.right) * LeftSideSpeed * Time.deltaTime;
			}
			else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
			{
				animator.SetFloat("Direction", 0, 0, Time.deltaTime);
				RightSideSpeed = RightSideSpeed + 2.0f;
				LeftSideSpeed = 0.0f;
				transform.position += transform.right * RightSideSpeed * Time.deltaTime;
			}
			else if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow))
			{				
				LeftSideSpeed = 0.0f;
				RightSideSpeed = 0.0f;
			}
			
			if (ForwardSpeed > 0 )
			{
				ForwardSpeed = ForwardSpeed*0.95f;
				
				if (ForwardSpeed < 1)
				{
					ForwardSpeed = 0;
				}				
			}			
			transform.position += transform.forward * ForwardSpeed * Time.deltaTime;
			DistanceTravelled = transform.position.z;
		}   		  
	}
}
