using UnityEngine;
using System.Collections;

public class InputControl : MonoBehaviour {
	
	protected Animator animator;	
	public float DirectionDampTime = .25f;
	public float HeartRate = 0.01f;
	public float TurnSpeed;
	private float mRotation = 0;
	public int HeartRateChangeRate = 0;
	
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
				mRotation = -1;
			}
			else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
			{
				mRotation = 1;
			}
			else if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow))
			{				
				mRotation = 0;
			}
			Debug.Log(mRotation);
			
			animator.SetFloat("Speed", HeartRate);
			animator.SetFloat("Direction", mRotation, DirectionDampTime, Time.deltaTime);	
			rigidbody.rotation.Set(transform.rotation.x, transform.rotation.y + mRotation * TurnSpeed, transform.rotation.z, transform.rotation.w);
			Debug.Log(rigidbody.rotation);
			rigidbody.MovePosition(transform.position + transform.forward * HeartRate * Time.deltaTime);
		}   		  
	}
}
