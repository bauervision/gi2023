using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class p_StepOn : MonoBehaviour
{
    public float movementSpeed = 1.0f;
    public float downPosition;

    public GameObject affectedObject;

    private Animator anim;

    private bool isOn = false;

    private void Awake()
    {
        anim = affectedObject.GetComponent<Animator>();
    }
    private void Start()
    {
        if (anim != null)
            anim.enabled = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isOn = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        isOn = false;
    }



    void Update()
    {
        if (isOn)
        {
            // if we don't have an animation clip loaded, then do simple transform animation
            if (anim == null)
            {
                if (transform.localPosition.y > downPosition)
                {
                    // Move the object upward in world space 1 unit/second.
                    transform.Translate(Vector3.down * (Time.deltaTime * movementSpeed), Space.World);
                }
                else
                {
                    affectedObject.GetComponent<AnimatedObject>().isAnimating = true;

                }
                // TODO code up the other possible transforms
            }
            else// otherwise play the animation
            {
                anim.enabled = true;
            }
        }
    }
}
