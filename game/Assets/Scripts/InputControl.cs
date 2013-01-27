using UnityEngine;
using System.Collections;

public class InputControl : MonoBehaviour {
	
	protected Animator animator;	
	public float DirectionDampTime = .25f;
	public float HeartRate = 0.01f;
	public float TurnSpeed;
	public float JumpForce;
	private float mRotation = 0;
	public float HeartRateChangeRate = 0.01f;
	public float MaxSpeed;
	private bool Jumping = false;
	public float JumpTime;
	private float timer = 0.25f;
	public float AnimatorSpeed;
	public AudioSource HeartBeatSource;
	public AnimationCurve HeartBeatCurve;
	
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
		
		//HeartBeatSource.pitch = HeartBeatCurve.Evaluate(HeartRate);
			
		HeartRate += HeartRateChangeRate * Time.deltaTime * Mathf.Sign(HeartRate - 0.5f);
		if(HeartRate < 0 || HeartRate > 1)
		{
			rigidbody.AddForce(new Vector3(0, JumpForce,0));
		}
		
		if (animator)
		{	
			
			AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);			
			
			if(stateInfo.IsName("Base Layer.Jump") && Jumping)
			{
				if(stateInfo.normalizedTime > JumpTime)
				{
					rigidbody.AddForce(new Vector3(0,JumpForce,0));
					Jumping = false;
				}
			}
			
			animator.SetBool("Jump", false);
			
			if (stateInfo.IsName("Base Layer.Run"))
			{
				if (Input.GetKey(KeyCode.Space))
				{
					animator.SetBool("Jump", true);
					Jumping = true;
					
				}
				animator.speed = Mathf.Sqrt(HeartRate * 2);
			}
			else
			{
				animator.speed = AnimatorSpeed;	
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
			rigidbody.MovePosition(transform.position + transform.forward * HeartRate * MaxSpeed * Time.deltaTime);
		}   		  
	}
}
