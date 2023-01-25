using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AddressablesManager : MonoBehaviour
{
    public static AddressablesManager instance;
    public AssetReference SceneRef;
    public AssetReference AudioRef;

    public AudioSource CameraAudioSource;

    private void Start()
    {
        instance = this;
        //HandleAddressablesAudio();
    }
    public void HandleAddressablesScene()
    {
        SceneRef.LoadSceneAsync(UnityEngine.SceneManagement.LoadSceneMode.Single);
    }

    public void HandleAddressablesAudio()
    {
        AudioRef.LoadAssetAsync<AudioClip>().Completed += OnAudioLoaded;
    }

    void OnAudioLoaded(AsyncOperationHandle<AudioClip> handle)
    {
        CameraAudioSource.clip = handle.Result;
        CameraAudioSource.Play();
    }
}