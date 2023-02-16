using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;


namespace Galo
{
    public class AddressableManager : MonoBehaviour
    {
        public static AddressableManager instance;
        public AssetReference remoteScene;

        public Slider progressBar;

        public UnityEvent onStartAssetsLoading = new UnityEvent();
        public UnityEvent onAssetsDone = new UnityEvent();

        public UnityEvent onStartSceneLoading = new UnityEvent();
        public UnityEvent onSceneDone = new UnityEvent();


        AsyncOperationHandle asyncSceneLoad;

        void Start()
        {
            //StartCoroutine(LoadRemoteAssets());
        }



        private void OnEnable()
        {
            asyncSceneLoad = Addressables.DownloadDependenciesAsync(remoteScene);
            asyncSceneLoad.Completed += SceneLoadComplete;
        }

        private void Update()
        {
            progressBar.value = asyncSceneLoad.PercentComplete;
        }

        IEnumerator LoadRemoteAssets()
        {
            onStartAssetsLoading.Invoke();
            yield return null;
            //Begin to load the Scene you specify
            AsyncOperationHandle asyncAssetLoad = Addressables.LoadResourceLocationsAsync("CloudAsset");

            //When the load is still in progress, output the Text and progress bar
            while (!asyncAssetLoad.IsDone)
            {
                //Output the current progress
                progressBar.value = asyncAssetLoad.PercentComplete;
                yield return null;
            }

            // Check if the load has finished
            if (asyncAssetLoad.IsDone)
            {
                asyncAssetLoad.Completed += AssetsLoadComplete;
            }

        }

        IEnumerator LoadScene()
        {
            onStartSceneLoading.Invoke();
            yield return null;

            //Begin to load the Scene you specify
            AsyncOperationHandle asyncSceneLoad = Addressables.LoadSceneAsync(remoteScene, LoadSceneMode.Single);

            //When the load is still in progress, output the Text and progress bar
            while (!asyncSceneLoad.IsDone)
            {
                //Output the current progress

                yield return null;
            }

            if (asyncSceneLoad.IsDone)
            {
                asyncSceneLoad.Completed += SceneLoadComplete;
            }
        }



        private void AssetsLoadComplete(AsyncOperationHandle obj)
        {
            if (obj.Status == AsyncOperationStatus.Succeeded)
            {
                onAssetsDone.Invoke();
                //fire off the next remote update
                //StartCoroutine(LoadScene());
            }
            else
                Debug.LogError("Loading Assets Failed");

        }

        private void SceneLoadComplete(AsyncOperationHandle obj)
        {
            if (obj.Status == AsyncOperationStatus.Succeeded)
            {
                onSceneDone.Invoke();
            }
            else
                Debug.LogError("Loading Scene Failed");

        }
    }
}
