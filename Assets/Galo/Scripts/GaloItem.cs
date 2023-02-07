using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Events;
namespace Galo
{
    public enum ItemType { IslandHeart, IslandBlood, Crystal, Coin, Gem, Oar, Boat, Chest, Potion, Jug, Halo, Shroom, };


    [RequireComponent(typeof(SphereCollider))]
    public class GaloItem : MonoBehaviour
    {

        public ItemType myType;
        public bool conditionMet;
        public bool destroyWhenCollected = true;
        public UnityEvent ConditionNotMet = new UnityEvent();
        public UnityEvent ConditionMet = new UnityEvent();
        UnityEvent onTriggerEnter = new UnityEvent();
        public UnityEvent Clicked = new UnityEvent();

        void Start()
        {
            GetComponent<SphereCollider>().isTrigger = true;// in case we forget to
        }


        private void OnMouseDown() { Clicked.Invoke(); }

        public void HasMetCondition() { conditionMet = true; }

        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {

                if (!conditionMet)
                {
                    // condition not yet met, but we arrived with the blood in hand at the heart
                    if (LevelManager.instance.foundBlood && myType == ItemType.IslandHeart)
                    {
                        AudioManager.instance.PlayCollectionSound(myType);
                        ConditionMet.Invoke();
                        HasMetCondition();
                        Halo.instance.GotBloodToHeart();
                    }
                    else
                        ConditionNotMet.Invoke();
                }
                else
                {
                    // if we found the blood
                    if (myType == ItemType.IslandBlood)
                    {
                        // turn off any halos if they have been triggered
                        if (Halo.instance.haloTriggered)
                            Halo.instance.LocatededBlood();
                    }

                    ConditionMet.Invoke();
                    AudioManager.instance.PlayCollectionSound(myType);
                    EnableThisObject(false);

                    if (destroyWhenCollected)
                        Destroy(this.gameObject);
                }

            }
        }

        private void EnableThisObject(bool state)
        {
            // disable the collider on this object right away
            gameObject.GetComponent<SphereCollider>().enabled = state;
        }
    }
}