using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Collections;
public class MusicPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource CameraAudioSource = null;
    [SerializeField] private AssetReference AudioRef = null;


    private void Update()
    {
        //if (HandleFirebase.instance.userSignedIn && CameraAudioSource.isPlaying == false)
        HandleAddressablesAudio();
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