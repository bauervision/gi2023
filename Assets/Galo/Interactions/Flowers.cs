using System.Collections;
using System.Collections.Generic;
using Invector.vCharacterController;
using UnityEngine;
using UnityEngine.Events;

namespace Galo.Interactions
{
    public enum FlowerType { POISON, HEALING }
    public class Flowers : MonoBehaviour
    {
        public FlowerType flowerType;
        public UnityEvent onTriggerEnter = new UnityEvent();

        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                other.GetComponent<vThirdPersonController>().alwaysWalkByDefault = flowerType == FlowerType.POISON;

                //AudioManager.instance.PlayCollectionSound(myType);
                onTriggerEnter.Invoke();

            }

        }
    }

}

