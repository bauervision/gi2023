using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Galo
{
    public class UIManager : MonoBehaviour
    {

        public static UIManager instance;

        public UI_Menus menus;

        private void Awake() { instance = this; }
        public UnityEvent toggleMenu = new UnityEvent();
        public GameObject AllVegetation;
        public Sprite walkSprite, runSprite, runnerTypeSprite, fighterTypeSprite, climberTypeSprite;
        public Image playerTypeImage;

        // Start is called before the first frame update
        void Start()
        {
            UpdateProfilePage();
        }

        public Sprite GetWalkSprite() { return walkSprite; }
        public Sprite GetRunSprite() { return runSprite; }

        public void ChangePlayerTypeSprite(PlayerType currentType)
        {
            Sprite currentSprite;
            switch (currentType)
            {
                case PlayerType.CLIMBER: { currentSprite = climberTypeSprite; break; }
                case PlayerType.FIGHTER: { currentSprite = fighterTypeSprite; break; }
                default: { currentSprite = runnerTypeSprite; break; }
            }
            playerTypeImage.sprite = currentSprite;
        }

        public void ShowEndOfLevel()
        {
            menus.userMenu.SetActive(!menus.userMenu.activeInHierarchy);
        }

        public void HideThisAfterSeconds(GameObject objectToHide)
        {
            objectToHide.SetActive(true);
            StartCoroutine(WaitForSeconds(objectToHide));

        }
        public IEnumerator WaitForSeconds(GameObject objectToHide)
        {
            yield return new WaitForSeconds(2);
            objectToHide.SetActive(false);
        }

        public void ResetData()
        {
            DataSaver.ClearPlayerData();
            bool resetSuccess = DataSaver.CheckResetSuccess();
            string message = resetSuccess ? " Was Successful!" : " Did not complete";
            NotificationManager.instance.DisplayNotification("Player Data Reset:" + message);
        }

        public void ChangeQualitySetting(int value)
        {
            QualitySettings.SetQualityLevel(value, false);
            AllVegetation.SetActive(value != 0);
        }

        public void UpdateProfilePage()
        {
            menus.collectionText.text = "";
            menus.collectionTally.text = "";
            int collectibleTally = 0;
            // now populate it with the items
            if (DataManager.instance)
            {
                if (DataManager.instance.playerData.collection.Count > 0)
                {
                    // for each collectible we have, populate the collection text
                    for (int i = 0; i < DataManager.instance.playerData.collection.Count; i++)
                    {
                        menus.collectionText.text += DataManager.instance.playerData.collection[i].name;
                        collectibleTally += DataManager.instance.playerData.collection[i].value;
                        // add the comma if we need to
                        if (i + 1 < DataManager.instance.playerData.collection.Count)
                            menus.collectionText.text += ", ";
                    }
                }
                else
                    menus.collectionText.text = $"There are tons of items scattered throughout the game! The main objective is to unlock levels and find as many collectibles as possible! Good Luck!";

                if (collectibleTally > 0)
                    menus.collectionTally.text = "+" + collectibleTally + " XP!";

                // now handle profile data
                GaloPlayerData player = DataManager.instance.playerData;
                string greatestFind = FindGreatest(player.collection);
                double nextRank = ExpManager.instance.GetNextLevel() - player.XP;
                menus.profileText.text = $"{player.currentPlayer}\n{player.XP}\n{player.rankString}\n{greatestFind}\n{nextRank}";
            }


        }

        string FindGreatest(List<GaloCollectible> collection)
        {
            if (collection.Count > 0)
            {
                List<GaloCollectible> sortedList = collection.OrderBy(x => x.value).ToList();
                GaloCollectible best = sortedList.First();
                return best.name;
            }
            else
                return "Nothing Yet!";

        }


        public void SetCurrentCharacterName(string name) { menus.characterName.text = name; }
        public void SetCurrentCharacterAbility(string name) { menus.ability.text = name; }
    }
}
