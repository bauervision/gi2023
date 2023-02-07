using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Galo
{
    public class Halo : MonoBehaviour
    {
        public static Halo instance;
        public int haloActivationTime = 1;//time when halo becomes active
        public int paddleStartTime = 0;
        public int paddleActivationTime = 1;// additional time when paddle gets triggered

        public bool haloTriggered;
        public UnityEvent onShowHalo = new UnityEvent();
        public UnityEvent onTriggerHalo = new UnityEvent();

        public bool paddleTriggered;
        public UnityEvent onShowPaddle = new UnityEvent();
        public UnityEvent onTriggerPaddle = new UnityEvent();

        GameObject[] bloodPoints;
        GameObject[] paddlePoints;

        GameObject currentBloodPoint, currentPaddlePoint;

        private void Awake() { instance = this; }

        private void Start()
        {
            bloodPoints = GameObject.FindGameObjectsWithTag("HaloPoint");
            paddlePoints = GameObject.FindGameObjectsWithTag("PaddlePoint");

            InitHaloObjects(bloodPoints, out currentBloodPoint);
            InitHaloObjects(paddlePoints, out currentPaddlePoint);

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


        private void Update()
        {
            if (PuzzleTimer.instance.currentLevelTime.minutes == haloActivationTime && !haloTriggered)
            {
                haloTriggered = true;
                // turn on the particles
                onShowHalo.Invoke();
                // play the sound
                AudioManager.instance.PlayHaloSound();
            }

            // make sure we've set paddle start time first
            if (paddleStartTime != 0)
            {
                if (PuzzleTimer.instance.currentLevelTime.minutes == (paddleStartTime + paddleActivationTime) && !paddleTriggered)
                {
                    paddleTriggered = true;
                    ToggleHaloLights(paddlePoints, currentPaddlePoint, false);
                    // turn on the paddle halo
                    onShowPaddle.Invoke();
                    // play the sound
                    AudioManager.instance.PlayHaloSound();
                }
            }
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                if (haloTriggered && !paddleTriggered)
                {
                    // turn on the lights
                    ToggleHaloLights(bloodPoints, currentBloodPoint, true);
                    AudioManager.instance.PlayHaloPointsOnSound();
                    onTriggerHalo.Invoke();
                }

                if (paddleTriggered)
                {
                    // turn on the lights
                    ToggleHaloLights(paddlePoints, currentPaddlePoint, true);
                    AudioManager.instance.PlayHaloPointsOnSound();
                    onTriggerPaddle.Invoke();
                }
            }

        }

        public void LocatededBlood()
        {
            ToggleHaloLights(bloodPoints, currentBloodPoint, false);
        }

        public void GotBloodToHeart()
        {
            paddleStartTime = PuzzleTimer.instance.currentLevelTime.minutes;
            print("Located Island Blood.......paddleStartTime: " + paddleStartTime);
        }

        public void LocatedPaddle()
        {
            // get the current minutes and set as the start time

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
                }

            // make sure we show the current item when we're done
            MeshRenderer currMR = currentItem.GetComponent<MeshRenderer>();
            if (currMR != null)
                currMR.enabled = true;
            else
                currentItem.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;

        }

    }
}
