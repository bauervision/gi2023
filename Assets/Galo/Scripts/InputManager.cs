using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ControlFreak2;

namespace Galo
{
    public class InputManager : MonoBehaviour
    {

        bool isCrouching = false;
        // Update is called once per frame
        void Update()
        {
            if (CF2Input.GetKeyDown(KeyCode.G)) { UIManager.instance.ShowEndOfLevel(); }
            if (CF2Input.GetKeyDown(KeyCode.L)) { ToggleWalk.instance.ToggleWalkState(); }
            if (CF2Input.GetKeyDown(KeyCode.P)) { PauseManager.instance.ToggleInGamePause(); }
            if (CF2Input.GetKeyDown(KeyCode.U)) { LevelManager.instance.ReplayLevel(); }

            if (CF2Input.GetKeyDown(KeyCode.C))
            {
                isCrouching = !isCrouching;
                GameObject.Find("Button-Crouch-Sprite").GetComponent<RectTransform>().rotation = new Quaternion(0, 0, isCrouching ? 180 : 0, 0);
            }
            if (CF2Input.GetKeyUp(KeyCode.X)) { ExpManager.UpdateXP(500); }

            if (CF2Input.GetKeyDown(KeyCode.N))
            {
                AnimationManager.instance.NotificationsOff(true);
                NotificationManager.instance.HideAllUIImages();
            }

            if (CF2Input.GetKeyDown(KeyCode.Y)) { UIManager.instance.ToggleSearchTouch(); }

            // if (CF2Input.touchCount > 0 && CF2Input.touches[0].phase == TouchPhase.Began)
            //     HandleClicks();

            // if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
            //     HandleClicks();

        }

        void HandleClicks()
        {
            NotificationManager.instance.DisplayNotificationAutoHide("Clicked Event fired");

            Ray ray = Camera.main.ScreenPointToRay(CF2Input.touches[0].position);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider != null)
                {
                    NotificationManager.instance.DisplayNotificationAutoHide("Collider: " + hit.collider.gameObject.name + " touched");
                    Collectible collectible = hit.collider.GetComponent<Collectible>();
                    if (collectible != null)
                    {
                        NotificationManager.instance.DisplayNotificationAutoHide("Collectible: touched");

                        //collectible.HandleClickEvent();
                    }

                }
            }
        }
    }
}
