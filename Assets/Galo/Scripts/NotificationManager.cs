using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Galo
{
    public class NotificationManager : MonoBehaviour
    {
        public static NotificationManager instance;
        public GameObject notificationPanel;
        public TextMeshProUGUI notificationText;
        public GameObject[] UICollectibles;
        public GameObject performanceBonusImage;


        void Awake() { instance = this; }

        private void Start()
        {
            HideAllUIImages();
        }

        public void DisplayNotification(string message)
        {
            notificationText.text = message;
            AnimationManager.instance.NotificationsOn(true);
        }

        public void DisplayNotification(string message, int indexToShow)
        {
            notificationText.text = message;
            AnimationManager.instance.NotificationsOn(true);
            UICollectibles[indexToShow].SetActive(true);

        }

        public void DisplayNotificationAutoHide(string message)
        {
            notificationText.text = message;
            AnimationManager.instance.NotificationsOn(true);
            StartCoroutine(WaitAndThenHide(true));

        }

        public void DisplayNotificationAutoHide(string message, bool playSound)
        {
            notificationText.text = message;
            AnimationManager.instance.NotificationsOn(playSound);
            StartCoroutine(WaitAndThenHide(playSound));
        }
        public void DisplayNotificationAutoHide(string message, bool playSound, float secondsToWait)
        {
            notificationText.text = message;
            AnimationManager.instance.NotificationsOn(playSound);
            StartCoroutine(WaitAndThenHide(playSound));
        }

        public void DisplayNotificationAutoHide(string message, float secs)
        {
            notificationText.text = message;
            AnimationManager.instance.NotificationsOn(true);
            StartCoroutine(WaitAndThenHide(secs));
        }

        public void HideAllUIImages()
        {
            foreach (GameObject ui in UICollectibles)
                ui.SetActive(false);
        }

        public IEnumerator WaitAndThenHide(bool playSound)
        {
            yield return new WaitForSeconds(2.5f);
            AnimationManager.instance.NotificationsOff(playSound);

            performanceBonusImage.SetActive(false);


        }

        public IEnumerator WaitAndThenHide(bool playSound, float seconds)
        {
            yield return new WaitForSeconds(seconds);
            AnimationManager.instance.NotificationsOff(playSound);

            performanceBonusImage.SetActive(false);
        }

        public IEnumerator WaitAndThenHide(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            AnimationManager.instance.NotificationsOff(true);

            performanceBonusImage.SetActive(false);
        }
    }
}