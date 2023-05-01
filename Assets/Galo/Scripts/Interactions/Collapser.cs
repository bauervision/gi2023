using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Galo
{
    [RequireComponent(typeof(Rigidbody), typeof(BoxCollider))]
    public class Collapser : MonoBehaviour
    {

        public Rigidbody rb;

        public float timer;
        public bool hasImpulse;


        bool playerArrived;

        private void Update()
        {
            if (playerArrived)
                if (timer > 0)
                    StartCoroutine(CollapseTimer());
                else if (hasImpulse)
                {
                    Vector3 m_NewForce = new Vector3(0f, 20.0f, 0.0f);
                    //Rigidbody playerRB = Find

                }
                else
                    HandleCollapse();
        }


        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
                playerArrived = true;
        }

        IEnumerator CollapseTimer()
        {
            float counter = timer;
            while (counter > 0)
            {
                yield return new WaitForSeconds(1);
                counter--;
            }
            HandleCollapse();
        }

        void HandleCollapse()
        {
            rb.useGravity = true;
            rb.isKinematic = false;
        }
    }
}
