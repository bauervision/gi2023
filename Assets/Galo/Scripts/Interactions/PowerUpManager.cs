using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Galo
{
    public class PowerUpManager : MonoBehaviour
    {
        public static PowerUpManager instance;

        public bool hasBreakingPower;
        private void Awake() { instance = this; }

        public void HasBreakingPower()
        {
            // we can only get it once per hit
            if (!hasBreakingPower)
            {
                hasBreakingPower = true;
                AudioManager.instance.PlayPowerup();
            }
        }
        public void LostBreakingPower() { hasBreakingPower = false; }
    }
}
