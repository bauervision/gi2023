using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Galo
{
    public enum PauseItemType { IslandHeart, Zipline };

    public class PauseTrigger : GaloTrigger
    {
        public PauseItemType pauseItemType;

        public override void Start()
        {
            // run our check to see if we will even be enabled
            GaloLevelNames currentLevel = Initializer.instance.currentLevelName;
            switch (currentLevel)
            {
                case GaloLevelNames.IsleOfNoob:
                    {
                        if (EventDataManager.instance)
                            if (EventDataManager.instance.currentLevel.galoLevel.levelPersistentData.isleOfNoobLevel.shownInitialFindBlood && pauseItemType == PauseItemType.IslandHeart)
                                this.gameObject.SetActive(false);
                        break;
                    }
                default://Tutorial Level
                    {
                        if (EventDataManager.instance)
                            if (EventDataManager.instance.currentLevel.galoLevel.levelPersistentData.tutorialLevel.shownInitialFindBlood && pauseItemType == PauseItemType.IslandHeart)
                                this.gameObject.SetActive(false);

                        if (EventDataManager.instance)
                            if (EventDataManager.instance.currentLevel.galoLevel.levelPersistentData.tutorialLevel.shownInitialZipline && pauseItemType == PauseItemType.Zipline)
                                this.gameObject.SetActive(false);
                        break;
                    }
            }

        }

        // public override void OnTriggerEnter(Collider other)
        // {
        //     if (other.gameObject.tag == "Player")
        //     {
        //         // we need to check to see if we
        //         switch (pauseItemType)
        //         {
        //             case PauseItemType.IslandHeart:
        //                 {
        //                     break;
        //                 }
        //             default:
        //                 {
        //                     break;
        //                 }
        //         }



        //     }
        // }
    }
}
