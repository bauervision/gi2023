using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using System.Text.RegularExpressions;

namespace Galo
{
    public class LevelSelector : MonoBehaviour
    {


        public string[] levelNames;

        public Button[] LevelButtons;

        public GameObject Row4;

        public Sprite foundSprite;//used for the stars
        public Sprite defaultSprite;

        public int CurrentLevelIndex;

        // Start is called before the first frame update
        void Start()
        {
            SetAvailableLevels();

        }


        public void LoadLevel(string levelName) { LevelLoader.instance.PlayLevel(levelName); }


        public void UnlockNextLevel()
        {
            print("Level unlocked!" + levelNames[CurrentLevelIndex + 1]);

        }



        public void SetAvailableLevels()
        {
            if (DataManager.instance.playerData != null)
            {
                List<GaloLevel> currentAvailable = DataManager.instance.playerData.availableLevels;

                for (int i = 0; i < LevelButtons.Length; i++)
                {
                    // set the availability of the level button
                    LevelButtons[i].interactable = currentAvailable[i].available;
                    // set the level name of the button
                    LevelButtons[i].transform.GetChild(1).transform.GetChild(0).GetComponent<Text>().text = FromCamelCase(currentAvailable[i].name);
                    // set the stars, by grabbing the "stars" gameobject
                    GameObject starsObj = LevelButtons[i].transform.GetChild(1).transform.GetChild(1).gameObject;
                    // now run through each of its children, the stars, and set their state
                    starsObj.transform.GetChild(0).GetComponent<Image>().sprite = currentAvailable[i].hasCompleted ? foundSprite : defaultSprite;
                    starsObj.transform.GetChild(1).GetComponent<Image>().sprite = currentAvailable[i].foundCrystal1 ? foundSprite : defaultSprite;
                    starsObj.transform.GetChild(2).GetComponent<Image>().sprite = currentAvailable[i].foundCrystal2 ? foundSprite : defaultSprite;

                    // now set the best time
                    bool hasRecord = (currentAvailable[i].levelTime.minutes != -1);
                    string bestTime = hasRecord ? string.Format("{0:00}:{1:00}", currentAvailable[i].levelTime.minutes, currentAvailable[i].levelTime.seconds) : "00:00";
                    LevelButtons[i].transform.GetChild(1).transform.GetChild(2).GetComponent<Text>().text = bestTime;
                    LevelButtons[i].transform.GetChild(1).transform.GetChild(2).GetComponent<Text>().color = hasRecord ? Color.white : Color.black;
                }
            }

            // store all the level names for easier access
            levelNames = System.Enum.GetNames(typeof(GaloLevelNames));

            // TODO: unlock 4th row at some point
            Row4.SetActive(false);


        }

        public string FromCamelCase(string propertyName)
        {
            string returnValue = null;
            returnValue = propertyName;

            //Strip leading "_" character
            returnValue = Regex.Replace(returnValue, "^_", "").Trim();
            //Add a space between each lower case character and upper case character
            returnValue = Regex.Replace(returnValue, "([a-z])([A-Z])", "$1 $2").Trim();
            //Add a space between 2 upper case characters when the second one is followed by a lower space character
            returnValue = Regex.Replace(returnValue, "([A-Z])([A-Z][a-z])", "$1 $2").Trim();

            return returnValue;
        }
    }

}