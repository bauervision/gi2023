using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace Galo
{
    public enum Ranking
    {
        Noob, // 0
        Pro,// 5000
        Master,//10,000

        Spotter,// 16,000
        Pro_Spotter,// 24,000
        Master_Spotter,//30,000

        Finder, //37,000
        Pro_Finder,// 44,000
        Master_Finder,//53,000


        Gatherer,// 62,000
        Pro_Gatherer,//70,000
        Master_Gatherer,//78,000


        Forager,//87,000
        Pro_Forager,//100,000
        Master_Forager,//110,000

        Discoverer,//130,000
        Pro_Discoverer,//150,000
        Master_Discoverer,//175,000


        Collector,// 200,000
        Pro_Collector,//250,000
        Master_Collector,//300,000

        Hoarder,//400,000
        Pro_Hoarder,// 500,000
        Master_Hoarder,//600,000

        Scrounge,//700,000
        Pro_Scrounge,//820,000
        Master_Scrounge,//1,000,000

        Hunter,//1,250,000
        Pro_Hunter,//1,500,000
        Master_Hunter,//1,750,000

        Connoisseur,//2,000,000
        Pro_Connoisseur,//2,500,000
        Master_Connoisseur,//3,000,000

        Curator,//3,750,000
        Pro_Curator,//4,500,000
        Master_Curator,//5,250,000

        Adventurer,//6,200,000
        Pro_Adventurer,//7,000,000
        Master_Adventurer,//7,800,000

        Scavenger,//10,000,000
        Pro_Scavenger,//11,000,000
        Master_Scavenger,//12,000,000

        Mega_Master,//20,000,000
        Elite_Master,//30,000,000
        Ultra_Master,//40,000,000
        Divine_Master,//50,000,000
        Infinite_Master,//100,000,000,000
        DEV

    }

    public class ExpManager : MonoBehaviour
    {
        public static ExpManager instance;
        public TextMeshProUGUI xpText;
        public TextMeshProUGUI rankText;
        public TextMeshProUGUI coinsFoundText;
        public TextMeshProUGUI gemsFoundText;

        private GameObject[] coinList;
        private GameObject[] gemList;



        private int nextLevel;


        private int[] nextLevelPoints = new int[] {
        5000,
        10000,
        16000,
        24000,
        30000,
        37000,
        44000,
        53000,
        62000,
        70000,
        78000,
        87000,
        100000,
        110000,
        130000,
        150000,
        175000,
        200000,
        250000,
        300000,
        400000,
        500000,
        600000,
        700000,
        820000,
        1000000,
        1250000,
        1500000,
        1750000,
        2000000,
        2500000,
        3000000,
        3750000,
        4500000,
        5250000,
        6200000,
        7000000,
        7800000,
        10000000,
        12000000,
        17000000,
        20000000,
        30000000,
        40000000,
        50000000,
        100000000
    };
        public Ranking myRanking;
        Ranking oldRanking;
        private TextMeshProUGUI savingText;

        public int levelCoinCount = 0;
        public int levelGemCount = 0;

        private void Awake()
        {
            instance = this;

        }


        private void Start()
        {
            instance = this;
            if (DataManager.instance != null)
            {
                oldRanking = myRanking = (Ranking)DataManager.instance.playerData.rank;
            }

            coinList = GameObject.FindGameObjectsWithTag("Coin");
            gemList = GameObject.FindGameObjectsWithTag("Gem");
            UpdateCoinGemCountUI();

        }

        private void Update()
        {

            MonitorRanking();

            // make sure we have playerData first
            if (DataManager.instance != null)
            {
                nextLevel = nextLevelPoints[(int)myRanking];

                //keep xpText updated with current xp value
                xpText.text = $"XP: {DataManager.instance.playerData.XP}/{nextLevel}";
                string displayRank = myRanking.ToString().Replace("_", " ");
                rankText.text = $"Rank: {displayRank}";

                // trigger level up notification
                if (oldRanking != myRanking)
                {
                    RankingMonitor.instance.TriggerLevelUp(displayRank);
                    oldRanking = myRanking;
                    // save the new rank
                    DataManager.instance.playerData.rank = (int)myRanking;
                    DataManager.instance.SaveData();
                }

            }

            MonitorCoinsAndGems();

        }

        private void MonitorCoinsAndGems()
        {
            if ((coinList.Length == levelCoinCount) && (gemList.Length == levelGemCount))
            {
                foreach (GameObject coin in coinList)
                    coin.GetComponent<Coin>().RegenerateMe();

                foreach (GameObject gem in gemList)
                    gem.GetComponent<Coin>().RegenerateMe();
            }
        }

        public static void UpdateXP(int xpIncrease)
        {
            if (DataManager.instance)
                if (DataManager.instance.playerData != null)
                    DataManager.instance.playerData.XP += xpIncrease;


        }

        public static void UpdateXPCoin(int xpIncrease, CoinType myType)
        {
            if (DataManager.instance)
                if (DataManager.instance.playerData != null)
                    DataManager.instance.playerData.XP += xpIncrease;

            // update all Text fields accordingly
            if (myType == CoinType.Coin)
            {
                instance.levelCoinCount++;
                instance.coinsFoundText.text = instance.levelCoinCount.ToString() + " / " + instance.coinList.Length;
            }
            else if (myType == CoinType.Gem)
            {
                instance.levelGemCount++;
                instance.gemsFoundText.text = instance.levelGemCount.ToString() + " / " + instance.gemList.Length;
            }
        }

        void UpdateCoinGemCountUI()
        {
            instance.coinsFoundText.text = instance.levelCoinCount.ToString() + " / " + instance.coinList.Length;
            instance.gemsFoundText.text = instance.levelGemCount.ToString() + " / " + instance.gemList.Length;
        }

        public static void UpdateCollectible(GaloCollectible newCollectible)
        {
            if (DataManager.instance)
            {
                if (DataManager.instance.playerData != null)
                {
                    DataManager.instance.playerData.XP += newCollectible.value;
                    DataManager.instance.SaveCollectibleData(newCollectible);
                    UIManager.instance.UpdateProfilePage();
                }

            }
        }
        private void MonitorRanking()
        {
            if (DataManager.instance != null)
            {
                double xp = DataManager.instance.playerData.XP;
                // will probably break this out further into color levels for each stage
                if (xp < 5000) myRanking = Ranking.Noob;
                if (xp >= 5000 && xp < 10000) myRanking = Ranking.Pro;
                if (xp >= 10000 && xp < 16000) myRanking = Ranking.Master;

                if (xp >= 16000 && xp < 24000) myRanking = Ranking.Spotter;
                if (xp >= 24000 && xp < 30000) myRanking = Ranking.Pro_Spotter;
                if (xp >= 30000 && xp < 37000) myRanking = Ranking.Master_Spotter;

                if (xp >= 37000 && xp < 44000) myRanking = Ranking.Finder;
                if (xp >= 44000 && xp < 53000) myRanking = Ranking.Pro_Finder;
                if (xp >= 53000 && xp < 62000) myRanking = Ranking.Master_Finder;

                if (xp >= 62000 && xp < 70000) myRanking = Ranking.Gatherer;
                if (xp >= 70000 && xp < 78000) myRanking = Ranking.Pro_Gatherer;
                if (xp >= 78000 && xp < 87000) myRanking = Ranking.Master_Gatherer;

                if (xp >= 87000 && xp < 100000) myRanking = Ranking.Forager;
                if (xp >= 100000 && xp < 110000) myRanking = Ranking.Pro_Forager;
                if (xp >= 110000 && xp < 130000) myRanking = Ranking.Master_Forager;

                if (xp >= 130000 && xp < 150000) myRanking = Ranking.Discoverer;
                if (xp >= 150000 && xp < 175000) myRanking = Ranking.Pro_Discoverer;
                if (xp >= 175000 && xp < 200000) myRanking = Ranking.Master_Discoverer;

                if (xp >= 200000 && xp < 250000) myRanking = Ranking.Collector;
                if (xp >= 250000 && xp < 300000) myRanking = Ranking.Pro_Collector;
                if (xp >= 300000 && xp < 400000) myRanking = Ranking.Master_Collector;

                if (xp >= 400000 && xp < 500000) myRanking = Ranking.Hoarder;
                if (xp >= 500000 && xp < 600000) myRanking = Ranking.Pro_Hoarder;
                if (xp >= 600000 && xp < 700000) myRanking = Ranking.Master_Hoarder;

                if (xp >= 700000 && xp < 820000) myRanking = Ranking.Scrounge;
                if (xp >= 820000 && xp < 1000000) myRanking = Ranking.Pro_Scrounge;
                if (xp >= 1000000 && xp < 1250000) myRanking = Ranking.Master_Scrounge;

                if (xp >= 1250000 && xp < 1500000) myRanking = Ranking.Hunter;
                if (xp >= 1500000 && xp < 1750000) myRanking = Ranking.Pro_Hunter;
                if (xp >= 1750000 && xp < 2000000) myRanking = Ranking.Master_Hunter;

                if (xp >= 2000000 && xp < 2500000) myRanking = Ranking.Connoisseur;
                if (xp >= 2500000 && xp < 3000000) myRanking = Ranking.Pro_Connoisseur;
                if (xp >= 3000000 && xp < 3750000) myRanking = Ranking.Master_Connoisseur;

                if (xp >= 3750000 && xp < 4500000) myRanking = Ranking.Curator;
                if (xp >= 4500000 && xp < 5250000) myRanking = Ranking.Pro_Curator;
                if (xp >= 5250000 && xp < 6200000) myRanking = Ranking.Master_Curator;

                if (xp >= 6200000 && xp < 7000000) myRanking = Ranking.Adventurer;
                if (xp >= 7000000 && xp < 7800000) myRanking = Ranking.Pro_Adventurer;
                if (xp >= 7800000 && xp < 10000000) myRanking = Ranking.Master_Adventurer;

                if (xp >= 10000000 && xp < 12000000) myRanking = Ranking.Scavenger;
                if (xp >= 12000000 && xp < 17000000) myRanking = Ranking.Pro_Scavenger;
                if (xp >= 17000000 && xp < 20000000) myRanking = Ranking.Master_Scavenger;

                if (xp >= 20000000 && xp < 30000000) myRanking = Ranking.Mega_Master;
                if (xp >= 30000000 && xp < 40000000) myRanking = Ranking.Elite_Master;
                if (xp >= 40000000 && xp < 50000000) myRanking = Ranking.Ultra_Master;
                if (xp >= 50000000 && xp < 100000000) myRanking = Ranking.Divine_Master;
                if (xp >= 100000000) myRanking = Ranking.Infinite_Master;
                // }
                // else
                // {//testing in dev mode
                //     myRanking = Ranking.DEV;
                // }

            }




        }


        public double GetNextLevel()
        {
            return nextLevelPoints[(int)myRanking];
        }
    }


}