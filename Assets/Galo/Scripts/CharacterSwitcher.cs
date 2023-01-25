using System.Collections;
using System.Collections.Generic;
using Invector.vCamera;
using Invector.vCharacterController;
using Invector.vCharacterController.vActions;
using Invector.vMelee;
using UnityEngine;

namespace Galo
{

    public class CharacterSwitcher : MonoBehaviour
    {
        public static CharacterSwitcher instance;
        public GameObject _spawnPoint;
        public GameObject _playerModelParent;
        public GameObject activePlayerModel;
        public GameObject _CadenMesh, _MilesMesh, _LukeMesh;

        public GameObject poof;
        public GameObject flash;
        public GameObject specialAttackButton;

        GameObject _tempMesh;
        public Animator controllerAnimator;

        vThirdPersonController _controller;
        CharacterData currentCharacterData;

        public string currentPlayerName;
        public PlayerType myCurrentPlayerType;

        private void Awake()
        {
            instance = this;
            _controller = FindObjectOfType<vThirdPersonController>();
            specialAttackButton = GameObject.Find("Button-Special");
        }

        // Start is called before the first frame update
        void Start()
        {
            _MilesMesh.SetActive(false);
            _LukeMesh.SetActive(false);
            poof.SetActive(false);
            flash.SetActive(false);
            IntitializeCharacterStats();

        }

        public void CallCharacterSwitch()
        {
            ChangeCharacterFromRoll();
            poof.SetActive(true);
        }

        public void StartFlash() { flash.SetActive(true); }

        void IntitializeCharacterStats()
        {

            currentCharacterData = activePlayerModel.GetComponent<CharacterData>();
            myCurrentPlayerType = currentCharacterData.playerType;

            //update the UI to show the new type
            UIManager.instance.ChangePlayerTypeSprite(myCurrentPlayerType);

            // first hide the special attack button unless this is Caden
            if (specialAttackButton == null)
                specialAttackButton = GameObject.Find("Button-Special");
            else
                specialAttackButton.SetActive(currentCharacterData.myName == "Caden");

            currentPlayerName = currentCharacterData.myName;
            if (UIManager.instance)
            {
                UIManager.instance.SetCurrentCharacterName(currentPlayerName);
                UIManager.instance.SetCurrentCharacterAbility(currentCharacterData.ability);
            }
            // now set the data
            _controller.freeSpeed.runningSpeed = currentCharacterData.runSpeed;
            _controller.jumpHeight = currentCharacterData.jumpHeight;
            GetComponent<vFreeClimb>().climbSpeed = currentCharacterData.climbSpeed;
            _controller.MultiJump = currentCharacterData.multiJump;
            _controller.currentMultiJump = 1;//reset

        }


        /// <summary>
        /// Called from Roll animation
        /// </summary>
        public void ChangeCharacterFromRoll()
        {
            //GetComponent<vMeleeManager>().enabled = false;
            //AudioManager.instance.PlayChange();
            //push to the last
            activePlayerModel.transform.SetAsLastSibling();
            activePlayerModel.SetActive(false);

            //find next
            _tempMesh = _playerModelParent.transform.GetChild(0).gameObject;
            _tempMesh.SetActive(true);

            // set the new model mesh
            activePlayerModel = _tempMesh;
            _tempMesh = null;
            // prepare for reinit
            controllerAnimator.SetInteger("AttackMax", currentCharacterData.attackMax);
            controllerAnimator.enabled = false;
            _controller.enabled = false;

            StartCoroutine(ReInit());

            currentCharacterData = GetComponent<CharacterData>();
            IntitializeCharacterStats();
        }


        IEnumerator ReInit()
        {
            controllerAnimator.enabled = true;

            yield return new WaitForSeconds(0.01f);
            activePlayerModel.SetActive(true);
            controllerAnimator.Rebind();
            _controller.enabled = true;
            _controller.UpdateMotor();
            controllerAnimator.SetInteger("AttackMax", currentCharacterData.attackMax);
            GameObject[] currentAttackObjects = currentCharacterData.myAttackObjects;
            GetComponent<vMeleeManager>().UpdateMembers(currentAttackObjects);

        }

    }
}