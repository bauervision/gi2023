using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Galo
{
    public enum CollectibleType { Jar, Horse, Bear, Ornament, Barrel, Bellows, Pulley, SoccerBall, LifePreserver, BarrelContainer };

    public class Collectible : MonoBehaviour
    {
        AudioSource _audioSource;
        public AudioClip _audioClip;

        private int[] collectiblePoints = new int[] { 1000, 2000, 3000, 4000, 5000, 10000, 11000, 13000, 150000, 20000 };


        public CollectibleType myCollectible;
        GaloCollectible _collectibleData;

        public int myUIIndex;// lines up with the array in notification manager

        public UnityEvent onCollect = new UnityEvent();


        private string[] collectibleText = new string[]{
    "You found a small jar, looks handmade. I wonder where the person who made it went?", // jar
    "A small, hand carved wooden horse!  What a rare find!",
    "A child's stuffed bear, this is incredibly rare to find, well done!",
    "A very old, delicately carved ornament of some kind, you are an excellent scavenger!",
    "Hmm...I believe this is a starfish, we have not seen any these in decades!"};

        private void Awake()
        {
            _collectibleData = new GaloCollectible(collectiblePoints[(int)myCollectible], myCollectible.ToString());
        }


        public bool hiddenFromStart = false;
        void Start()
        {
            // locate required items
            _audioSource = GetComponent<AudioSource>();

            //hiddenFromStart = alreadyFoundThis();
            //this.gameObject.SetActive(hiddenFromStart);
            //HandleCollectibles();
        }

        IEnumerator ValidateDataManager()
        {
            // we need to make sure that we hold off on pulling in data until we have valid GPS data
            yield return DataManager.instance.WaitForData();
            // now we are safe 

        }

        bool alreadyFoundThis()
        {
            if (DataManager.instance)
                return DataManager.instance.playerData.collection.FindIndex(i => string.Equals(i.name, _collectibleData.name)) != -1;

            return false;
        }

        // private void OnMouseDown()
        // {
        //     Clicked.Invoke();
        // }

        // void IPointerDownHandler.OnPointerDown(PointerEventData pointerEventData)
        // {
        //     HandleClickEvent();
        // }

        // public void HandleClickEvent()
        // {
        //     _audioSource.PlayOneShot(_audioClip);
        //     EnableThisObject(false);
        //     ExpManager.UpdateCollectible(_collectibleData);
        //     onCollect.Invoke();
        //     HandleRemoval();
        // }

        private void HandleCollectibles()
        {
            if (alreadyFoundThis())
                return;
            else
            {
                // if this is a collectible, determine when to unhide it based on the players ranking
                bool showCollectible = false;
                int playerRanking = DataManager.instance ? (int)DataManager.instance.playerData.rank : 0;


                switch (myCollectible)
                {
                    case CollectibleType.Bear://Spotter
                        {
                            showCollectible = playerRanking > 2;
                            break;
                        }
                    case CollectibleType.Barrel:// Finder
                        {
                            showCollectible = playerRanking > 5;
                            break;
                        }
                    case CollectibleType.Ornament://Gatherer
                        {
                            showCollectible = playerRanking > 8;
                            break;
                        }
                    default:
                        {
                            showCollectible = true;
                            break;
                        }
                }

                // if we determine that we need to show the model, do so only if it isnt already showing
                if (!gameObject.activeInHierarchy)
                    EnableThisObject(showCollectible);
            }

        }
        private void HandleMessageDisplay()
        {
            string message = collectibleText[(int)myCollectible];
            NotificationManager.instance.DisplayNotification(message, myUIIndex);
        }


        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                _audioSource.PlayOneShot(_audioClip);
                EnableThisObject(false);
                ExpManager.UpdateCollectible(_collectibleData);
                onCollect.Invoke();
                HandleRemoval();
            }
        }



        private void HandleRemoval()
        {
            HandleMessageDisplay();
            //Destroy(gameObject, _audioClip.length);
        }

        private void EnableThisObject(bool state)
        {
            // disable the collider on this object right away
            gameObject.GetComponent<SphereCollider>().enabled = state;

            if (transform.childCount > 0)
                transform.GetChild(0).GetComponent<MeshRenderer>().enabled = state;
            else
                gameObject.GetComponent<MeshRenderer>().enabled = state;


        }

        // private void Update()
        // {
        //     if (hiddenFromStart)
        //         return;
        //     HandleCollectibles();
        // }
    }
}
