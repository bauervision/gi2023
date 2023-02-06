using System.Collections;
using System.Collections.Generic;
using Galo.Interactions;
using UnityEngine;

namespace Galo
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager instance;
        AudioSource audioSource;
        public AudioClip islandBlood;
        public AudioClip islandHeart;
        public AudioClip bonusCrystal;
        public AudioClip collectedOar;
        public AudioClip collectedBoat;
        public AudioClip performanceBonus;
        public AudioClip changeCharacter;
        public AudioClip notificationOn;
        public AudioClip notificationOff;
        public AudioClip punchingBag;
        public AudioClip deathSound;

        private void Awake()
        {
            instance = this;
            audioSource = this.GetComponent<AudioSource>();
        }

        public void PlayCollectionSound(ItemType soundType)
        {
            AudioClip collectionSound;
            switch (soundType)
            {
                case ItemType.IslandHeart:
                    {
                        collectionSound = islandHeart;
                        // make sure we dont trigger unless we have gotten the blood first
                        if (LevelManager.instance.foundBlood)
                            LevelManager.instance.foundHeart = true;//trigger complete
                        break;
                    }
                case ItemType.IslandBlood:
                    {
                        collectionSound = islandBlood;
                        LevelManager.instance.foundBlood = true;
                        break;
                    }
                case ItemType.Crystal:
                    {
                        collectionSound = bonusCrystal;
                        LevelManager.instance.foundCrystal++;//add to the crystal count
                        break;
                    }
                case ItemType.Oar:
                    {
                        collectionSound = collectedOar;
                        LevelManager.instance.playerHasTheOar = true;//trigger complete
                        break;
                    }
                case ItemType.Boat:
                    {
                        collectionSound = collectedBoat;
                        break;
                    }
                default: collectionSound = islandHeart; break;
            }

            audioSource.PlayOneShot(collectionSound);
        }

        public void PlayLevelEndSound() { audioSource.PlayOneShot(islandHeart); }

        public void PlayPerformanceBonus() { audioSource.PlayOneShot(performanceBonus); }
        public void PlayChange() { audioSource.PlayOneShot(changeCharacter); }
        public void PlayNotificationOn() { audioSource.PlayOneShot(notificationOn); }
        public void PlayNotificationOff() { audioSource.PlayOneShot(notificationOff); }
        public void PlayPunchingBag() { audioSource.PlayOneShot(punchingBag); }
        public void PlayDeathSound() { audioSource.PlayOneShot(deathSound); }


    }
}
