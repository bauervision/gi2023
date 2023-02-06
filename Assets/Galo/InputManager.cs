using System.Collections;
using System.Collections.Generic;
using Galo.Characters;
using UnityEngine;

namespace Galo
{
    public class InputManager : MonoBehaviour
    {

        // Update is called once per frame
        void Update()
        {
            if (ControlFreak2.CF2Input.GetKeyDown(KeyCode.G)) { UIManager.instance.ShowEndOfLevel(); }
            if (ControlFreak2.CF2Input.GetKeyDown(KeyCode.L)) { ToggleWalk.instance.ToggleWalkState(); }
            if (ControlFreak2.CF2Input.GetKeyDown(KeyCode.P)) { PauseManager.instance.ToggleInGamePause(); }
            if (ControlFreak2.CF2Input.GetKeyDown(KeyCode.U)) { LevelManager.instance.ReplayLevel(); }

            if (ControlFreak2.CF2Input.GetKeyUp(KeyCode.X)) { ExpManager.UpdateXP(500); }

            if (ControlFreak2.CF2Input.GetKeyDown(KeyCode.N))
            {
                AnimationManager.instance.NotificationsOff(true);
                NotificationManager.instance.HideAllUIImages();
            }


        }
    }
}
