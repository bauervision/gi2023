using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace Galo
{
    public enum GaloLevelNames { Tutorial, IsleOfNoob, MountEgo, FrigidForest, Level4, Level5, Level6, Level7, Level8, Level9 };


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
        public GaloLevelNames levelName;

        // is this level available to play?
        public bool available;

        // highest score in this level
        public int highScore;

        public LevelTime levelTime;

        public bool hasCompleted;
        // have we found the 2 bonus crystals?
        public bool foundCrystal1;
        public bool foundCrystal2;
        public LevelPersistentData levelPersistentData;



        public GaloLevel(GaloLevelNames levelName)
        {
            this.available = levelName != GaloLevelNames.Tutorial ? false : true;// first level is enabled by default
            this.highScore = 0;
            this.hasCompleted = false;
            this.foundCrystal1 = false;
            this.foundCrystal2 = false;
            this.levelName = levelName;
            this.levelTime = new LevelTime();

        }

    }
    [System.Serializable]
    public class LevelPersistentData
    {
        public TutorialLevel tutorialLevel;
        public IsleOfNoobLevel isleOfNoobLevel;
        public MountEgoLevel mountEgoLevel;
    }

    [System.Serializable]
    public class TutorialLevel
    {
        public bool shownInitialFindBlood;
        public bool shownInitialZipline;
    }

    [System.Serializable]
    public class IsleOfNoobLevel
    {
        public bool shownInitialFindBlood;
        public bool shownInitialZipline;
    }

    [System.Serializable]
    public class MountEgoLevel
    {
        public bool shownInitialFindBlood;
        public bool shownInitialZipline;
    }
}