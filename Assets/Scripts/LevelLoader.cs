using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Collections.Generic;

public class LevelLoader : MonoBehaviour
{
    public static LevelLoader instance;
    public AssetReference AssetRefLevel1;

    //public AssetReference AssetRefLevel2;

    public Button Level_1;
    public Button Level_2;
    public Button Level_3;
    public Button Level_4;
    public Button Level_5;
    public Button Level_6;
    public Button Level_7;
    public Button Level_8;
    public Button Level_9;

    public Slider progressBar;
    public Sprite foundSprite;//used for the stars
    public Sprite defaultSprite;

    private Button[] buttonList;

    public int CurrentLevelIndex;
    public string[] levelNames = new string[] { "Isle of Noob", "Mount Ego", "Frigid Forest", "Level 4", "Level 5", "Level 6", "Level 7", "Level 8", "Level 9" };

    private void Awake()
    {

        // add all of our found level buttons to our array
        buttonList = new Button[] { Level_1, Level_2, Level_3, Level_4, Level_5, Level_6, Level_7, Level_8, Level_9 };
    }
    private void Start()
    {
        instance = this;
        Level_1.onClick.AddListener(PlayLevel1);
        // Level_2.onClick.AddListener(PlayLevel2);
        // Level_3.onClick.AddListener(PlayLevel3);
        // Level_4.onClick.AddListener(PlayLevel4);
        // Level_5.onClick.AddListener(PlayLevel5);
        // Level_6.onClick.AddListener(PlayLevel6);
        // Level_7.onClick.AddListener(PlayLevel7);
        // Level_8.onClick.AddListener(PlayLevel8);
        // Level_9.onClick.AddListener(PlayLevel9);
        // Level_10.onClick.AddListener(PlayLevel10);
    }


    public void SetAvailableLevels()
    {
        // if (DataManager.instance.playerData != null)
        // {
        //     var currentAvailable = DataManager.instance.playerData.availableLevels;

        //     for (int i = 0; i < instance.buttonList.Length; i++)
        //     {
        //         // set the availability of the level button
        //         instance.buttonList[i].interactable = currentAvailable[i].available;
        //         // set the level name of the button
        //         instance.buttonList[i].transform.GetChild(1).GetComponent<Text>().text = levelNames[i];
        //         // set the stars, by grabbing the "stars" gameobject
        //         GameObject starsObj = instance.buttonList[i].transform.GetChild(3).gameObject;
        //         // now run through each of its children, the stars, and set their state
        //         starsObj.transform.GetChild(0).GetComponent<Image>().sprite = currentAvailable[i].hasCompleted ? instance.foundSprite : instance.defaultSprite;
        //         starsObj.transform.GetChild(1).GetComponent<Image>().sprite = currentAvailable[i].foundCrystal1 ? instance.foundSprite : instance.defaultSprite;
        //         starsObj.transform.GetChild(2).GetComponent<Image>().sprite = currentAvailable[i].foundCrystal2 ? instance.foundSprite : instance.defaultSprite;

        //     }
        // }


    }
    public void UnlockNextLevel()
    {
        print("Level unlocked!" + levelNames[CurrentLevelIndex + 1]);

    }

    IEnumerator LoadScene(AssetReference addressableLevel)
    {
        //LoginManager.LoadLevel();//switch to level load screen
        yield return null;

        //Begin to load the Scene you specify
        AsyncOperationHandle asyncAssetLoad = Addressables.LoadSceneAsync(addressableLevel, LoadSceneMode.Single);//.Completed += SceneLoadComplete;

        //When the load is still in progress, output the Text and progress bar
        while (!asyncAssetLoad.IsDone)
        {
            //Output the current progress
            progressBar.value = asyncAssetLoad.PercentComplete;

            // Check if the load has finished
            if (asyncAssetLoad.PercentComplete >= 0.9f)
            {
                // hide the progress bar
                progressBar.gameObject.SetActive(false);
                //asyncAssetLoad.Completed += SceneLoadComplete;
            }

            //asyncOperation.allowSceneActivation = true;
            yield return null;
        }


    }

    private void SceneLoadComplete(AsyncOperationHandle<UnityEngine.ResourceManagement.ResourceProviders.SceneInstance> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {

        }
    }

    #region Play Level Buttons
    public void PlayLevel1()
    {
        // if (AssetRefLevel1 != null)
        //     StartCoroutine(LoadScene(AssetRefLevel1));
        // else
        SceneManager.LoadScene(levelNames[0]);
        //LoginManager.LoadLevel();
    }

    // public void PlayLevel2()
    // {
    //     StartCoroutine(LoadScene(AssetRefLevel1));
    // }
    // public void PlayLevel3()
    // {
    //     StartCoroutine(LoadScene(levelNames[2]));
    // }
    // public void PlayLevel4()
    // {
    //     StartCoroutine(LoadScene(levelNames[3]));
    // }
    // public void PlayLevel5()
    // {
    //     StartCoroutine(LoadScene(levelNames[4]));
    // }
    // public void PlayLevel6()
    // {
    //     StartCoroutine(LoadScene(levelNames[5]));
    // }
    // public void PlayLevel7()
    // {
    //     StartCoroutine(LoadScene(levelNames[6]));
    // }
    // public void PlayLevel8()
    // {
    //     StartCoroutine(LoadScene(levelNames[7]));
    // }
    // public void PlayLevel9()
    // {
    //     StartCoroutine(LoadScene(levelNames[8]));
    // }
    // public void PlayLevel10()
    // {
    //     StartCoroutine(LoadScene(levelNames[9]));
    // }

    #endregion


}