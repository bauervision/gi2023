using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

namespace Galo
{
    public class AddressableManager : MonoBehaviour
    {

        public AssetReference remoteScene;

        public Slider progressBar;

        public UnityEvent onStartAssetsLoading = new UnityEvent();
        public UnityEvent onAssetsDone = new UnityEvent();

        public UnityEvent onStartSceneLoading = new UnityEvent();
        public UnityEvent onSceneDone = new UnityEvent();


        AsyncOperationHandle downloadDependencies;

        bool startedDownload;
        private void OnEnable()
        {
            onStartAssetsLoading.Invoke();
            downloadDependencies = Addressables.DownloadDependenciesAsync("CloudAsset");
            downloadDependencies.Completed += DownloadDependenciesCompleted;

        }

        private void Update()
        {
            progressBar.value = downloadDependencies.PercentComplete;
        }


        private void DownloadDependenciesCompleted(AsyncOperationHandle obj)
        {
            if (obj.Status == AsyncOperationStatus.Succeeded)
            {
                onAssetsDone.Invoke();
            }
            else
                Debug.LogError("Loading Scene Failed");
        }

        public IEnumerator DownloadDependencies()
        {
            string key = "CloudAsset";
            // Clear all cached AssetBundles
            // WARNING: This will cause all asset bundles to be re-downloaded at startup every time and should not be used in a production game
            // Addressables.ClearDependencyCacheAsync(key);

            //Check the download size
            AsyncOperationHandle<long> getDownloadSize = Addressables.GetDownloadSizeAsync(key);
            yield return getDownloadSize;

            GameObject.Find("Loading Text").GetComponent<TextMeshProUGUI>().text = $"Downloading Asset dependencies...{getDownloadSize}";
            //If the download size is greater than 0, download all the dependencies.
            if (getDownloadSize.Result > 0)
            {
                downloadDependencies = Addressables.DownloadDependenciesAsync(key);
                yield return downloadDependencies;
            }


        }

    }
}
