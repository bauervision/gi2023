using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Galo
{
    public class LevelScreenEvent : MonoBehaviour
    {
        public void LevelStart()
        {
            gameObject.SetActive(false);
            Initializer.instance.StartLevelEvents();
        }
    }
}
