using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

namespace Galo
{
    [System.Serializable]
    public enum PlayerType { RUNNER, FIGHTER, CLIMBER };


    [System.Serializable]
    public class GaloCollectible
    {
        public int value;
        public string name;
        public GaloCollectible(int value, string name)
        {
            this.value = value;
            this.name = name;
        }
    }

    [System.Serializable]
    public class AllGaloPlayers
    {
        public GaloPlayerData lastUsedPlayer;
        public List<GaloPlayerData> savedPlayers;
        public AllGaloPlayers(GaloPlayerData newPlayer)
        {
            savedPlayers = new List<GaloPlayerData>();
            savedPlayers.Add(newPlayer);
        }
    }


    [System.Serializable]
    public class Tribe
    {
        public Dictionary<string, int> tribe;
    }

    [System.Serializable]
    public class GaloPlayerData
    {

        public int rank;//save as int to convert to Ranking later
        public string rankString;
        public string currentPlayer;
        public double XP;
        public List<GaloCollectible> collection;

        public List<GaloLevel> availableLevels;

        public GaloPlayerData()
        {
            this.currentPlayer = "Caden";
            this.rank = 0;
            this.rankString = "Noob";
            this.XP = 0;


            // handle all lists
            this.collection = new List<GaloCollectible>();

            // // create and add all 10 levels
            this.availableLevels = new List<GaloLevel>();
            string[] levelNames = Enum.GetNames(typeof(GaloLevelNames));

            for (int i = 0; i < levelNames.Length; i++)
            {
                GaloLevel newLevel;
                newLevel = new GaloLevel(levelNames[i]);
                this.availableLevels.Add(newLevel);
            }

        }




    }
}