using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

[System.Serializable]
public class AllPlayers
{
    public string lastUsedPlayer = string.Empty;
    public List<PlayerData> savedPlayers;
    public AllPlayers(PlayerData newPlayer)
    {
        savedPlayers = new List<PlayerData>();
        savedPlayers.Add(newPlayer);
    }
}

[System.Serializable]
public class PlayerData
{
    public string userPin;
    public string name;
    public int rank;//save as int to convert to Ranking later
    public string rankString;
    public double XP;
    public string greatestItem;
    public DateTime dateJoined;
    public List<Collectible> collection;
    public List<Attachment> attachments;

    public List<Level> availableLevels;



    public PlayerData(string pin, string name)
    {
        this.userPin = pin;
        this.name = name;
        this.rank = 0;
        this.rankString = "Noob";
        this.XP = 0;
        this.greatestItem = "Nothing yet";
        this.dateJoined = DateTime.Now;

        // handle all lists
        this.collection = new List<Collectible>();
        Collectible defaultCollectible = new Collectible("Scavenger Spirit!", 0);// keep collection from being empty
        this.collection.Add(defaultCollectible);

        this.attachments = new List<Attachment>();
        Attachment starterGear = new Attachment("Starter Gear");
        this.attachments.Add(starterGear);

        // create and add all 10 levels
        this.availableLevels = new List<Level>();
        for (int i = 0; i < LevelLoader.instance.levelNames.Length; i++)
        {
            Level newLevel;
            newLevel = new Level(LevelLoader.instance.levelNames[i]);

            this.availableLevels.Add(newLevel);
        }

    }


}