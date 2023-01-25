using UnityEngine;
using UnityEngine.UI;


public class InteractionManager : MonoBehaviour
{
    public static InteractionManager instance;
    public enum CurrentLevel { Tutorial, IsleOfNoob, MountEgo, FrigidForest, Level4, Level5, Level6, Level7, Level8, Level9 };
    public CurrentLevel thisLevel;

    public GameObject spawnItem;

    // Player abilities
    public bool canJump = true;
    public bool canSprint = true;
    public bool hasStamina = false;
    public bool speedReduced = false;
    public bool canDie = false;
    public bool isDevelopmentTest = false;

    public int currentSeedCount = 0;


    #region FindTheseItemsDuringAwake
    public Text SeedCountText;
    private GameObject testCharacter;
    private GameObject testController;
    private GameObject[] spawnLocations;
    private GameObject[] mysteryLocations;
    private GameObject GoodieBag;
    private GameObject GoodieBagResult;
    private Text GoodieBagText;
    private Text GoodieBagUIText;
    private GameObject GoodieBagStamina;
    private GameObject initialCanvas;
    private GameObject gameCanvas;
    private GameObject levelCanvas;
    private GameObject finalCanvas;
    private GameObject mobileCanvas;
    private GameObject bestTime;

    private Image mainItemSprite;
    private Image bonusItem1Sprite;
    private Image bonusItem2Sprite;
    private Image finalMainItemSprite;
    private Image finalBonusItem1Sprite;
    private Image finalBonusItem2Sprite;
    private Text GemText;
    private Text CoinText;
    private Text finalTimeText;
    private Text finalCoinText;
    private Text finalGemText;
    private Text finalDeductionText;
    private Text tallyTimeText;
    private Text tallyCollectibleText;
    private Text tallyItemText;
    private Text tallyDeductionText;
    private Text finalScoreText;
    private Text specialText;
    private Button LoadLevelButton;
    private Button LevelSelectButtonInitial;
    private Button LevelSelectButtonFinal;
    private Button LoadNextLevelButton;

    private Camera gameCamera;
    private Camera uiCamera;
    private GameObject gameController;
    private GameObject notifyPanel;
    private Text notifyText;
    private Text levelDescriptionText;
    private Text nextLevelName;
    private Text finalSpecialText;
    private Text bestTimeText;

    private GameObject finalSpecialPanel;
    private GameObject[] coinList;
    private GameObject[] gemList;

    private GameObject throwButton;

    #endregion

    public Sprite foundSprite;


    public static int finalMinutes = 0;
    public static int finalSeconds = 0;

    public static bool foundMountainBlood;


    private string[] goodieBag = new[] { "Bag O' Nothing", "Goggles", "Halo", "Radar", "Bad Knees", "Stamina", "Poisoned" };
    private int[] goodieBagDeductions = new[] { 0, -10, -100, -200, 50, 150, 500 };

    private string[] goodieBagDescription = new[] {
        "Bag O' Nothing!\nYou enter the level with all that you came with, no help or hindrances to speak of, good luck!",
        "Goggles!\nYou have gained the ability to see where the Mountain Blood can appear! Keep a mental note of all the spots, as it can help you later!",
        "Halo!\nDown from the heavens you will see beams of light pointing to every place the special items might be. Instead of randomly running around, you now know exactly where to look!",
        "Radar!\nLike the Halo, except you will only see one ray of light from the sky, directly where the item is!",
        "Bad Knees!\nOuch, you've injured your knees which means no jumping for you today!",
        "Stamina Meter installed!\nNot quite as young as you were, today you will have to deal with a lack of stamina, which means you can only sprint for so long before you will need to catch your breath.",
        "Poisoned!\nMovement speed has been reduced to half as you struggle to move around at all." };

    private string levelDeductionText = "None";
    private int levelDeduction = 0;
    public int levelBonusItemScore = 0;
    private int levelCoinCount = 0;
    public int levelGemCount = 0;
    private int levelTotalPoints = 0;
    private int chosenDropzoneIndex = -1;

    private int chosenMysteryIndex = -1;

    private Color OffColor = new Color(0, 0, 0, 0);
    private Color OnColor = new Color(0, 0, 0, 0.7f);

    private bool foundBonus1 = false;
    private bool foundBonus2 = false;

    private int specialPoints = 0;


    private PlayerData gamePlayer;
    private bool haloEnabled = false;

    private void Awake()
    {
        // handle testing
        if (isDevelopmentTest)
        {
            testCharacter = GameObject.Find("Miles");
            testCharacter.SetActive(isDevelopmentTest);
            testController = GameObject.Find("devGameController");
            testController.SetActive(isDevelopmentTest);
        }


        // grab all cameras
        gameCamera = GameObject.Find("ThirdPersonCamera").GetComponent<Camera>();
        //uiCamera = GameObject.Find("UICamera").GetComponent<Camera>();

        // gameobjects
        gameController = GameObject.Find("vGameController");
        notifyPanel = GameObject.Find("NotifyPanel");
        finalSpecialPanel = GameObject.Find("FinalSpecialText");
        // set this before we disable the throw button

        throwButton = GameObject.Find("ShroomLauncher");
        GoodieBag = GameObject.Find("GoodieBag");
        GoodieBagResult = GameObject.Find("GoodieResult");
        bestTime = GameObject.Find("BestTimeText");



        // grab all dropzones in the scene
        spawnLocations = GameObject.FindGameObjectsWithTag("DropZone");

        // grab all mystery chests
        mysteryLocations = GameObject.FindGameObjectsWithTag("MysteryChest");

        // grab and assign listeners to all buttons

        GoodieBag.GetComponent<Button>().onClick.AddListener(GrabGoodieBag);
        LoadLevelButton = GameObject.Find("LoadLevelButton").GetComponent<Button>();
        LoadLevelButton.GetComponent<Button>().onClick.AddListener(PlayLevel);
        LevelSelectButtonInitial = GameObject.Find("ReturnButton").GetComponent<Button>();
        LevelSelectButtonInitial.GetComponent<Button>().onClick.AddListener(ShowLevelSelect);
        LevelSelectButtonFinal = GameObject.Find("LevelSelectButtonFinal").GetComponent<Button>();
        LevelSelectButtonFinal.GetComponent<Button>().onClick.AddListener(ShowLevelSelect);
        LoadNextLevelButton = GameObject.Find("LoadNextLevelButton").GetComponent<Button>();
        LoadNextLevelButton.GetComponent<Button>().onClick.AddListener(LoadNextLevel);

        // assign all item images
        mainItemSprite = GameObject.Find("MainItemSprite").GetComponent<Image>();
        bonusItem1Sprite = GameObject.Find("BonusItem1Sprite").GetComponent<Image>();
        bonusItem2Sprite = GameObject.Find("BonusItem2Sprite").GetComponent<Image>();
        finalMainItemSprite = GameObject.Find("FinalMain").GetComponent<Image>();
        finalBonusItem1Sprite = GameObject.Find("FinalBonusStar1").GetComponent<Image>();
        finalBonusItem2Sprite = GameObject.Find("FinalBonusStar2").GetComponent<Image>();

        // assign all Text objects

        GoodieBagText = GameObject.Find("GoodieBagText").GetComponent<Text>();
        GoodieBagUIText = GameObject.Find("GoodieBagUIText").GetComponent<Text>();
        GoodieBagStamina = GameObject.Find("GoodieBagStamina");
        initialCanvas = GameObject.Find("InitialCanvas");
        gameCanvas = GameObject.Find("GameCanvas");
        levelCanvas = GameObject.Find("LevelCanvas");
        finalCanvas = GameObject.Find("FinalCanvas");
        mobileCanvas = GameObject.Find("CF2-Panel");
        GemText = GameObject.Find("GemTextUI").GetComponent<Text>();
        CoinText = GameObject.Find("CoinTextUI").GetComponent<Text>();
        finalTimeText = GameObject.Find("FinalTimeText").GetComponent<Text>();
        finalCoinText = GameObject.Find("FinalCoinText").GetComponent<Text>();
        // finalGemText = GameObject.Find("FinalGemText").GetComponent<Text>();
        finalDeductionText = GameObject.Find("FinalDeductionText").GetComponent<Text>();
        tallyTimeText = GameObject.Find("TallyTimeText").GetComponent<Text>();
        tallyCollectibleText = GameObject.Find("TallyCollectibleText").GetComponent<Text>();
        tallyItemText = GameObject.Find("TallyItemText").GetComponent<Text>();
        tallyDeductionText = GameObject.Find("TallyDeductionText").GetComponent<Text>();
        finalScoreText = GameObject.Find("FinalScoreText").GetComponent<Text>();
        notifyText = GameObject.Find("NotifyText").GetComponent<Text>();
        levelDescriptionText = GameObject.Find("LevelDescription").GetComponent<Text>();
        nextLevelName = GameObject.Find("NextLevelName").GetComponent<Text>();
        specialText = GameObject.Find("SpecialText").GetComponent<Text>();
        specialText.text = "";
        finalSpecialText = GameObject.Find("TallySpecialText").GetComponent<Text>();
        bestTimeText = GameObject.Find("BestTimeDataText").GetComponent<Text>();

        // now handle game level name update
        GameObject.Find("LevelTitleInitial").GetComponent<Text>().text = thisLevel.ToString();
        GameObject.Find("LevelTitleFinal").GetComponent<Text>().text = thisLevel.ToString();

    }


    private void HandleLevelDescriptionUpdate()
    {

        // handle level descriptions
        switch ((int)thisLevel)
        {
            case 0:
                {
                    levelDescriptionText.text = "Let's warm up with a nice easy island."; break;
                }
            case 1:
                {
                    levelDescriptionText.text = "Time to crank it up a little and make you work for these items!"; break;
                }
            case 2:
                {
                    levelDescriptionText.text = "You're doing great, but now let's introduce you to your first puzzle."; break;
                }
            case 3:
                {
                    levelDescriptionText.text = "Level 4 description"; break;
                }
            case 4:
                {
                    levelDescriptionText.text = "Level 5 description"; break;
                }
            case 5:
                {
                    levelDescriptionText.text = "Level 6 description"; break;
                }
            case 6:
                {
                    levelDescriptionText.text = "Level 7 description"; break;
                }
            case 7:
                {
                    levelDescriptionText.text = "Level 8 description"; break;
                }
            case 8:
                {
                    levelDescriptionText.text = "Level 9 description"; break;
                }
            default:// final level
                {
                    levelDescriptionText.text = "Level 10 description"; break;
                }
        }
    }
    private void InitializeAllVariables()
    {
        levelDeductionText = "None";
        levelDeduction = 0;
        levelBonusItemScore = 0;
        levelCoinCount = 0;
        levelGemCount = 0;
        levelTotalPoints = 0;
        finalMinutes = 0;
        finalSeconds = 0;
        foundMountainBlood = false;
    }

    private void InitializeGoodieBagAbilities()
    {
        GoodieBagStamina.SetActive(false);
        // hide all the dropzones
        foreach (GameObject dropzone in spawnLocations)
        {
            // hide the mesh
            dropzone.GetComponent<MeshRenderer>().enabled = false;
            // hide the halo
            dropzone.GetComponentInChildren<Light>().enabled = false;
        }
    }


    public void EnableThrow(bool value)
    {
        throwButton.SetActive(value);
    }

    public void GrabGoodieBag()
    {
        GoodieBag.SetActive(false);
        GoodieBagResult.SetActive(true);

        int goodieBagChoice = Random.Range(0, goodieBag.Length);

        GoodieBagText.text = goodieBagDescription[goodieBagChoice];
        levelDeductionText = goodieBag[goodieBagChoice];
        levelDeduction = goodieBagDeductions[goodieBagChoice];

        // set the game UI
        GoodieBagUIText.text = goodieBag[goodieBagChoice];
        // turn off all abilities first


        // now turn on only what we pulled from the bag
        switch (goodieBagChoice)
        {
            case 1: EnableGoggles(); break;
            case 2: EnableHalo(); break;
            case 3: EnableRadar(); break;

            case 4: { canJump = false; canSprint = false; break; }//bad knees
            case 5: { hasStamina = true; GoodieBagStamina.SetActive(true); break; }//stamina
            case 6: { SetPoisoned(true); break; }//poison
            default: { break; } // bag o nothing
        }

    }

    private void EnableGoggles()
    {
        foreach (GameObject dropzone in spawnLocations)
        {
            dropzone.GetComponent<MeshRenderer>().enabled = true;
        }
    }
    private void EnableRadar()
    {
        if (chosenDropzoneIndex != -1)
        {
            spawnLocations[chosenDropzoneIndex].GetComponentInChildren<Light>().enabled = true;
        }
    }
    private void EnableHalo()
    {
        haloEnabled = true;
        GoodieBagUIText.text = goodieBag[2];
        foreach (GameObject dropzone in spawnLocations)
        {
            dropzone.GetComponentInChildren<Light>().enabled = true;
        }
    }

    public static void DisableHalo()
    {
        if (instance.haloEnabled)
        {
            instance.GoodieBagUIText.text = "";
            foreach (GameObject dropzone in instance.spawnLocations)
                dropzone.GetComponentInChildren<Light>().enabled = false;
        }
    }

    private void LoadNextLevel()
    {
        //LevelLoader.instance.PlayNextLevel(((int)thisLevel + 1));
    }
    public void PlayLevel()
    {
        initialCanvas.SetActive(false);
        gameCanvas.SetActive(true);
        mobileCanvas.SetActive(true);
        uiCamera.transform.gameObject.SetActive(false);
        gameCamera.transform.gameObject.SetActive(true);
        gameController.GetComponent<Invector.vGameController>().enabled = true;
        //PuzzleTimer.StartTimer();
    }

    public void ShowLevelSelect()
    {
        levelCanvas.SetActive(true);
        initialCanvas.SetActive(false);
        //mobileCanvas.SetActive(false);
    }

    public void ReturnToLaunch()
    {
        levelCanvas.SetActive(false);
        initialCanvas.SetActive(true);
    }


    private void Start()
    {
        instance = this;
        coinList = GameObject.FindGameObjectsWithTag("Coin");
        gemList = GameObject.FindGameObjectsWithTag("Gem");

        gameCanvas.SetActive(isDevelopmentTest);
        initialCanvas.SetActive(!isDevelopmentTest);

        gameCamera.transform.gameObject.SetActive(isDevelopmentTest);
        uiCamera.transform.gameObject.SetActive(!isDevelopmentTest);
        finalCanvas.SetActive(false);
        levelCanvas.SetActive(false);
        mobileCanvas.SetActive(false);
        throwButton.SetActive(false);
        GoodieBagResult.SetActive(false);

        SpawnItem();
        SpawnMysteryChests();

        GoodieBagText.text = "";
        GoodieBagUIText.text = "";
        InitializeGoodieBagAbilities();

        finalSpecialPanel.SetActive(false);

        HandleLevelDescriptionUpdate();

        // if we're playing the game, player data comes from the database
        // if (DataManager.instance != null)
        // {
        //     gamePlayer = DataManager.instance.playerData;
        //     // since have live data, update the best time accordingly
        //     if (gamePlayer.availableLevels[(int)thisLevel].bestTimeMinutes != null)
        //     {
        //         int mins = (int)gamePlayer.availableLevels[(int)thisLevel].bestTimeMinutes;
        //         int secs = (int)gamePlayer.availableLevels[(int)thisLevel].bestTimeSeconds;
        //         bestTime.SetActive(true);
        //         bestTimeText.text = $"{mins}:{secs}";
        //     }
        // }

        // else
        // {
        //     // otherwise use Dad's data
        //     print("Dad's playing");
        //     gamePlayer = new PlayerData("1111", "Dad");
        // }

    }

    private void SpawnItem()
    {
        chosenDropzoneIndex = Random.Range(0, spawnLocations.Length);
        GameObject mountainBlood = Instantiate(spawnItem, spawnLocations[chosenDropzoneIndex].transform.position, Quaternion.identity);
        mountainBlood.GetComponent<Item>().enabled = true;
    }

    private void SpawnMysteryChests()
    {
        chosenMysteryIndex = Random.Range(0, mysteryLocations.Length);
        // disable all of them first
        foreach (GameObject chest in mysteryLocations)
        {
            chest.SetActive(false);
        }
        // now enable only the one we chose
        mysteryLocations[chosenMysteryIndex]?.SetActive(true);
    }

    private void FoundMysteryChest(int itemIndex)
    {

        switch (itemIndex)
        {
            case 0:
                {
                    SetPoisoned(false);
                    //potion
                    break;
                }
            case 1:
                {
                    EnableHalo();
                    //halo
                    break;
                }
            case 2:
                {
                    hasStamina = false;
                    //jug
                    break;
                }
            case 3:
                {
                    //coins
                    levelCoinCount += 1000;
                    break;
                }
            case 4:
                {
                    //gems
                    levelGemCount += 200;
                    break;
                }
            case 5:
                {
                    //XP
                    //ExpManager.UpdateXP(100);
                    break;
                }
        }
        // respawn the chests
        SpawnMysteryChests();

    }

    public static void SetItemFound(int itemFound, bool isCollectible, bool isChest)
    {
        Collectible newCollectible;
        if (isCollectible)
        {
            if (!instance.finalSpecialPanel.activeInHierarchy)
                instance.finalSpecialPanel.SetActive(true);

            int bonusAmount = 0;

            switch (itemFound)
            {
                case 1:
                    {
                        bonusAmount = 1000;
                        instance.gamePlayer.XP += 1000;
                        //ExpManager.UpdateXP(1000);

                        newCollectible = new Collectible("Wooden Horse", 1);
                        instance.gamePlayer.collection.Add(newCollectible);
                        break;
                    }
                case 2:
                    {
                        bonusAmount = 2500;
                        instance.gamePlayer.XP += 2500;
                        //ExpManager.UpdateXP(2500);// found the bear

                        newCollectible = new Collectible("Toy Bear", 2);
                        instance.gamePlayer.collection.Add(newCollectible);
                        break;
                    }
                case 3:
                    {
                        bonusAmount = 5000;
                        instance.gamePlayer.XP += 5000;
                        //ExpManager.UpdateXP(5000);// found the ornament

                        newCollectible = new Collectible("Ornament", 3);
                        instance.gamePlayer.collection.Add(newCollectible);
                        break;
                    }
                case 4:
                    {
                        bonusAmount = 10000;
                        instance.gamePlayer.XP += 10000;
                        //ExpManager.UpdateXP(10000);// found the starfish

                        newCollectible = new Collectible("Starfish", 4);
                        instance.gamePlayer.collection.Add(newCollectible);
                        break;
                    }
            }
            instance.specialPoints = instance.specialPoints + bonusAmount;
            instance.specialText.text = "Special:" + instance.specialPoints;
        }
        else
        {
            // if this was a chest, then we need to use the itemFound differently
            if (isChest)
            {
                instance.FoundMysteryChest(itemFound);
            }
            else
            {
                switch (itemFound)
                {
                    // main level items
                    case 0:
                        {
                            // mountain blood
                            instance.mainItemSprite.sprite = instance.foundSprite;
                            foundMountainBlood = true;
                            instance.gamePlayer.XP += 1000;
                            //ExpManager.UpdateXP(1000);
                            instance.gamePlayer.availableLevels[(int)instance.thisLevel].hasCompleted = true;
                            break;
                        }
                    case 1:
                        {
                            instance.gamePlayer.XP += 500;
                            //ExpManager.UpdateXP(500);
                            instance.levelBonusItemScore++;
                            instance.bonusItem1Sprite.sprite = instance.foundSprite;
                            instance.foundBonus1 = true;
                            instance.gamePlayer.availableLevels[(int)instance.thisLevel].foundCrystal1 = true;
                            break;
                        }
                    case 2:
                        {
                            instance.gamePlayer.XP += 500;
                            //ExpManager.UpdateXP(500);
                            instance.levelBonusItemScore++;
                            instance.bonusItem2Sprite.sprite = instance.foundSprite;
                            instance.foundBonus2 = true;
                            instance.gamePlayer.availableLevels[(int)instance.thisLevel].foundCrystal2 = true;
                            break;
                        }
                    // collectibles
                    case 3:
                        {
                            instance.gamePlayer.XP += 1;
                            //ExpManager.UpdateXP(1);
                            instance.levelCoinCount++;
                            break;
                        }
                    case 4:
                        {
                            instance.gamePlayer.XP += 5;
                            //ExpManager.UpdateXP(5);
                            instance.levelGemCount++; break;
                        }
                    //case 5: { instance.FoundMysteryChest(itemFound); break; }//found the chest
                    case 6: { instance.SetPoisoned(false); break; }//potion
                    case 7: { break; }//jug
                    case 8: { instance.EnableHalo(); break; }//halo enabled
                    case 9: { instance.EnableThrow(true); break; } // found the shroom
                    case 10: LevelCompleted(); break;
                    default: { break; }
                }
            }


        }

    }



    private string GetNextLevelName(int index)
    {
        // unlock the next level
        instance.gamePlayer.availableLevels[index].available = true;
        // grab its name and return it
        CurrentLevel nextLevel = (CurrentLevel)index;
        return nextLevel.ToString();
    }


    private void SetNewBestTime()
    {
        instance.gamePlayer.availableLevels[(int)instance.thisLevel].bestTimeMinutes = finalMinutes;
        instance.gamePlayer.availableLevels[(int)instance.thisLevel].bestTimeMinutes = finalMinutes;
    }


    private void HandleBestTime()
    {
        int currentLevelIndex = (int)instance.thisLevel;

        // Now let's set the best time for this level
        // if it is null, then this is the first time playing the level so just save it
        if (instance.gamePlayer.availableLevels[currentLevelIndex] == null)
            instance.SetNewBestTime();
        else // if it isn't null, then have a best time saved, so compare it
        {
            if (finalMinutes < instance.gamePlayer.availableLevels[currentLevelIndex].bestTimeMinutes)
            {
                // if current final minutes is simply less than what is saved
                instance.SetNewBestTime();
            }
            else if (finalMinutes == instance.gamePlayer.availableLevels[currentLevelIndex].bestTimeMinutes)
            {
                // if final minutes and saved minutes is the same then we do a final compare on the seconds
                if (finalSeconds < instance.gamePlayer.availableLevels[currentLevelIndex].bestTimeSeconds)
                {
                    // finalSeconds is simply less than saved seconds
                    instance.SetNewBestTime();
                }
            }
        }
    }

    public static void LevelCompleted()
    {
        // find out what the next level is
        int nextLevelIndex = (int)instance.thisLevel + 1;// TODO: handle the last level
        // set its name in the data
        instance.gamePlayer.availableLevels[nextLevelIndex].name = instance.GetNextLevelName(nextLevelIndex);


        instance.finalCanvas.SetActive(true);
        instance.gameCanvas.SetActive(false);
        instance.gameController.GetComponent<Invector.vGameController>().enabled = false;
        instance.uiCamera.transform.gameObject.SetActive(true);
        instance.gameCamera.transform.gameObject.SetActive(false);
        instance.CalculateLevelPoints();
        instance.HandleBestTime();


        instance.HandleCollectiblesForSave();
        instance.HandleAttachmentsForSave();

        instance.HandleGreatestItemCompare();

        // if we are actually playing the game, save the data
        // TODO: handle anonymous login


        //stop timer
        //PuzzleTimer.instance.finished = true;
        // // unlock the next level
        // LevelLoader.instance.UnlockNextLevel();

        // if (HandleFirebase.instance.thisPlayer != null)
        // {
        //     HandleFirebase.SavePlayer(instance.gamePlayer);
        // }
        // else // testing levels without real PlayerData
        // {
        print(JsonUtility.ToJson(instance.gamePlayer));
        // }
    }

    private int HandleTimeScore()
    {
        //finalMinutes = PuzzleTimer.instance.timeMinutes;
        // finalSeconds = PuzzleTimer.instance.timeSeconds;

        switch (finalMinutes)
        {
            case 4: { return 100; }
            case 3: { return 250; }
            case 2: { return 500; }
            case 1: { return 750; }
            case 0: { return 1000; }
            default: { return 50; }
        }
    }


    private void HandleCollectiblesForSave()
    {
        // check for all items found and mark them as saved
        foreach (Collectible item in gamePlayer.collection)
            item.saved = true;
    }

    private void HandleAttachmentsForSave()
    {
        // check for all items found and mark them as saved
        foreach (Attachment item in gamePlayer.attachments)
            item.saved = true;
    }

    private void HandleGreatestItemCompare()
    {
        // check to see what player found in this level and compare to see if its the best
        instance.gamePlayer.collection.Sort((c1, c2) => c2.collectibleRank.CompareTo(c1.collectibleRank));
        instance.gamePlayer.greatestItem = instance.gamePlayer.collection[0].name;
    }
    private void CalculateLevelPoints()
    {
        // coins are 1 point each, gems are 5 pts each
        int collectibleScore = levelCoinCount + (levelGemCount * 5);

        // handle bonus items
        int itemScore = (levelBonusItemScore * 500) + 1000; // 1000 is for getting the main item
        if (foundBonus1)
            finalBonusItem1Sprite.sprite = foundSprite;

        if (foundBonus2)
            finalBonusItem2Sprite.sprite = foundSprite;

        finalMainItemSprite.sprite = foundSprite;// no need to check anything, if we get here, we got it

        // time
        int timeScore = HandleTimeScore();
        levelTotalPoints = collectibleScore + itemScore + timeScore;

        // handle deductions


        // set all text objects
        finalTimeText.text = "Time:" + finalMinutes + ":" + finalSeconds;
        finalCoinText.text = levelCoinCount.ToString();
        finalGemText.text = levelGemCount.ToString();
        finalDeductionText.text = levelDeductionText;

        tallyTimeText.text = timeScore.ToString() + "pts!";
        tallyCollectibleText.text = collectibleScore.ToString() + "pts!";
        tallyItemText.text = itemScore.ToString() + "pts!";

        finalSpecialText.text = specialPoints.ToString();

        if (levelDeduction < 0)
        {
            tallyDeductionText.text = "-" + levelDeduction + "pts!";
            levelTotalPoints = levelTotalPoints - levelDeduction;
        }
        else if (levelDeduction > 0)
        {
            tallyDeductionText.text = "+" + levelDeduction + "pts!";
            levelTotalPoints = levelTotalPoints + levelDeduction;
        }
        else
        {
            tallyDeductionText.text = "None";
        }

        levelTotalPoints = levelTotalPoints + specialPoints;

        // update next level name
        int currentLevelIndex = (int)thisLevel;
        nextLevelName.text = LevelLoader.instance.levelNames[currentLevelIndex + 1];
        // finally set the total score
        finalScoreText.text = "Total: " + levelTotalPoints.ToString();

        instance.HandleHighScore(levelTotalPoints);
    }


    private void HandleHighScore(int levelScore)
    {
        // check to make sure we need to update the data
        if (levelScore > instance.gamePlayer.availableLevels[(int)instance.thisLevel].highScore)
            instance.gamePlayer.availableLevels[(int)instance.thisLevel].highScore = levelScore;
    }

    private void SetPoisoned(bool state)
    {
        speedReduced = state;
        hasStamina = state;

    }
    private void Update()
    {

        GoodieBagStamina.SetActive(hasStamina);
        if (ControlFreak2.CF2Input.GetKey(KeyCode.Escape))
        {
            ReturnToLaunch();
        }

        // update all Text fields accordingly
        GemText.text = instance.levelGemCount.ToString() + " / " + gemList.Length;
        CoinText.text = instance.levelCoinCount.ToString() + " / " + coinList.Length;
        SeedCountText.text = currentSeedCount.ToString();

        if (ControlFreak2.CF2Input.GetKey(KeyCode.P))
        {
            EnableRadar();
        }

        // if we are showing the throw button and run out of seeds, hide the button
        if (currentSeedCount < 1 && throwButton.activeInHierarchy)
            throwButton.SetActive(false);


    }

}
