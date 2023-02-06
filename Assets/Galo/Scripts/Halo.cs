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
        public int paddleActivationTime = 1;// additional time when paddle gets triggered

        public bool haloTriggered;
        public UnityEvent onShowHalo = new UnityEvent();
        public UnityEvent onTriggerHalo = new UnityEvent();

        public bool paddleTriggered;
        public UnityEvent onShowPaddle = new UnityEvent();
        public UnityEvent onTriggerPaddle = new UnityEvent();

        GameObject[] haloPoints;
        GameObject[] paddlePoints;

        private void Awake() { instance = this; }

        private void Start()
        {
            haloPoints = GameObject.FindGameObjectsWithTag("HaloPoint");
            paddlePoints = GameObject.FindGameObjectsWithTag("PaddlePoint");
            // turn them all off
            ToggleHalo(false);
            TogglePaddle(false);

            print("Halos found: " + haloPoints.Length);
            print("Paddles found: " + paddlePoints.Length);

            // now that we have all the halos randomly turn one on 
            if (haloPoints.Length > 1)
            {
                //haloPoints[]
            }
            else//otherwise just turn on the one we have
                haloPoints[0].GetComponentInChildren<Light>().enabled = true;

            // now that we have all the paddles, randomly turn on one
            if (paddlePoints.Length > 1)
            {
                //paddlePoints[]
            }
            else
                paddlePoints[0].GetComponentInChildren<Light>().enabled = true;
        }

        private void Update()
        {
            if (PuzzleTimer.instance.currentLevelTime.minutes == haloActivationTime && !haloTriggered)
            {
                print(PuzzleTimer.instance.currentLevelTime.minutes);
                //AudioManager.instance.PlayCollectionSound(myType);
                haloTriggered = true;
                // turn on the particles
                onShowHalo.Invoke();
                // play the sound
                AudioManager.instance.PlayHaloSound();
            }

            if (PuzzleTimer.instance.currentLevelTime.minutes == (paddleActivationTime + paddleActivationTime) && !paddleTriggered)
            {
                //AudioManager.instance.PlayCollectionSound(myType);
                paddleTriggered = true;
                // turn on the paddle halo
                onShowPaddle.Invoke();
            }
        }

        void OnTriggerEnter(Collider other)
        {


            if (other.gameObject.tag == "Player")
            {
                if (haloTriggered && !paddleTriggered)
                {
                    // turn on the lights
                    ToggleHalo(true);
                    AudioManager.instance.PlayHaloPointsOnSound();
                    onTriggerHalo.Invoke();
                }

                if (paddleTriggered)
                {
                    // turn on the lights
                    TogglePaddle(true);
                    AudioManager.instance.PlayHaloPointsOnSound();
                    onTriggerPaddle.Invoke();
                }
            }

        }

        public void LocatedBlood()
        {
            ToggleHalo(false);
            // start timer for paddle...
        }

        public void LocatedPaddle()
        {
            TogglePaddle(false);
            // start timer for paddle...
        }

        public void ToggleHalo(bool showValue)
        {
            if (haloPoints.Length > 0)
                foreach (GameObject dropzone in haloPoints)
                    dropzone.GetComponentInChildren<Light>().enabled = showValue;

        }

        public void TogglePaddle(bool showValue)
        {
            if (paddlePoints.Length > 0)
                foreach (GameObject dropzone in paddlePoints)
                    dropzone.GetComponentInChildren<Light>().enabled = showValue;

        }
    }
}
