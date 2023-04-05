using UnityEngine;
using UnityEngine.Events;

namespace Galo
{
    [DefaultExecutionOrder(-1)]
    public class Initializer : MonoBehaviour
    {
        public static Initializer instance;
        public GameObject[] DeActivate;
        public GameObject[] Activate;
        public GaloLevelNames currentLevelName;
        public UnityEvent onSceneStart = new UnityEvent();

        private void Awake()
        {
            instance = this;

            if (DeActivate.Length > 0)
                foreach (GameObject d in DeActivate)
                    d.SetActive(false);

            if (Activate.Length > 0)
                foreach (GameObject a in Activate)
                    a.SetActive(true);
        }

        private void Start()
        {
            onSceneStart.Invoke();
            // get the best time for this level
            LevelTime bestTime = new LevelTime();
            if (DataManager.instance)
            {
                bestTime = DataManager.instance.GetBestLevelTime((int)currentLevelName);
                // make sure we know what to load when we want to replay
                DataManager.instance.CurrentLevelToLoad = currentLevelName.ToString();
            }
            else
            {
                bestTime.minutes = 1;
                bestTime.seconds = 0;
            }

            if (bestTime != null && NotificationManager.instance != null)
                NotificationManager.instance.DisplayNotificationAutoHide("Time to Beat... " + string.Format("{0:00}:{1:00}", bestTime.minutes, bestTime.seconds));
        }

    }
}