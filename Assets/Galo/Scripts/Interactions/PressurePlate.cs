using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Galo
{
    public class PressurePlate : MonoBehaviour
    {

        public bool conditionMet;

        public float downPosition;
        public GameObject affectedObject;

        public UnityEvent ConditionNotMet = new UnityEvent();
        public UnityEvent ConditionMet = new UnityEvent();

        bool shownIntitially;

        public void HasMetCondition() { conditionMet = true; }

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (!conditionMet && !shownIntitially)
                {
                    ConditionNotMet.Invoke();
                    shownIntitially = true;
                    return;
                }
                else
                    StartCoroutine(SmoothLerpDown());
            }
        }

        // void OnTriggerStay(Collider other)
        // {
        //     if (other.CompareTag("Player") && !hasTriggered)
        //     {
        //         downMultiplier = 1;
        //     }
        // }


        public float speed = 1f;
        private void OnTriggerExit(Collider other) { if (conditionMet) StartCoroutine(SmoothLerpUp()); }

        private IEnumerator SmoothLerpDown()
        {

            Vector3 startingPos = new Vector3(0, 0, 0);
            Vector3 finalPos = new Vector3(0, downPosition, 0);
            float elapsedTime = 0;

            while (affectedObject.transform.localPosition.y > downPosition)
            {
                elapsedTime += speed * Time.deltaTime;
                affectedObject.transform.localPosition = Vector3.MoveTowards(startingPos, finalPos, elapsedTime);
                yield return null;
            }

            ConditionMet.Invoke();
            shownIntitially = true;

        }

        private IEnumerator SmoothLerpUp()
        {

            Vector3 startingPos = affectedObject.transform.localPosition;
            Vector3 finalPos = new Vector3(0, 0, 0);
            float elapsedTime = 0;

            while (affectedObject.transform.localPosition.y < 0)
            {
                elapsedTime += speed * Time.deltaTime;
                affectedObject.transform.localPosition = Vector3.MoveTowards(startingPos, finalPos, elapsedTime);
                yield return null;
            }


        }

        private void FixedUpdate()
        {

        }
    }
}
