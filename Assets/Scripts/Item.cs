using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Events;
public class Item : MonoBehaviour
{
    public static Item instance;
    AudioSource _audioSource;
    public AudioClip _audioClip;

    public enum ItemType { Main, Crystal1, Crystal2, Coin, Gem, Chest, Potion, Jug, Halo, Shroom, IslandHeart };
    private int[] itemPoints = new int[] { 1000, 500, 500, 1, 5, 250, 300, 300, 750, 800, 1000 };

    public enum CollectibleType { None, Jar, Horse, Bear, Ornament, Barrel, Bellows, Pulley, SoccerBall, LifePreserver, BarrelContainer };

    private enum MysteryType { Potion, Halo, Jug, Coins, Gems, XP };
    private int myMysteryIndex;



    // public UnityEvent OnPlayerEnter;

    public ItemType myType;
    public int seedCount;


    public enum SeedType { None, Flower, Coin, Crystal, Tree, SpiderTrap };
    public SeedType myShroomType;
    public CollectibleType myCollectible;


    public bool willSpin = true;
    public float SpinRate = 1.0f;
    public bool SpinX = false;
    public bool SpinY = false;
    public bool SpinZ = true;




    private string[] gemText = new string[] { "Sweet a gem! Nice find!", "Another gem! Way to go!", "Gems are awesome!" };
    private string[] crystalText = new string[] { "Sweet! You found one of the Mountain Crystals! There is another somewhere...", "Great! You found both Mountain Crystals!" };
    private string[] mysteryText = new string[] {
        "a Potion! You feel your speed returning immediately!",
        "Halo! Look to the skies my friend!",
        " a Jug! No more stamina issues!",
        "1000 coins!",
        "200 gems!",
        "100 XP!" };

    private string[] collectibleText = new string[]{
    "", // none
    "A small, hand carved wooden horse!  What a rare find!",
    "A child's stuffed bear, this is incredibly rare to find, well done!",
    "A very old, delicately carved ornament of some kind, you are an excellent scavenger!",
    "Hmm...I believe this is a starfish, we have not seen any these in decades!"};

    private string[] shroomText = new string[]{
    "A Flower Mushroom! Spread these around and grow something beautiful!\nIt only sprouts on good soil.",
    "A Coin Mushroom! Grow these to sprout coins!\nIt will only sprout on the gravel path",
    "A Crystal Mushroom! These bloom into very rare gems!\nBest thrown on hard rock.",
    "A Tree Mushroom! Very rare, but useful if you need to reach certain heights\nIt only sprouts on good soil.",
    "A Spidertrap Mushroom! Use these to grow shrubs that no spider will pass!\nThrow it anywhere!"};

    private int[] collectiblePoints = new int[]{
        0, // none
        1000,//horse
        2500,// bear
        5000,// ornament
        10000,//starfish
    };
    private string[] messages = new string[]{
    "", "", "", "", "", "",
    "Your poisoning has been healed!",
    "All stamina issues have been resolved!",
    "You have enabled the Halo to help you locate this Mountain's blood!",


};



    private void HandleCollectibles()
    {
        // if this is a collectible, determine when to unhide it based on the players ranking
        if (myCollectible != CollectibleType.None)
        {
            bool showCollectible = false;
            int playerRanking = 0;//(int)ExpManager.myRanking;

            switch (myCollectible)
            {
                case CollectibleType.Horse:// Noob
                    {
                        // TODO: randomly choose which one to show
                        showCollectible = true;// always show the initial collectible
                        break;
                    }
                case CollectibleType.Bear://Spotter
                    {
                        showCollectible = playerRanking > 2;
                        break;
                    }
                case CollectibleType.Barrel:// Finder
                    {
                        showCollectible = playerRanking > 5;
                        break;
                    }
                case CollectibleType.Ornament://Gatherer
                    {
                        showCollectible = playerRanking > 8;
                        break;
                    }
            }

            // if we determine that we need to show the model, do so only if it isnt already showing
            if (!gameObject.activeInHierarchy)
                EnableThisObject(showCollectible);

        }
    }

    void Start()
    {
        instance = this;

        // locate required items
        _audioSource = GetComponent<AudioSource>();

        // hide this item by default if one of these basic categories
        if (myType == ItemType.Potion || myType == ItemType.Jug || myType == ItemType.Halo)
        {

            EnableThisObject(false);
        }
        else if (myType == ItemType.Chest)
        {
            // if we set this as a mystery chest
            myMysteryIndex = Random.Range(0, mysteryText.Length);

        }
        else
        {
            // hide by default based on loaded player ranking
            HandleCollectibles();
        }
    }

    private void HandleMessageDisplay()
    {
        string message = "";
        string guideMessage = "";
        float baseDuration = 2f;

        if (myType == ItemType.Main)
        {
            message = "Great job! Now just locate the heart of the island!";
            Notificatons.ShowNotification(message, baseDuration);
            guideMessage = "You have found the mysyterious Mountain Blood, now you are trying to locate the heart of the island.  See if you can find anything similiar to the Mountain Blood around the island. Maybe...the same color, or texture...";
            Guide.instance.UpdateGuide(guideMessage);
            InteractionManager.DisableHalo();
        }
        if (myType == ItemType.IslandHeart)
        {
            if (InteractionManager.foundMountainBlood)
            {
                guideMessage = "At last! You have found the Island Heart and the Mountain Blood! Before you enter the heart, be sure that you are done with the island. Is there anything else you might like to find before you leave?";
                Guide.instance.UpdateGuide(guideMessage);
            }
        }
        if (myType == ItemType.Gem)//certain items have an array of messages to display
        {
            message = gemText[InteractionManager.instance.levelGemCount - 1];
            Notificatons.ShowNotification(message, baseDuration);
        }
        else if (myType == ItemType.Crystal1 || myType == ItemType.Crystal2)
        {
            message = crystalText[InteractionManager.instance.levelBonusItemScore - 1];
            Notificatons.ShowNotification(message, 3f);
        }
        else if (myType == ItemType.Shroom)
        {
            message = shroomText[(int)myShroomType - 1]; // account for the 'None' option
            Notificatons.ShowNotification(message, 4f);
            //signal to the Launcher which to seed to fire
            //Launcher.seedIndex = (int)myShroomType - 1;// again, account for 'None'
        }
        else if (myType == ItemType.Chest)
        {
            // check to see if the player is poisoned, if they are NOT, and we drew to give a potion
            // then push the index 1 past to not waste a potion
            // TODO: not tested
            if (!InteractionManager.instance.speedReduced && myMysteryIndex == 1)
                myMysteryIndex++;

            message = $"You found a Mystery Chest which contained.....{mysteryText[myMysteryIndex]}";
            Notificatons.ShowNotification(message, baseDuration);
        }
        else if (myCollectible != CollectibleType.None)
        {
            message = collectibleText[(int)myCollectible];
            Notificatons.ShowNotification(message, 3f);

            if (InteractionManager.foundMountainBlood)
                guideMessage = "Since you have already found the Mountain Blood, keep searching for more of these rare items, I'm sure there is much to be found!";
            else
                guideMessage = "You are still exploring and have yet to locate the Mountain Blood, but collecting items like this will go a long way to boost your experience, keep looking everywhere!";

            Guide.instance.UpdateGuide(guideMessage); ;
        }
        else if ((myType != ItemType.IslandHeart) && (myType != ItemType.Main))
        {
            message = messages[(int)myType];
            Notificatons.ShowNotification(message, baseDuration);

        }


    }


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            _audioSource.PlayOneShot(_audioClip);
            EnableThisObject(false);

            // if this is a collectible trigger differently
            if (myCollectible != CollectibleType.None)
            {
                InteractionManager.SetItemFound((int)myCollectible, true, false);
                print("Collectible points" + collectiblePoints[(int)myCollectible]);
                //ExpManager.UpdateXP(collectiblePoints[(int)myCollectible]);
            }
            else if (myType == ItemType.Chest)
            {
                InteractionManager.SetItemFound((int)myMysteryIndex, false, true);
                //ExpManager.UpdateXP(itemPoints[(int)myType]);
            }
            else if (myType == ItemType.Shroom)
            {
                // if this is a shroom, we also need to pass the number of seeds to add
                InteractionManager.instance.currentSeedCount += seedCount;
                InteractionManager.SetItemFound((int)myType, false, false);
                //ExpManager.UpdateXP(itemPoints[(int)myType]);
            }
            else if (myType == ItemType.IslandHeart)
            {
                // make sure the player has found the mountain blood first
                if (InteractionManager.foundMountainBlood)
                {
                    InteractionManager.SetItemFound((int)myType, false, false);
                    //E/xpManager.UpdateXP(itemPoints[(int)myType]);
                }
            }
            else
            {
                InteractionManager.SetItemFound((int)myType, false, false);
                //ExpManager.UpdateXP(itemPoints[(int)myType]);
            }

            HandleRemoval();

        }
    }

    private void HandleRemoval()
    {
        // dont show the display if its a coin
        if (myType != ItemType.Coin && myType != ItemType.IslandHeart)
            HandleMessageDisplay();
        else if (myType == ItemType.Gem || myType == ItemType.Coin)
            StartCoroutine(ReplaceItem());// instead of destroying, begin timer so we can make it reappear
        else
            Destroy(gameObject, _audioClip.length);
    }

    IEnumerator ReplaceItem()
    {
        yield return new WaitForSeconds(myType == ItemType.Gem ? 340f : 120f);
        EnableThisObject(true);
    }


    private void EnableThisObject(bool state)
    {
        // disable the collider on this object right away
        gameObject.GetComponent<SphereCollider>().enabled = state;

        if (myType != ItemType.IslandHeart)//islandheart has nothing to hide
        {
            if (transform.childCount > 0)
                transform.GetChild(0).GetComponent<MeshRenderer>().enabled = state;
            else
                gameObject.GetComponent<MeshRenderer>().enabled = state;
        }

    }

    private void Update()
    {
        // turn this items on if they arent already
        // if (!gameObject.activeInHierarchy)
        // {
        HandleCollectibles();

        //show this object only if we are poisoned
        if (myType == ItemType.Potion)
        {
            EnableThisObject(InteractionManager.instance.speedReduced);
        }

        //show this object only if we have stamina, but not if we're poisoned
        if (myType == ItemType.Jug && (InteractionManager.instance.hasStamina && !InteractionManager.instance.speedReduced))
        {
            EnableThisObject(true);
        }


        if (myType == ItemType.Halo)
            if (!gameObject.GetComponent<MeshRenderer>().enabled)
                // if (PuzzleTimer.instance.timeMinutes > 2)
                //     EnableThisObject(true);




                if (willSpin)
                {
                    var spinAmount = (SpinRate * 50) * Time.deltaTime;
                    transform.Rotate(SpinX ? spinAmount : 0, SpinY ? spinAmount : 0, SpinZ ? spinAmount : 0);
                }


    }
}

