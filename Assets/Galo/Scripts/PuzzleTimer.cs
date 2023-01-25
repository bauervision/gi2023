using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace Galo
{
    public class PuzzleTimer : MonoBehaviour
    {
        public static PuzzleTimer instance;
        private TextMeshProUGUI timerText;
        private float startTime;
        private float endTime;
        public bool finished = false;

        public LevelTime currentLevelTime;

        private void Awake() { instance = this; timerText = this.GetComponent<TextMeshProUGUI>(); }

        private void Start()
        {
            StartTimer();
        }

        private void Update()
        {
            if (finished)
                return;

            instance.timerText.text = ProcessLevelTime();
        }
        public static void StartTimer()
        {
            instance.finished = false;
            instance.startTime = Time.time;
            instance.currentLevelTime = new LevelTime();
        }

        string ProcessLevelTime()
        {
            float t = Time.time - instance.startTime;
            instance.currentLevelTime.minutes = Mathf.FloorToInt(t / 60);
            instance.currentLevelTime.seconds = Mathf.FloorToInt(t % 60);
            return string.Format("{0:00}:{1:00}", instance.currentLevelTime.minutes, instance.currentLevelTime.seconds);
        }

        public static void StopTimer()
        {
            instance.finished = true;
            instance.endTime = Time.time;
            DataManager.instance.SetBestLevelTime((int)Initializer.instance.currentLevelName, instance.currentLevelTime);
        }


        public string GetFinalTime()
        {
            return string.Format("{0:00}:{1:00}", instance.currentLevelTime.minutes, instance.currentLevelTime.seconds);
        }

        public LevelTime GetCurrentLevelTime() { return currentLevelTime; }
        public int GetFinalMinutes() { return instance.currentLevelTime.minutes; }
        public int GetFinalSeconds() { return instance.currentLevelTime.seconds; }
        public int GetFinalTimeScore() { return HandleTimeScoreCalculation(); }

        private int HandleTimeScoreCalculation()
        {

            switch (instance.currentLevelTime.minutes)
            {
                case 4: { return 100; }
                case 3: { return 250; }
                case 2: { return 500; }
                case 1: { return 750; }
                case 0: { return 1000; }
                default: { return 50; }
            }
        }

    }



}