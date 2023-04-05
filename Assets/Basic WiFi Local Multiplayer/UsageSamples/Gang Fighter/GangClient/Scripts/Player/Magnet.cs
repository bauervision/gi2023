using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Magnet : MonoBehaviour
{

    public LayerMask layerObj = 2;
	
	public bool fixedForce = false;
	
	public float ApproximationCoefficient = 1.5f;
	
	public float magnetForce;
	
	public float maxDist = 50;
	
	float objDist =0;
	float sqrtDist = 0;
	float sqrtDist_force = 0;
	
	private void Awake()
	{
	 // gameObject.layer = 1;
	}
 

   private void FixedUpdate()
   {
    Collider[] objectsInRayReach = Physics.OverlapSphere(transform.position,maxDist,layerObj);
	 foreach(Collider targetCollider in objectsInRayReach)
	 {
	 
	 
	   Transform magnetTarget = targetCollider.transform;
	   Rigidbody rbTemp = magnetTarget.GetComponent<Rigidbody>();
	   
	   if(rbTemp)
	   {
	    
	    Vector3 objectDirection = (transform.position - magnetTarget.position).normalized;
		 if(fixedForce)
		 {
		  sqrtDist = (ApproximationCoefficient*ApproximationCoefficient);
		 }
		 else
		 {
		  objDist = Vector3.Distance(transform.position,magnetTarget.position);
		   sqrtDist = Mathf.Pow(objDist,ApproximationCoefficient);
		 }
		 
		 sqrtDist_force = (magnetForce/sqrtDist)*100.0f;
		 sqrtDist_force = Mathf.Clamp(sqrtDist_force,0.01f,10000000000.0f);
		 rbTemp.AddForce(objectDirection*sqrtDist_force);
		 
		 Debug.DrawLine(transform.position,magnetTarget.position,Color.green);

		
	   }
	 }
   }
}
