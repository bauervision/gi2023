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

        Sprite _walkSprite, _runSprite;
        private void Awake()
        {
            instance = this;
            controller = this.GetComponent<vThirdPersonController>();
        }

        private void Start()
        {
            walkButton = GameObject.Find("Button-WalkToggle-Sprite").GetComponent<TouchButtonSpriteAnimator>();
            _walkSprite = UIManager.instance.GetWalkSprite();
            _runSprite = UIManager.instance.GetRunSprite();
        }



        public void ToggleWalkState()
        {
            controller.alwaysWalkByDefault = !controller.alwaysWalkByDefault;
            walkButton.spriteNeutral.sprite = controller.alwaysWalkByDefault ? _runSprite : _walkSprite;
        }
    }
}
