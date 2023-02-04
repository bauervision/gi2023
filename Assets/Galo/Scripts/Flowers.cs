using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Galo
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
                if (flowerType == FlowerType.POISON)
                {
                    print("Poisoned!");
                }
                else
                {
                    print("Healed!");
                }

                //AudioManager.instance.PlayCollectionSound(myType);
                onTriggerEnter.Invoke();

            }

        }
    }

}

