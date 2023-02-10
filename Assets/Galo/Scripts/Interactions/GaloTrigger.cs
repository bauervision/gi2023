using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Galo
{
    [RequireComponent(typeof(BoxCollider))]
    public class GaloTrigger : MonoBehaviour
    {
        public UnityEvent onTriggerEnter = new UnityEvent();
        public UnityEvent onTriggerExit = new UnityEvent();
        public virtual void Start()
        {
            GetComponent<BoxCollider>().isTrigger = true;// in case we forget to
        }

        public virtual void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player") { onTriggerEnter.Invoke(); }
        }

        public virtual void OnTriggerExit(Collider other)
        {
            if (other.gameObject.tag == "Player") { onTriggerExit.Invoke(); }
        }


    }
}