using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Galo
{
    public class ObjectiveManager : MonoBehaviour
    {
        public static ObjectiveManager instance;

        public string[] levelObjectives;
        int objectiveIndex = 0;

        private void Awake() { instance = this; }

        public void SetNextObjective() { objectiveIndex++; }

        public string GetCurrentObjective() { return levelObjectives[objectiveIndex]; }

    }
}


