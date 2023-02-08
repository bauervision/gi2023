using System.Collections;
using System.Collections.Generic;
using ControlFreak2;
using Invector.vCharacterController;
using TMPro;
using UnityEngine;

namespace Galo
{
    public class ToggleWalk : MonoBehaviour
    {
        public static ToggleWalk instance;
        vThirdPersonController controller;
        public TouchButtonSpriteAnimator walkButton;

        GameObject walkToggleButton;

        Sprite _walkSprite, _runSprite;
        private void Awake()
        {
            instance = this;
            controller = this.GetComponent<vThirdPersonController>();

        }

        private void Start()
        {
            GameObject walkToggleSprite = GameObject.Find("Button-WalkToggle-Sprite");
            walkToggleButton = GameObject.Find("Button-WALKTOGGLE");

            if (walkToggleSprite != null)
                walkButton = walkToggleSprite.GetComponent<TouchButtonSpriteAnimator>();

            if (UIManager.instance != null)
            {
                _walkSprite = UIManager.instance.GetWalkSprite();
                _runSprite = UIManager.instance.GetRunSprite();
            }
        }

        public void Poisoned()
        {
            if (walkToggleButton)
                walkToggleButton.SetActive(false);

            controller.alwaysWalkByDefault = true;
        }

        public void Healed()
        {
            if (walkToggleButton)
                walkToggleButton.SetActive(true);

            if (controller.alwaysWalkByDefault)
                controller.alwaysWalkByDefault = false;
        }

        public void ToggleWalkState()
        {
            controller.alwaysWalkByDefault = !controller.alwaysWalkByDefault;
            walkButton.spriteNeutral.sprite = controller.alwaysWalkByDefault ? _runSprite : _walkSprite;
        }
    }
}
