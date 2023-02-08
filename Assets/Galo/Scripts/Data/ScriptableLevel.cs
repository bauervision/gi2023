using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Galo
{
    [CreateAssetMenu(fileName = "Galo Level", menuName = "New Galo Level", order = 0)]
    public class ScriptableLevel : ScriptableObject
    {
        public GaloLevel galoLevel;
    }
}
