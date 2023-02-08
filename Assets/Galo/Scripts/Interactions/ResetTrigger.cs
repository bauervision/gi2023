using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Galo
{
    public class ResetTrigger : MonoBehaviour
    {
        public void ResetAnimatorTrigger()
        {
            AnimationManager.ResetTrigger();
        }
    }
}
