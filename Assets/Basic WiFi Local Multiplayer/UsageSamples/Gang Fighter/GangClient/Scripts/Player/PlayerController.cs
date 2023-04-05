using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GangFighter{
public class PlayerController : MonoBehaviour {
public string	id;

	public string name;

	public int cont;

	public int character_index;

	
	public  GameObject[] models;

    public Animator[] animators;

	public bool isOnline;

	public bool isLocalPlayer;

	Animator myAnim;

	public Rigidbody myRigidbody;

	public enum state : int {idle,walk,attack,damage,dead};

	public state currentState;

	//distances low to arrive close to the player
	[Range(1f, 200f)][SerializeField] float minDistanceToPlayer = 10f ;

	public float verticalSpeed = 3.0f;

	public float rotateSpeed = 150f;

	float m_GroundCheckDistance = 1f;

	public float jumpPower = 12f;

	public float jumpTime=0.4f;

	public float jumpdelay=0.4f;

	public bool m_jump;

	public bool isJumping;

	public bool onGrounded;

	public bool isPunch;

	public bool isKick;

	public bool isPunchButton;

	public bool isKickButton;

	public float punchTime;

	public float kickTime;

	public float timeOut;

	public Transform cameraTotarget;

	float h ;
	
	float v;

	public bool move;
	
	bool attack;

	bool hit;



	public bool onMobileButton;

	
	[HideInInspector] public Joystick joystick;

	[HideInInspector] public bool onJoystick;
    
	public float maximumFingerDistance = 3000f;



	// Use this for initialization
	void Start () {

		myRigidbody = GetComponent<Rigidbody> ();
		
	}

	void Awake()
	{

		myRigidbody = GetComponent<Rigidbody> ();

		 #if UNITY_EDITOR  && UNITY_ANDROID || UNITY_ANDROID || UNITY_IOS
		   joystick = FindObjectOfType<Joystick> () as Joystick;
		   #else
		  
		   #endif

		
	}

	public void Set3DName(string name)
	{
		GetComponentInChildren<TextMesh> ().text = name;

	}

    void Update() {
		
		#if UNITY_EDITOR  && UNITY_ANDROID || UNITY_ANDROID || UNITY_IOS
	        ManageMobileInput();
		#endif
	}

	// Update is called once per frame
	void FixedUpdate () {
		

		if (isLocalPlayer )
		{
			Attack ();
			Move();
	
		}
	
		Jump ();

	}

	void Move( )
	{

			
		move = false;
		v = 0;
		h = 0;

		if(!GetComponent<PlayerHealth>().onRagdollAnim)
		{
			#if UNITY_EDITOR  && UNITY_ANDROID || UNITY_ANDROID || UNITY_IOS

		     ManageMobileMove();

		    #else //NOT_MOBILE_INPUT

		    // Store the input axes.
            h = Input.GetAxisRaw("Horizontal");
              
		    v = Input.GetAxisRaw("Vertical");

		    if(h!=0|| v!=0)
		    {
			  move = true; 
		    }

		    if (Input.GetKey("down") || Input.GetKey("up") ||Input.GetKey("left")|| Input.GetKey("right"))
		    {   
			  move = true;   
		    }

		    #endif

		}

		var x = h* Time.deltaTime *  verticalSpeed;
		var y = h * Time.deltaTime * rotateSpeed;
		var z = v * Time.deltaTime * verticalSpeed;

		transform.Rotate (0, y, 0);

		transform.Translate (0, 0, z);

		UpdateStatusToServer ();

		if (h != 0 || v != 0 || isJumping ) {
			currentState = state.walk;
		
			if(v>0)
			{
				UpdateAnimator ("IsWalk");
				NetworkManager.instance.EmitAnimation ("IsWalk");

			}
			else
			{
				UpdateAnimator ("IsWalkBack");
				NetworkManager.instance.EmitAnimation ("IsWalkBack");

			}
			
		}
		else
		{
			
			currentState = state.idle;
			UpdateAnimator ("IsIdle");
			NetworkManager.instance.EmitAnimation ("IsIdle");
		}

	}

	public void Jump()
	{
	    
		 
		RaycastHit hitInfo;

		onGrounded = Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out hitInfo, m_GroundCheckDistance);

		jumpTime -= Time.deltaTime;



		if (isLocalPlayer) 
		{
		   
			if(!onMobileButton)
		    {
               m_jump = Input.GetButton("Jump");

		    }
		}
	


		if (jumpTime <= 0 && isJumping && onGrounded)
		{

			m_jump = false;
			isJumping = false;
			UpdateAnimator ("IsJump");
		}



		if (m_jump && !isJumping) 
		{	


			myRigidbody.AddForce(Vector3.up * jumpPower, ForceMode.VelocityChange);

			jumpTime = jumpdelay;

			isJumping = true;

		}
		
	}


	void Attack()
	{

	
		if (isPunchButton || Input.GetKey (KeyCode.LeftControl))
		{
				
			currentState = state.attack;
			UpdateAnimator ("isPunch");
			NetworkManager.instance.EmitAnimation ("isPunch");
				
			string msg = id;
		}

			
		if (isKickButton || Input.GetKey (KeyCode.M))
		{
			
			currentState = state.attack;
			UpdateAnimator ("isKick");
			NetworkManager.instance.EmitAnimation ("isKick");
			string msg = id;
		}

		if(isKick || isPunch)
		{
			if(hit)
			{
				hit = false;
				foreach(KeyValuePair<string, PlayerController> enemy in NetworkManager.instance.networkPlayers)
			    {

				    if ( enemy.Key != id)
				    {
					
					    Vector3 meToEnemy = transform.position - enemy.Value.transform.position;
					    
					    //if i am close to any network player
					    if (meToEnemy.sqrMagnitude < minDistanceToPlayer &&!enemy.Value.GetComponent<PlayerHealth>().onRagdollAnim)
					    {

						//damage network player
						 NetworkManager.instance.EmitDamage (id, enemy.Key);
					    }
				    }
			    }
			}

			}
		
	}
	
	 void  ManageMobileMove()
	{
	
		Vector3 delta = joystick.GetDelta();
	
		h  = -1*delta.x;
        v =  delta.y;
		 //up button or joystick
		if (v>0)
		{
		    v =1;
		    move = true;
		}
		if (v<0)//down button or joystick
		{
			v=-1;
			move = true;

			
		}
		//up button or joystick
		if (h<0)
		{
		    h =-1;
		    move = true;
		
		 
		}
		if (h>0)//down button or joystick
		{
			h=1;
			 move = true;
			
		}
	

	}

	void ManageMobileInput()
	{
		if (Input.touchCount == 1) {
			
			Vector3 meToJoystick =
					Input.GetTouch (0).position - new Vector2 (joystick.GetComponent<RectTransform> ()
					.position.x, joystick.GetComponent<RectTransform> ()
					.position.y);

			//fingers close to joystick
			if (meToJoystick.sqrMagnitude < maximumFingerDistance) {
				onJoystick = true;
			
			}else {
				onJoystick = false;
					
			 }//END_IF
		}//END_IF

		
		if (Input.touchCount > 1 && Input.GetTouch (1).phase == TouchPhase.Moved) {

			Vector3 meToJoystick =
				Input.GetTouch (1).position - new Vector2 (joystick.GetComponent<RectTransform> ()
				.position.x, joystick.GetComponent<RectTransform> ()
				.position.y);

			//fingers close to joystick
			if (meToJoystick.sqrMagnitude < maximumFingerDistance)
			{
				onJoystick = true;
		
			} 
			else
			{
				onJoystick = false;
		
			}//END_ELSE

					
		}//END_IF

	}

	void UpdateStatusToServer ()
	{
		NetworkManager.instance.EmitPosAndRot(transform.position,transform.rotation);
	}


	public void UpdatePosition(Vector3 position) 
	{
		if (!isLocalPlayer) {

			if (!isJumping)
			{
				transform.position = new Vector3 (position.x, position.y, position.z);

				
			}
		}

	}

	public void UpdateRotation(Quaternion _rotation) 
	{
		if (!isLocalPlayer) 
		{
			transform.rotation = _rotation;
		

		}

	}



	public void UpdateAnimator(string _animation)
	{

	
			switch (_animation) { 
			case "IsWalk":
				if (!myAnim.GetCurrentAnimatorStateInfo (0).IsName ("Walk"))
				{
					myAnim.SetTrigger ("IsWalk");


				}
				break;

				case "IsWalkBack":
				if (!myAnim.GetCurrentAnimatorStateInfo (0).IsName ("WalkBack"))
				{
					myAnim.SetTrigger ("IsWalkBack");


				}
				break;

				case "IsGrab":
				if (!myAnim.GetCurrentAnimatorStateInfo (0).IsName ("Grab"))
				{
					myAnim.SetTrigger ("IsGrab");


				}
				break;

			case "IsIdle":

				if (!myAnim.GetCurrentAnimatorStateInfo(0).IsName("Idle") &&!isPunch &&!isKick)
				{
					myAnim.SetTrigger ("IsIdle");

				}
				break;

				
			case "IsJump":

				if (!myAnim.GetCurrentAnimatorStateInfo(0).IsName("Jump"))
				{
					myAnim.SetTrigger ("IsJump");

				}
				break;

			case "IsDamage":
				if (!myAnim.GetCurrentAnimatorStateInfo(0).IsName("Damage") ) 
				{
					myAnim.SetTrigger ("IsDamage");
				}
				break;

			case "isPunch":
				if (!myAnim.GetCurrentAnimatorStateInfo(0).IsName("Punch"))
				{
				
					myAnim.SetTrigger ("IsPunch");
					StartCoroutine ("StopPunch");

				}
				break;

				case "isKick":
				if (!myAnim.GetCurrentAnimatorStateInfo(0).IsName("Kick"))
				{
				
					myAnim.SetTrigger ("IsKick");
					StartCoroutine ("StopKick");

				}
				break;


			case "IsDead":
				if (!myAnim.GetCurrentAnimatorStateInfo(0).IsName("Dead"))
				{
					myAnim.SetTrigger ("IsDead");
				}
				break;

			}
	
	}



	public void UpdateJump()
	{
		m_jump = true;
	}

	// reload your weapon
	IEnumerator StopPunch()
	{
		if (isPunch)
		{
			yield break; // if already attack... exit and wait attack is finished
		}

		isPunch = true; // we are now attack

	
		yield return new WaitForSeconds(punchTime); // wait for set attack animation time
		isPunch = false;
		hit = true;


	}

	// reload your weapon
	IEnumerator StopKick()
	{
		if (isKick)
		{
			yield break; // if already attack... exit and wait attack is finished
		}

		isKick = true; // we are now attack
		yield return new WaitForSeconds(kickTime); // wait for set attack animation time
		isKick = false;
		hit = true;


	}

	//method used in the NetworkManager class to define the skin of the character chosen by the localPlayer
	public void SetUpCharacter(int _character_index)
	{
	   character_index = _character_index;
	    for(int i =0;i< models.Length;i++)
	  {
	    if(i.Equals(character_index))
		{
		  models[character_index].SetActive(true);  
		  GetComponent<PlayerHealth>().thirdPerson =  models[character_index];
		}
		else
		{
		   models[i].SetActive(false);  
		}
	  }
		 		
	}

	public void SetAnimator()
	{
		myAnim = animators[character_index];

	}



	public void EnableKey(string _key)
	 {
	 
	   onMobileButton = true;

	  
	   switch(_key)
	   {
	   
	     case "up":
		 v = 1;
		 break;
		 case "down":
		 v= -1;
		 break;
		 case "right":
		 h = 1;
		 break;
		 case "left":
		 h = -1;
		 break;
		case "jump":
		 m_jump = true;
		 break;
		 case "punch":
		 isPunchButton= true;
		 break;
		case "kick":
	
		 isKickButton = true;
		 break;
	   }
	 }
	 
	 public void DisableKey(string _key)
	 {
	   onMobileButton = false;
	   switch(_key)
	   {
	    case "up":
		 v = 0;
		 break;
		 case "down":
		 v= 0;
		 break;
		 case "right":
		 h = 0;
		 break;
		 case "left":
		 h = 0;
		 break;
		 case "jump":
		 m_jump =  false;
		 break;
		  case "punch":
		 isPunchButton= false;
		 break;
		case "kick":
	
		 isKickButton = false;
		 break;
		
	   }
	 }

}
}
