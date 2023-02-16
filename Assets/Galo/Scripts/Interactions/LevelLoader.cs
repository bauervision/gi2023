using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

namespace Galo
{
    public class LevelLoader : MonoBehaviour
    {
        public static LevelLoader instance;


        GameObject continueButton, welcomeButton, updateButton;



        private void Awake()
        {
            if (instance != null && instance != this)
                Destroy(this);
            else instance = this;
        }

        private void Start()
        {
            continueButton = GameObject.Find("ContinueButton");
            welcomeButton = GameObject.Find("WelcomeButton");
            updateButton = GameObject.Find("UpdateButton");

            // player has a tribe? show continue and update buttons
            if (DataManager.instance)
            {
                if (DataManager.instance.playerHasTribe)
                {
                    if (continueButton != null)
                        continueButton.SetActive(true);

                    if (welcomeButton != null)
                        welcomeButton.SetActive(false);

                    if (updateButton != null)
                        updateButton.SetActive(true);
                }
                else // player doesnt yet have a tribe
                {
                    if (continueButton != null)
                        continueButton.SetActive(false);

                    if (welcomeButton != null)
                        welcomeButton.SetActive(true);

                    if (updateButton != null)
                        updateButton.SetActive(false);
                }
            }
        }


        public void LoadLevelSelect()
        {
            SceneManager.LoadScene("LevelSelect");
        }

        /// <summary>
        /// When we want to load into addressable levels, 
        /// </summary>
        /// <param name="level"></param>
        public void PlayLevel(string level)
        {
            if (DataManager.instance)
                DataManager.instance.CurrentLevelToLoad = level;

            Addressables.LoadSceneAsync(level);
        }




    }

}