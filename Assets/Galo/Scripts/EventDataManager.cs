using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Galo
{
    public class EventDataManager : MonoBehaviour
    {
        public static EventDataManager instance;
        public ScriptableLevel currentLevel;

        private void Awake() { instance = this; }


        #region Level Specific Settings
        #region Tutorial Level
        public void Tutorial_BloodPause()
        {
            currentLevel.galoLevel.levelPersistentData.tutorialLevel.shownInitialFindBlood = true;
        }
        public void Tutorial_Zipline()
        {
            currentLevel.galoLevel.levelPersistentData.tutorialLevel.shownInitialZipline = true;
        }

        #endregion
        #endregion
    }
}
