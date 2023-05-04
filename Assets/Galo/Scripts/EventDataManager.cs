using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Galo
{
    public class EventDataManager : MonoBehaviour
    {
        public static EventDataManager instance;

        private void Awake() { instance = this; }


        #region Level Specific Settings
        #region Tutorial Level
        public void Tutorial_BloodPause()
        {
            if (DataManager.instance)
            {
                DataManager.instance.playerData.levelPersistentData.tutorialLevel.shownInitialFindBlood = true;
                DataSaver.SaveFile();
            }
        }

        public void Tutorial_Zipline()
        {
            if (DataManager.instance)
            {
                DataManager.instance.playerData.levelPersistentData.tutorialLevel.shownInitialZipline = true;
                DataSaver.SaveFile();
            }
        }

        public void Tutorial_FoundStar()
        {
            if (DataManager.instance)
            {
                DataManager.instance.playerData.levelPersistentData.tutorialLevel.foundStar = true;
                DataSaver.SaveFile();
            }
        }

        #endregion
        #endregion
    }
}
