using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

namespace Galo
{
    [DefaultExecutionOrder(-2)]
    public class DataManager : MonoBehaviour
    {
        public static DataManager instance;

        public AllGaloPlayers allPlayers;
        public GaloPlayerData playerData;

        public GaloCollectible latestCollectible;
        public GameObject[] currentTribe;

        public bool playerHasTribe;
        public string CurrentLevelToLoad;
        private void Awake()
        {
            if (instance != null && instance != this)
                Destroy(this);
            else
                instance = this;
        }

        private void Start()
        {

            // make persistent
            DontDestroyOnLoad(instance);

            //print(Application.persistentDataPath);
            if (DataSaver.CheckFirstTimeData())
                LoadSavedData();
            else
                SaveNewUserData();


            GameObject osDisplay = GameObject.Find("OS Display");
            if (osDisplay != null)
            {

#if UNITY_ANDROID && !UNITY_EDITOR
                osDisplay.GetComponent<TextMeshProUGUI>().text = getSDKInt().ToString();
#endif

#if UNITY_EDITOR
                osDisplay.GetComponent<TextMeshProUGUI>().text = "Editor";
#endif
            }

        }

#if UNITY_ANDROID && !UNITY_EDITOR
        int getSDKInt()
        {

            using (var version = new AndroidJavaClass("android.os.Build$VERSION"))
            {
                return version.GetStatic<int>("SDK_INT");
            }

        }
#endif

        #region Core Data Access
        public IEnumerator WaitForData()
        {

            while (playerData == null)// when valid coordinates come in, this will definitely not be zero
            {
                yield return new WaitForSeconds(1);
            }
            // trigger to continue 

        }

        /// <summary> Called when the player gets to the save point </smmary>
        public void SaveCollectibleData()
        {
            // update the data
            if (latestCollectible != null)
                playerData.collection.Add(latestCollectible);

            allPlayers.savedPlayers[0] = allPlayers.lastUsedPlayer = playerData;
            //TODO: also save this data to allPlayers.savedPlayers

            DataSaver.SaveFile();
            latestCollectible = null;
            UIManager.instance.UpdateProfilePage();
        }

        public void WipeData()
        {
            DataSaver.ClearPlayerData();
        }

        public void SaveData()
        {
            allPlayers.savedPlayers[0] = allPlayers.lastUsedPlayer = playerData;
            DataSaver.SaveFile();
        }

        ///<summary>We found existing save data on disc, so handle loading up the last used player account </summary>
        private void LoadSavedData()
        {

            allPlayers = Galo.DataSaver.LoadFile();
            if (allPlayers.savedPlayers.Count > 0)
            {
                if (allPlayers.lastUsedPlayer != null)
                    LoadReturningPlayer(allPlayers.lastUsedPlayer);
                else//load our first player
                    LoadReturningPlayer(allPlayers.savedPlayers.ToArray()[0]);
            }

        }


        public void SaveNewUserData()
        {
            playerData = new GaloPlayerData();
            playerHasTribe = false;

            // if this is our first save, create the list and add the new player
            if (allPlayers == null)
                allPlayers = new AllGaloPlayers(playerData);
            else
            {
                // otherwise just add the new player
                allPlayers.savedPlayers.Add(playerData);
            }

            allPlayers.lastUsedPlayer = playerData;
            DataSaver.SaveFile();
            // NotificationManager.instance.DisplayNotificationAutoHide("Welcome to Galo Islands!<br>This tutorial stage will introduce you to the world<br>Have fun and thanks for playing!", false, 4f);
        }

        #endregion

        #region Handle Tribe Related

        public void LoadReturningPlayer(GaloPlayerData player)
        {
            playerHasTribe = (player.tribe.playables != null && player.tribe.playables.Length > 0);
            playerData = allPlayers.lastUsedPlayer = player;

            // since we have a tribe already saved, let's set them
            if (playerHasTribe)
                if (TribeManager.instance)
                    currentTribe = TribeManager.instance.GetSavedTribe(player.tribe.playables);


        }

        public void SetTribe(GameObject[] newTribe)
        {
            // store right away for playing
            currentTribe = newTribe;
            // and update the players saved data for next time
            List<string> tribeList = new List<string>();
            foreach (GameObject person in newTribe)
                tribeList.Add(person.name);

            if (playerData.tribe == null)
                playerData.tribe = new();

            playerData.tribe.playables = tribeList.ToArray();
            allPlayers.lastUsedPlayer = playerData;
            DataSaver.SaveFile();
            playerHasTribe = true;
        }
        #endregion

        #region Handle Level Time
        public void SetBestLevelTime(int currentLevelIndex, LevelTime currentTime)
        {
            // do we have saved a time?
            if (playerData.availableLevels[currentLevelIndex].levelTime.minutes != -1)
            {
                int currentBestTimeMinutes = playerData.availableLevels[currentLevelIndex].levelTime.minutes;
                int thisBestTimeMinutes = PuzzleTimer.instance.GetFinalMinutes();

                //is our new minutes lower than current?
                if (thisBestTimeMinutes < currentBestTimeMinutes)
                {
                    //we beat our record
                    playerData.availableLevels[currentLevelIndex].levelTime = currentTime;
                }
                else if (thisBestTimeMinutes == currentBestTimeMinutes)// current and best have the same minutes
                {
                    // then we need to compare seconds
                    int currentBestTimeSeconds = playerData.availableLevels[currentLevelIndex].levelTime.seconds;
                    int thisBestTimeSeconds = PuzzleTimer.instance.GetFinalSeconds();

                    // current level had better time so update the record
                    if (thisBestTimeSeconds < currentBestTimeSeconds)
                    {
                        playerData.availableLevels[currentLevelIndex].levelTime = currentTime;
                    }
                }
                else // we dont have a best time for this level
                {
                    playerData.availableLevels[currentLevelIndex].levelTime = currentTime;
                }

            }
            else// we dont have a saved time yet, so save the first one
            {
                playerData.availableLevels[currentLevelIndex].levelTime = currentTime;
            }


            SaveData();

        }

        public LevelTime GetBestLevelTime(int currentLevelIndex)
        {

            // do we have an actual saved record?
            bool hasBestTime = CheckBestTime(currentLevelIndex);

            return hasBestTime ? playerData.availableLevels[currentLevelIndex].levelTime : null;
        }

        bool CheckBestTime(int currentLevelIndex)
        {
            if (playerData.availableLevels[currentLevelIndex] != null)
            {
                if (playerData.availableLevels[currentLevelIndex].levelTime.minutes != -1)
                {
                    //we dont have a negative 1, but we need to make sure we dont have 0:00 too
                    return playerData.availableLevels[currentLevelIndex].levelTime.minutes != 0 && playerData.availableLevels[currentLevelIndex].levelTime.seconds != 0;
                }
                else return false;
            }

            else return false;
        }
        #endregion




    }
}