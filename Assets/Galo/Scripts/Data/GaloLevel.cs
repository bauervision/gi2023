using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace Galo
{
    [System.Serializable]
    public class LevelTime
    {
        public int minutes;
        public int seconds;
        public LevelTime()
        {
            this.minutes = -1;
            this.seconds = -1;
        }
    }

    [System.Serializable]
    public class GaloLevel
    {
        // is this level available to play?
        public bool available;

        // highest score in this level
        public int highScore;

        public LevelTime levelTime;

        public bool hasCompleted;
        // have we found the 2 bonus crystals?
        public bool foundCrystal1;
        public bool foundCrystal2;
        // name of the level
        public string name;

        public GaloLevel(string levelName)
        {
            this.available = levelName != "Tutorial" ? false : true;// first level is enabled by default
            this.highScore = 0;
            this.hasCompleted = false;
            this.foundCrystal1 = false;
            this.foundCrystal2 = false;
            this.name = levelName;
            this.levelTime = new LevelTime();
        }

    }
}