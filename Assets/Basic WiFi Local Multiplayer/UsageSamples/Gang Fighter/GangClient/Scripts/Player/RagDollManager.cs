using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RagDollManager : MonoBehaviour
{
   
   public GameObject hips;

   public Transform cameraToTarget;
   	public int character_index;

	public GameObject model;

	public Material[] materials;

	

    void Awake()
    {
        Rigidbody[] rig = GetComponentsInChildren<Rigidbody> ();

		foreach(Rigidbody r in rig)
		{
			r.useGravity = true;
			r.isKinematic= false;
		}
       
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if(GameManager.instance.isGameOver)
        {
            DestroyRagDoll();
        }

        */
  
        
    }

    //method used in the NetworkManager class to define the skin of the character chosen by the localPlayer
	public void SetUpCharacter(int _character_index)
	{
	   character_index = _character_index;
	   SkinnedMeshRenderer[] skinRends = model.GetComponentsInChildren<SkinnedMeshRenderer> ();
		
		  foreach(SkinnedMeshRenderer smr in skinRends)
		  {
		    smr.material = materials[character_index];
		  }
		 		
	}

 

    public void DestroyRagDoll()
    {
       Destroy(gameObject);

    }
}

