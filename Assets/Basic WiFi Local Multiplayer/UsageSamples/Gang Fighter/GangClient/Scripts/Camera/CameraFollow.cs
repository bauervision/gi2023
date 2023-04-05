using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform target;
	[SerializeField] float smoothing = 5f;
	[SerializeField] Transform maxPointcameraY;
	[SerializeField] Transform minPointCameraY;
	[SerializeField] Transform maxPointcameraZ;
	[SerializeField] Transform minPointCameraZ;
	[SerializeField] Transform maxPointcameraX;
	[SerializeField] Transform minPointCameraX;
	Transform inicialCamTransform;

	[HideInInspector]
	Vector3 targetCamPos;
	[SerializeField]Vector3 offset;

	[HideInInspector]
	 Quaternion defaultrotation;

	[HideInInspector]
	public bool changeAngle;
	private Quaternion newRot;
	public float defaultFieldOfView;

	 float x;

	 float y;

	 float z;
	 float yRot;


	
	void Start()
	{
		defaultrotation = transform.rotation;
		newRot = Quaternion.Euler (0f, yRot, 0f);
		defaultFieldOfView = GetComponent<Camera>().fieldOfView;
		if (target) {
			offset = new Vector3 (transform.position.x - target.position.x, transform.position.y - target.position.y, transform.position.z - target.position.z);
		}
	}
	
     private void LateUpdate()
	{
		
		if (target) {
			
			if (target.position.z + offset.z <= maxPointcameraZ.position.z && target.position.z + offset.z >= minPointCameraZ.position.z)
			{
				z = target.position.z + offset.z;
			} 
			else
			{
				z = transform.position.z;
			}
			if(target.position.y + offset.y <= maxPointcameraY.position.y && target.position.y + offset.y >= minPointCameraY.position.y)
			{
				y = target.position.y + offset.y;
			} 
			else
			{
				y = transform.position.y;
			}
			if(target.position.x + offset.x <= maxPointcameraX.position.x && target.position.x + offset.x >= minPointCameraX.position.x)
			 {
					x = target.position.x + offset.x;
			  } 
			  else
			  {
					x = transform.position.x;
			  }
				targetCamPos = new Vector3 (x, y, z);
				transform.position = Vector3.Lerp (transform.position, targetCamPos, smoothing * Time.deltaTime);

		}

		if (changeAngle) 
		{
			transform.Rotate (new Vector3 (3f, 0f, 0f));
		
			changeAngle = false;
		}
	}

	public void SetTarget(Transform _target)
	{
		target = _target;
		
	
	}

	public void ChangePerspective()
	{
		changeAngle = true;
		GetComponent<Camera> ().fieldOfView = 70f;
	}
	public void SetPointsPositions(Transform minPX,Transform  maxPX,Transform  minPY,
	                          Transform  maxPY,Transform  minPZ,Transform  maxPZ,Transform  inicialCamPos)
	{
		
		maxPointcameraY=maxPY;
		minPointCameraY=minPY;
		maxPointcameraZ=maxPZ;
		minPointCameraZ=minPZ;
		maxPointcameraX=maxPX;
		minPointCameraX=minPX;
		inicialCamTransform =  inicialCamPos;
	

	}


	
	public void ResetCamera()
	{
		changeAngle = false;
		transform.rotation = defaultrotation;
		GetComponent<Camera> ().fieldOfView = defaultFieldOfView;
	}

}
