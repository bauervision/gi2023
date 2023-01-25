using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Level
{
    // is this level available to play?
    public bool available;

    // highest score in this level
    public int highScore;

    public int? bestTimeMinutes;
    public int? bestTimeSeconds;

    public bool hasCompleted;
    // have we found the 2 bonus crystals?
    public bool foundCrystal1;
    public bool foundCrystal2;
    // name of the level
    public string name;

    public Level(string levelName)
    {
        this.available = levelName != "Tutorial" ? false : true;// first level is enabled by default
        this.highScore = 0;
        this.hasCompleted = false;
        this.foundCrystal1 = false;
        this.foundCrystal2 = false;
        this.name = levelName;
        this.bestTimeMinutes = null;
        this.bestTimeSeconds = null;
    }

}
