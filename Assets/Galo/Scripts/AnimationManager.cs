using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Galo
{

    [DefaultExecutionOrder(-1)]
    public class AnimationManager : MonoBehaviour
    {
        public static AnimationManager instance;

        public Animator notificationAnimator;
        public Animator pillarAnimator;
        public Animator[] infoPlinthAnimators;
        public Animator familyAnimator;
        public Animator randomAnimator;
        public Animator tallyPageAnimator;
        public GameObject[] families;

        private void Awake() { instance = this; }


        public void NotificationsOn(bool playSound)
        {
            if (notificationAnimator != null)
            {
                notificationAnimator.SetBool("On", true);
                if (playSound)
                    AudioManager.instance.PlayNotificationOn();
            }
        }
        public void NotificationsOff(bool playSound)
        {
            if (notificationAnimator != null)
            {
                notificationAnimator.SetBool("On", false);
                if (playSound)
                    AudioManager.instance.PlayNotificationOff();
            }
        }

        public void PillarTriggered()
        {
            if (pillarAnimator != null)
                pillarAnimator.SetBool("Play", true);


        }

        public void PlinthOn(int plinthIndex)
        {
            if (infoPlinthAnimators[plinthIndex] != null)
                infoPlinthAnimators[plinthIndex].SetBool("On", true);
        }

        public void PlinthOff(int plinthIndex)
        {
            if (infoPlinthAnimators[plinthIndex] != null)
                infoPlinthAnimators[plinthIndex].SetBool("On", false);
        }

        // handle family selections
        int _familyIndex;

        /// <summary>
        /// Called when user first hits the choose family button, make sure we start with Alon family
        /// </summary>
        public void FamilyAppearFirst()
        {


            // Make sure we start with Alon first
            if (families[0] != null)
            {
                families[0].SetActive(true);
                familyAnimator.SetTrigger("Appear");
            }
            TribeManager.instance.SetFamily(0);

        }

        void HideAllFamiles()
        {
            if (families.Length > 0)
                foreach (GameObject family in families)
                {
                    family.SetActive(false);
                    family.GetComponent<Image>().color = new Color(1, 1, 1, 0);
                }
        }

        public void FamilyAppearNext()
        {
            // handle looper
            _familyIndex++;
            if (_familyIndex > 3)
                _familyIndex = 0;

            // handle which family is displayed
            HideAllFamiles();
            if (families[_familyIndex] != null)
            {
                families[_familyIndex].SetActive(true);
                familyAnimator.SetTrigger("Appear");
                TribeManager.instance.SetFamily(_familyIndex);
            }
        }

        public void FamilyAppearPrev()
        {

            // handle looper
            _familyIndex--;
            if (_familyIndex < 0)
                _familyIndex = 3;

            // handle which family is displayed
            HideAllFamiles();
            if (families[_familyIndex] != null)
            {
                families[_familyIndex].SetActive(true);
                familyAnimator.SetTrigger("Appear");
                TribeManager.instance.SetFamily(_familyIndex);
            }


        }

        public void ChangeTribe()
        {
            randomAnimator.SetTrigger("Change");
        }

        public static void ResetTrigger()
        {
            if (instance.familyAnimator != null)
                instance.familyAnimator.ResetTrigger("Appear");

            if (instance.randomAnimator != null)
                instance.randomAnimator.ResetTrigger("Change");


        }

        public void NextPageTally()
        {
            if (tallyPageAnimator != null)
                tallyPageAnimator.SetTrigger("Next");
        }
        public void PrevPageTally()
        {
            if (tallyPageAnimator != null)
                tallyPageAnimator.SetTrigger("Prev");
        }
    }
}
