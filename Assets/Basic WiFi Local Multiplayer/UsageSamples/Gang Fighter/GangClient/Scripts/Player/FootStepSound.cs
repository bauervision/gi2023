using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GangFighter{
public class FootStepSound : MonoBehaviour {

	public AudioSource footAudioSource ;

	public AudioClip[] woodSteps;
	public AudioClip[] metalSteps ;
	public AudioClip[] concreteSteps;
	public  AudioClip[] sandSteps ;


	private Transform myTransform;

	public   LayerMask hitLayer;
	private  string cTag;
	public bool step;
	public bool inicialDelay;
	public bool isMoving;
	float audioStepDelayWalk = 0.45f;
	float audioStepDelayRun = 0.25f;
	public bool onHit;
	public float offset;

	// Use this for initialization
	void Start () {
		
		myTransform = transform;
		step = true;
		footAudioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {

			// read inputs
		isMoving = false;
		
		float v = 0;
		float h = 0;

	
		//up button or joystick
		if (!GetComponentInParent<PlayerHealth>().isDead&&GetComponentInParent<PlayerController>().move&&
		GetComponentInParent<PlayerController>().onGrounded)
		{
		    isMoving = true;
		}
			
		
		OnFootStrike ();

	}

	public void OnFootStrike () 
	{
		//if player not moving
		if (!isMoving) 
		{
			inicialDelay = false;//clear the variable inicialDelay
		}

		  //  GetComponent<AudioSource>().volume = Mathf.Clamp01(0.1f + myRigidbody.velocity.magnitude * 0.3f);
		if (isMoving && step)
		{
			//if the player was  began to run, we have to give a delay also for begin  of the movement 
			if (!inicialDelay) 
			{
				StartCoroutine (WaitForFootSteps (audioStepDelayRun));//call coroutine 
				inicialDelay = true;
			}
			else
			{
			     GetComponent<AudioSource>().PlayOneShot (GetAudio ());
				
				StartCoroutine (WaitForFootSteps (audioStepDelayRun));//call coroutine for wait other foot
			}
		}
	}


	public AudioClip GetAudio()
	{
		RaycastHit hit;
       
		onHit = Physics.Raycast(myTransform.position + new Vector3(0, offset, 0), -Vector3.up,out hit, Mathf.Infinity, hitLayer);
		if(Physics.Raycast(myTransform.position + new Vector3(0, offset, 0), -Vector3.up,out hit, Mathf.Infinity, hitLayer))
		{
			cTag = hit.collider.tag.ToLower(); 
		}
       
		if(cTag == "Wood")
		{
			return woodSteps[Random.Range(0, woodSteps.Length)];
		}
		else if(cTag == "Metal")
		{
			return metalSteps[Random.Range(0, metalSteps.Length)];
		}
		else if(cTag == "Floor")
		{
			
			//GetComponent<AudioSource>().volume = 0.8f;
			return concreteSteps[Random.Range(0, concreteSteps.Length)];
		}
		else if(cTag == "Dirt")
		{
		//	GetComponent<AudioSource>().volume = 1.0f;
			return sandSteps[Random.Range(0, sandSteps.Length)];
		}
		else if(cTag == "Sand")
		{
			//GetComponent<AudioSource>().volume = 1.0f;
			return sandSteps[Random.Range(0, sandSteps.Length)];
		}
		else
		{
			//GetComponent<AudioSource>().volume = 1.0f;
			return concreteSteps[Random.Range(0, concreteSteps.Length)];
		}
	}

	//  is necessary to wait  the other foot of the player reaches the ground 
	IEnumerator WaitForFootSteps(float _stepDelay) 
	{
		step = false;
		yield return new WaitForSeconds(_stepDelay);
		step = true;
	} 
}
}