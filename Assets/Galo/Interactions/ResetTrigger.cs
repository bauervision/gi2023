using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Galo.Interactions
{
    public class ResetTrigger : MonoBehaviour
    {
        public void ResetAnimatorTrigger()
        {
            AnimationManager.ResetTrigger();
        }
    }
}
