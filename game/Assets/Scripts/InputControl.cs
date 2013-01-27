using UnityEngine;
using System.Collections;

public class InputControl : MonoBehaviour {
	
	protected Animator animator;	
	public float DirectionDampTime = .10f;
	public float HeartRate = 0.01f;
	public float TurnSpeed;
	public float JumpForce;
	private float mRotation = 0;
	public float HeartRateChangeRate = 0.01f;
	public float MinHeartRateChange = 0.01f;
	public float MaxSpeed;
	private bool Jumping = false;
	public float JumpTime;
	public float Timer = 0.0f;
	public float AnimatorSpeed;
	public AudioSource HeartBeatSource;
	public AnimationCurve HeartBeatCurve;
	public string GameOverSceneName = "GameOverMenu";
	public PhotonView view = null;
	
	// Use this for initialization
	void Start() 
	{
		animator = GetComponent<Animator>();
		
		if(animator.layerCount >= 2)
			animator.SetLayerWeight(1, 1);
	}
	
	IEnumerator LoadLevel(float delay)
	{
		yield return new WaitForSeconds(delay);
		Application.LoadLevel(GameOverSceneName);
	}
	
	public float EffectiveHeartRate
	{
		get
		{
			return (-(Mathf.Pow(Mathf.Clamp(Mathf.Abs(2 * (HeartRate -0.5f)), 0, 1), 0.5f)) + 1);
		}
	}
	
	// Update is called once per frame
	void Update() 
	{
		
		//HeartBeatSource.pitch = HeartBeatCurve.Evaluate(HeartRate);
		if (view != null && view.isMine)
		{
			HeartRate += Time.deltaTime * ((HeartRate - 0.5f) * HeartRateChangeRate + (MinHeartRateChange * Mathf.Sign(HeartRate - 0.5f)) );
			if((HeartRate < 0 || HeartRate > 1) && ((Application.loadedLevelName != "Menu") && (Application.loadedLevelName != "GameOverMenu")))
			{
				rigidbody.AddForce(new Vector3(0, JumpForce,0));
				StartCoroutine(LoadLevel(3));			
			}
		}
		
		if (view != null && animator && view.isMine)
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
				animator.speed = EffectiveHeartRate;
				//Debug.Log(EffectiveHeartRate);	
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
			
			animator.SetFloat("Speed", EffectiveHeartRate);
			animator.SetFloat("Direction", mRotation, DirectionDampTime, Time.deltaTime);	
			rigidbody.rotation.Set(transform.rotation.x, transform.rotation.y + mRotation * TurnSpeed * Time.deltaTime, transform.rotation.z, transform.rotation.w);
			rigidbody.MovePosition(transform.position + transform.forward * EffectiveHeartRate * MaxSpeed * Time.deltaTime);
		}
		if (HeartRate > 0 && HeartRate < 1)
		{
			Timer += Time.deltaTime;
		}
	}
}
