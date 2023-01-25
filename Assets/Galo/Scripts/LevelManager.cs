using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using TMPro;



namespace Galo
{
    public enum GaloLevelNames { Tutorial, IsleOfNoob, MountEgo, FrigidForest, Level4, Level5, Level6, Level7, Level8, Level9 };


    public class LevelManager : MonoBehaviour
    {
        public static LevelManager instance;
        public GaloLevel galoLevel;

        public bool foundBlood;
        public bool foundHeart;
        public int foundCrystal = 0;

        public TextMeshProUGUI starPointsText;
        public TextMeshProUGUI finalTimeText;
        public TextMeshProUGUI finalTimeScoreText;
        public TextMeshProUGUI totalScoreText;

        public UnityEvent onReadyForNextLevel = new UnityEvent();

        public bool isLevelComplete;// player has unlocked the level
        public bool playerHasTheOar;



        private void Awake() { instance = this; }



        public void LevelCompleted()
        {
            onReadyForNextLevel.Invoke();
            AudioManager.instance.PlayLevelEndSound();
            isLevelComplete = true;
            PuzzleTimer.StopTimer();
            CalculateFinalPoints();
        }

        public void CalculateFinalPoints()
        {
            int bonusCrystalPoints = 500 * foundCrystal;
            int levelPoints = 1000 + bonusCrystalPoints;

            starPointsText.text = levelPoints.ToString() + " pts!";

            finalTimeText.text = PuzzleTimer.instance.GetFinalTime();
            // store final time if it's lower than the current
            int timePoints = PuzzleTimer.instance.GetFinalTimeScore();
            finalTimeScoreText.text = timePoints + " pts!";
            ExpManager.UpdateXP(timePoints);
            totalScoreText.text = "Total: " + (levelPoints + timePoints).ToString() + " XP!";
            DataManager.instance.SaveData();
        }


        public void ReturnToLevelSelect() { SceneManager.LoadScene(1); }

        public void ReplayLevel() { SceneManager.LoadScene(2); }





    }
}
