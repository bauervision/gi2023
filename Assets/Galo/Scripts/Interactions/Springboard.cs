using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Galo
{
    public class Springboard : MonoBehaviour
    {

        public Vector3 impulse = new Vector3(0f, 220.0f, 0.0f);

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                other.gameObject.GetComponent<Rigidbody>().AddRelativeForce(impulse, ForceMode.Impulse);
                AudioManager.instance.PlayShroomBounce();
            }
        }
    }
}
