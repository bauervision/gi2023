using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Galo.Interactions
{
    public class RankingMonitor : MonoBehaviour
    {
        public static RankingMonitor instance;
        public GameObject levelUpParticleSystem;

        private void Awake() { instance = this; }

        // Start is called before the first frame update
        void Start() { levelUpParticleSystem.SetActive(false); }



        public void TriggerLevelUp(string newRank)
        {
            levelUpParticleSystem.SetActive(true);
            NotificationManager.instance.DisplayNotificationAutoHide($"Leveled Up! {newRank}!");
        }
    }
}
