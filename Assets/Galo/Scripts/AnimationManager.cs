using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Galo
{

    [DefaultExecutionOrder(-1)]
    public class AnimationManager : MonoBehaviour
    {
        public static AnimationManager instance;

        public Animator notificationAnimator;
        public Animator pillarAnimator;
        public Animator[] infoPlinthAnimators;

        private void Awake() { instance = this; }


        public void NotificationsOn(bool playSound)
        {
            notificationAnimator.SetBool("On", true);
            if (playSound)
                AudioManager.instance.PlayNotificationOn();
        }
        public void NotificationsOff(bool playSound)
        {
            notificationAnimator.SetBool("On", false);
            if (playSound)
                AudioManager.instance.PlayNotificationOff();
        }

        public void PillarTriggered()
        {
            pillarAnimator.SetBool("Play", true);
            // if (playSound)
            //     AudioManager.instance.PlayNotificationOff();
        }

        public void PlinthOn(int plinthIndex)
        {
            infoPlinthAnimators[plinthIndex].SetBool("On", true);
        }

        public void PlinthOff(int plinthIndex)
        {
            infoPlinthAnimators[plinthIndex].SetBool("On", false);
        }
    }
}
