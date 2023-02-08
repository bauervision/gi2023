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


        private void SceneLoadComplete(AsyncOperationHandle<UnityEngine.ResourceManagement.ResourceProviders.SceneInstance> obj)
        {
            if (obj.Status == AsyncOperationStatus.Succeeded)
            {

            }
        }

        public void LoadLevelSelect() { SceneManager.LoadScene(1); }

        public void PlayLevel(string level)
        {
            DataManager.instance.CurrentLevelToLoad = level;
            SceneManager.LoadScene(2);
        }




    }

}