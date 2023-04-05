using UnityEngine;
using UnityEngine.UI;
using System.Collections;
namespace GangFighter{
public class PlayerHealth : MonoBehaviour
{
    	
	
	public Image damageImage;

	public float health = 100;

	public float maxHealth = 100;

	public float damageValue = 10;

	[Header("Health Slider")]
	public Slider healthSlider;

	public float flashSpeed = 5f;

	Color defaultColour;

	public Color flashColour = new Color(1f, 0f, 0f, 0.1f);

	ParticleSystem hitParticles;

	bool damaged;

    public AudioClip damageSound;

	public AudioClip deathSound;

	private AudioSource damageSoundSource;
	
	CapsuleCollider capsuleCollider;
	
	public bool isDead;

	float hitCount;

	public GameObject thirdPerson;

	public GameObject thirdPersonRagDollPref;
    
	[HideInInspector]
	public GameObject thirdPersonRagDoll ;

    public bool onRagdollAnim;
    public float ragdollAnimTime = 3;

    public Vector3 ragDollOffset;

    Vector3 tempPosition;

	bool isHit;

	public float hitTime;

    

	void Awake()
	{
		isDead = false;



		capsuleCollider = GetComponent <CapsuleCollider> ();

		hitParticles = GetComponentInChildren <ParticleSystem> ();

		healthSlider.GetComponent<Canvas>().enabled = false;
		healthSlider.maxValue = maxHealth;
	    healthSlider.value = maxHealth;

		if (GetComponent<PlayerController> ().isLocalPlayer && GameObject.Find ("DamageImage"))
		{
			damageImage = GameObject.Find ("DamageImage").GetComponent<Image> () as Image;
		}

	}

	
	// Update is called once per frame
	void Update () {


		if (GetComponent<PlayerController> ().isLocalPlayer) {
			if (damaged) {
			
			//	damageImage.color = flashColour;

			} else {
				
				//damageImage.color = Color.Lerp (damageImage.color, Color.clear, flashSpeed * Time.deltaTime);

			}

			damaged = false;
		}
		

	}

	public void TakeDamage ()
	{
		
		if (GetComponent<PlayerController> ().isLocalPlayer) {

			damaged = true;
			
			GetComponent<PlayerController> ().UpdateAnimator ("IsDamage");

			hitParticles.transform.position = transform.position + capsuleCollider.center;
			hitParticles.Play ();
			PlayDamageSound ();	

			

			if (health - damageValue > 0) {


				health = health - damageValue;
				CanvasManager.instance.healthSlider.value = health;
				CanvasManager.instance.txtHealth.text = "HP " + health+ " / " + maxHealth;
				Debug.Log("receive damage");


			} 
		}
		else
		{
			if (health - damageValue > 0)
			 {
				 health = health - damageValue;
			     healthSlider.value = health;
				 healthSlider.GetComponent<Canvas>().enabled = true;
				 Debug.Log("active canvas");
			 }


			hitParticles.transform.position = transform.position + capsuleCollider.center;
			hitParticles.Play ();
			GetComponent<PlayerController> ().UpdateAnimator ("IsDamage");

		
		}

		StartCoroutine ("WaitAnotherHits");

		hitCount+=1;
		if(hitCount >=3)
		{
			//Do Ragdoll
			StartCoroutine ("RagdollAnimation");
		}
	}
	
	 public void Death ()
    {
	   StartCoroutine ("DeathCutScene"); 
		
    }

	IEnumerator DeathCutScene()
	{ 
		isDead = true;
		health = 0;
		healthSlider.GetComponent<Canvas>().enabled = false;
	
	    DoRagDoll();
		yield return new WaitForSeconds(3f);

		NetworkManager.instance.GameOver ();
	}

	public void DoRagDoll()
	{
	    Debug.Log("do ragdoll");
		
		thirdPerson.SetActive(false);
				

		 thirdPersonRagDoll = GameObject.Instantiate (thirdPersonRagDollPref,
				               transform.position, thirdPersonRagDollPref.transform.rotation) as GameObject;

		thirdPersonRagDoll.GetComponent<RagDollManager>().SetUpCharacter(GetComponent<PlayerController>().character_index);

		
		
		NetworkManager.instance.camFollow.GetComponent<CameraFollow> ().SetTarget( thirdPersonRagDoll.transform);

         onRagdollAnim = true;

        	

	}


	IEnumerator WaitAnotherHits()
	{
		if (isHit)
		{
			yield break; 
		}

		isHit = true; 
		yield return new WaitForSeconds(hitTime); 

		hitCount =0;
	
		isHit = false;


	}

	/// <summary>
	/// Tries the atack.
	/// </summary>
	private IEnumerator RagdollAnimation()
	{
		

		if (onRagdollAnim)
		{
			yield break; // if already atack
		}

		healthSlider.GetComponent<Canvas>().enabled = false;
	
		thirdPerson.SetActive(false);
				

		 thirdPersonRagDoll = GameObject.Instantiate (thirdPersonRagDollPref,
				               transform.position, thirdPersonRagDollPref.transform.rotation) as GameObject;

		thirdPersonRagDoll.GetComponent<RagDollManager>().SetUpCharacter(GetComponent<PlayerController>().character_index);

		
		
		NetworkManager.instance.camFollow.GetComponent<CameraFollow> ().SetTarget( thirdPersonRagDoll.transform);

         onRagdollAnim = true;



		yield return new WaitForSeconds(ragdollAnimTime);
		
     
        tempPosition = thirdPersonRagDoll.GetComponent<RagDollManager>().hips.transform.position;

	    NetworkManager.instance.camFollow.GetComponent<CameraFollow> ().SetTarget(transform);


        gameObject.transform.position = new Vector3(tempPosition.x +ragDollOffset.x,tempPosition.y +ragDollOffset.y,tempPosition.z +ragDollOffset.z);
      
        thirdPersonRagDoll.GetComponent<RagDollManager>().DestroyRagDoll();
     
		thirdPerson.SetActive(true);
       
		onRagdollAnim = false;
		
	}

	

	
	//---------------AUDIO METHODS--------
	public void PlayDeathSound()
	{
	   if (!GetComponent<AudioSource> ().isPlaying)
	    {
			
		  GetComponent<AudioSource>().PlayOneShot(deathSound);

		}
		

	}
		
	public void PlayDamageSound()
	{

	   if (!GetComponent<AudioSource> ().isPlaying )
		{
		
		  GetComponent<AudioSource>().PlayOneShot(damageSound);

		}


	}
}//END_CLASS
}//END_NAMESPACE
