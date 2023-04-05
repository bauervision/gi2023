using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Galo
{
    public class Halo : MonoBehaviour
    {
        public static Halo instance;

        public int bloodHaloWaitTime = 2;
        public int paddleHaloWaitTime = 2;


        public bool haloTriggered;
        public UnityEvent onShowHalo = new UnityEvent();
        public UnityEvent onTriggerHalo = new UnityEvent();

        public bool paddleTriggered;
        public UnityEvent onShowPaddle = new UnityEvent();
        public UnityEvent onTriggerPaddle = new UnityEvent();

        public InfoManager infoManager;

        GameObject[] bloodPoints;
        GameObject[] paddlePoints;

        GameObject currentBloodPoint, currentPaddlePoint;

        private void Awake() { instance = this; }

        Coroutine bloodTimer;

        bool foundBlood, foundPaddle;

        private void Start()
        {
            bloodPoints = GameObject.FindGameObjectsWithTag("HaloPoint");
            paddlePoints = GameObject.FindGameObjectsWithTag("PaddlePoint");

            InitHaloObjects(bloodPoints, out currentBloodPoint);
            InitHaloObjects(paddlePoints, out currentPaddlePoint);

            // fire off the first timer that waits to see if player has a hard time finding the blood
            bloodTimer = StartCoroutine(TriggerHaloEvent(() => DisplayBloodHalo(), bloodHaloWaitTime, foundBlood));

        }

        void InitHaloObjects(GameObject[] haloArray, out GameObject currentPoint)
        {
            if (haloArray.Length > 1)
            {
                // turn off all the lights
                foreach (GameObject blood in haloArray)
                {
                    blood.GetComponentInChildren<LensFlare>().enabled = false;
                    blood.SetActive(false);//hide them all
                }

                // set the current point
                int randHalo = Random.Range(0, haloArray.Length);
                currentPoint = haloArray[randHalo];
                currentPoint.SetActive(true);//unhide this one
            }
            else
            {
                // set the current point
                currentPoint = haloArray[0];
                currentPoint.GetComponentInChildren<LensFlare>().enabled = false;
            }
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                if (!haloTriggered)
                {
                    infoManager.HaloPlayerWaiting();
                }
                else
                {

                    if (haloTriggered && !paddleTriggered)
                    {
                        // turn on the lights and pass the player for checking
                        ToggleHaloLights(bloodPoints, currentBloodPoint, true, other.gameObject);
                        AudioManager.instance.PlayHaloPointsOnSound();
                        onTriggerHalo.Invoke();
                    }

                    if (paddleTriggered)
                    {
                        // turn on the lights and pass the player for checking
                        ToggleHaloLights(paddlePoints, currentPaddlePoint, true, other.gameObject);
                        AudioManager.instance.PlayHaloPointsOnSound();
                        onTriggerPaddle.Invoke();
                    }
                }

            }

        }


        void DisplayBloodHalo()
        {
            haloTriggered = true;
            // turn on the particles
            onShowHalo.Invoke();
            // play the sound
            AudioManager.instance.PlayHaloSound();
        }

        void DisplayPaddleHalo()
        {
            paddleTriggered = true;
            ToggleHaloLights(paddlePoints, currentPaddlePoint, false);
            // turn on the paddle halo
            onShowPaddle.Invoke();
            // play the sound
            AudioManager.instance.PlayHaloSound();
        }

        /// <summary>
        /// Called when player finds the island blood
        /// </summary>
        public void LocatedBlood()
        {
            foundBlood = true;
            StopCoroutine(bloodTimer);
            ToggleHaloLights(bloodPoints, currentBloodPoint, false);
        }

        /// <summary>
        /// Called when the player found the blood and returned to the heart
        /// </summary>
        public void GotBloodToHeart()
        {
            StartCoroutine(TriggerHaloEvent(() => DisplayPaddleHalo(), paddleHaloWaitTime, foundPaddle));
        }

        /// <summary>
        /// Called from the Paddle when it gets found
        /// </summary>
        public void LocatedPaddle()
        {
            foundPaddle = true;
            // Turn off the lights
            ToggleHaloLights(paddlePoints, currentPaddlePoint, false);
        }

        /// <summary>
        /// Halo will show all possible locations for an item
        /// </summary>
        /// <param name="items"></param>
        /// <param name="showValue"></param>
        public void ToggleHaloLights(GameObject[] items, GameObject currentItem, bool isShown)
        {
            if (items.Length > 0)
                foreach (GameObject item in items)
                {
                    //hide all the mesh renderers so we dont get to see false items
                    MeshRenderer mr = item.GetComponent<MeshRenderer>();
                    if (mr != null)
                        mr.enabled = false;
                    else// this is the blood, so the MR is the first child
                    {
                        item.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
                    }
                    // handle the flare
                    item.GetComponentInChildren<LensFlare>().enabled = isShown;

                    // if we are showing the flares
                    if (isShown)
                    {
                        //fire off the fade if this player isnt a FINDER

                    }
                }

            // make sure we show the current item when we're done
            MeshRenderer currMR = currentItem.GetComponent<MeshRenderer>();
            if (currMR != null)
                currMR.enabled = true;
            else
                currentItem.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;

        }

        public void ToggleHaloLights(GameObject[] items, GameObject currentItem, bool isShown, GameObject player)
        {
            if (items.Length > 0)
                foreach (GameObject item in items)
                {
                    //hide all the mesh renderers so we dont get to see false items
                    MeshRenderer mr = item.GetComponent<MeshRenderer>();
                    if (mr != null)
                        mr.enabled = false;
                    else// this is the blood, so the MR is the first child
                    {
                        item.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
                    }
                    // handle the flare
                    item.GetComponentInChildren<LensFlare>().enabled = isShown;


                }

            // make sure we show the current item when we're done
            MeshRenderer currMR = currentItem.GetComponent<MeshRenderer>();
            if (currMR != null)
                currMR.enabled = true;
            else
                currentItem.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;


            //fire off the fade if this player isnt a FINDER
            if (player.transform.GetChild(0).transform.GetChild(0).GetComponent<CharacterData>().playerType != PlayerType.FINDER)
                StartCoroutine(FadeOutHalo(items));


        }


        IEnumerator TriggerHaloEvent(UnityAction proceedingEvent, float timeToWait, bool monitoringBool)
        {
            yield return new WaitForSeconds(timeToWait);
            // only fire off if the player still hasnt found the requirement
            if (!monitoringBool)
                proceedingEvent.Invoke();
        }

        int flareFadeTime = 120;
        IEnumerator FadeOutHalo(GameObject[] items)
        {
            foreach (GameObject item in items)
            {
                // handle the flare fade out
                float elapsedTime = 0.0f;
                LensFlare flare = item.GetComponentInChildren<LensFlare>();

                while (elapsedTime < flareFadeTime)
                {
                    yield return null;
                    elapsedTime += Time.deltaTime;
                    flare.brightness = 1.0f - Mathf.Clamp01(elapsedTime / flareFadeTime);

                }

            }
        }
    }
}
