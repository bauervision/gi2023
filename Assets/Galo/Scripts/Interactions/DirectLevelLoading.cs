using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Galo
{

    public class DirectLevelLoading : MonoBehaviour
    {
        public Slider progressBar;
        AsyncOperation loadingOperation;

        // Start is called before the first frame update
        void Start()
        {
            if (DataManager.instance.CurrentLevelToLoad != null)
            {
                StartCoroutine(LoadSceneAsync(DataManager.instance.CurrentLevelToLoad));
            }
            else
            {
                Debug.LogWarning("No scene set to load in DataManager!");
            }
        }

        IEnumerator LoadSceneAsync(string sceneName)
        {
            DataManager.instance.playerHasTribe = false;
            progressBar = GameObject.Find("SceneLoadProgress").GetComponent<Slider>();
            //Begin to load the Scene you specify
            loadingOperation = SceneManager.LoadSceneAsync(sceneName);
            //When the load is still in progress, output the Text and progress bar
            while (!loadingOperation.isDone)
            {
                //Output the current progress
                progressBar.value = Mathf.Clamp01(loadingOperation.progress / 0.9f);
                yield return null;
            }


        }


    }
}
