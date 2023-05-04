using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;


namespace Galo
{
    public class LoadStartMenu : MonoBehaviour
    {
        public AssetReference startMenuScene;
        AsyncOperationHandle m_SceneHandle;

        public Slider progressBar;
        public TextMeshProUGUI downloadedBytesText;
        public TextMeshProUGUI totalBytesText;


        // Start is called before the first frame update
        void Start()
        {
            BeginDownload();
        }

        private void Update()
        {

            if (m_SceneHandle.IsValid())
            {
                progressBar.value = m_SceneHandle.GetDownloadStatus().Percent;
                downloadedBytesText.text = m_SceneHandle.GetDownloadStatus().DownloadedBytes.ToString();
                totalBytesText.text = m_SceneHandle.GetDownloadStatus().TotalBytes.ToString() + " bytes";
            }

        }

        public void BeginDownload()
        {
            m_SceneHandle = Addressables.DownloadDependenciesAsync(startMenuScene);
            m_SceneHandle.Completed += OnSceneLoaded;
        }

        private void OnSceneLoaded(AsyncOperationHandle obj)
        {
            // We show the UI button once the scene is successfully downloaded      
            if (obj.Status == AsyncOperationStatus.Succeeded)
            {
                Addressables.LoadSceneAsync(startMenuScene, LoadSceneMode.Single);
            }
            else if (obj.Status == AsyncOperationStatus.Failed)
            {
                downloadedBytesText.text = obj.Status.ToString();
                totalBytesText.text = obj.OperationException.Message;
            }

        }

    }
}
