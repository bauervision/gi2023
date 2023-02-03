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
        public string currentPlayerName;
        public PlayerType myCurrentPlayerType;

        public GameObject _spawnPoint;


        public GameObject poof;
        public GameObject flash;
        public GameObject specialAttackButton;


        public Animator controllerAnimator;

        vThirdPersonController _controller;
        public GameObject _playerModelParent;
        GameObject _activePlayerModel;
        GameObject _Primary, _Secondary, _Tertiary;

        CharacterData currentCharacterData;

        GameObject _tempMesh;



        private void Awake()
        {
            instance = this;
            _controller = FindObjectOfType<vThirdPersonController>();
            _playerModelParent = transform.GetChild(0).gameObject;
            specialAttackButton = GameObject.Find("Button-Special");
        }

        // Start is called before the first frame update
        void Start()
        {

            _Primary = _activePlayerModel = _playerModelParent.transform.GetChild(0).gameObject;
            _Primary.GetComponent<CharacterData>().playerType = PlayerType.RUNNER;

            _Secondary = _playerModelParent.transform.GetChild(1).gameObject;
            _Secondary.GetComponent<CharacterData>().playerType = PlayerType.FIGHTER;
            _Secondary.SetActive(false);

            _Tertiary = _playerModelParent.transform.GetChild(2).gameObject;
            _Tertiary.GetComponent<CharacterData>().playerType = PlayerType.CLIMBER;
            _Tertiary.SetActive(false);

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
            currentCharacterData = _activePlayerModel.GetComponent<CharacterData>();
            myCurrentPlayerType = currentCharacterData.playerType;

            //update the UI to show the new type
            if (UIManager.instance != null)
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
            _activePlayerModel.transform.SetAsLastSibling();
            _activePlayerModel.SetActive(false);

            //find next
            _tempMesh = _playerModelParent.transform.GetChild(0).gameObject;
            _tempMesh.SetActive(true);

            // set the new model mesh
            _activePlayerModel = _tempMesh;
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
            _activePlayerModel.SetActive(true);
            controllerAnimator.Rebind();
            _controller.enabled = true;
            _controller.UpdateMotor();
            controllerAnimator.SetInteger("AttackMax", currentCharacterData.attackMax);
            vMeleeAttackObject[] currentAttackObjects = currentCharacterData.myAttackObjects;
            GetComponent<vMeleeManager>().UpdateMembers(currentAttackObjects);

        }

    }
}