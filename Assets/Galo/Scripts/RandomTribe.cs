using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;
using System.Linq;

namespace Galo
{
    public class RandomTribe : MonoBehaviour
    {
        public static RandomTribe instance;

        // handle the actual character mesh assignments
        public GameObject[] chosenTribe;
        public GameObject[] alonBoyCharacters;
        public GameObject[] alonGirlCharacters;
        public GameObject[] lunaBoyCharacters;
        public GameObject[] lunaGirlCharacters;
        public GameObject[] creyBoyCharacters;
        public GameObject[] creyGirlCharacters;
        public GameObject[] mingBoyCharacters;
        public GameObject[] mingGirlCharacters;


        public Image[] randomTribeImages;
        public Sprite[] alonBoys;
        public Sprite[] alonGirls;
        public Sprite[] lunaBoys;
        public Sprite[] lunaGirls;
        public Sprite[] creyBoys;
        public Sprite[] creyGirls;
        public Sprite[] mingBoys;
        public Sprite[] mingGirls;

        public Sprite[] testBoyArrayPreShuffle;
        public Sprite[] testBoyArrayShuffled;

        private void Awake() { instance = this; }


        public void RandomizeTribe()
        {
            //generate 1 random boy and girl from each tribe
            int alonBoy = UnityEngine.Random.Range(0, 3);
            int alonGirl = UnityEngine.Random.Range(0, 3);
            int lunaBoy = UnityEngine.Random.Range(0, 3);
            int lunaGirl = UnityEngine.Random.Range(0, 3);
            int creyBoy = UnityEngine.Random.Range(0, 3);
            int creyGirl = UnityEngine.Random.Range(0, 3);
            int mingBoy = UnityEngine.Random.Range(0, 3);
            int mingGirl = UnityEngine.Random.Range(0, 3);

            // set and store the randomly chosen boy sprites and character models
            List<Sprite> finalBoys = new();
            finalBoys.Add(alonBoys[alonBoy]);
            finalBoys.Add(lunaBoys[lunaBoy]);
            finalBoys.Add(creyBoys[creyBoy]);
            finalBoys.Add(mingBoys[mingBoy]);

            List<GameObject> finalBoyMeshes = new();
            finalBoyMeshes.Add(alonBoyCharacters[alonBoy]);
            finalBoyMeshes.Add(lunaBoyCharacters[lunaBoy]);
            finalBoyMeshes.Add(creyBoyCharacters[creyBoy]);
            finalBoyMeshes.Add(mingBoyCharacters[mingBoy]);

            // set and store the randomly chosen girl sprites and character models
            List<Sprite> finalGirls = new();
            finalGirls.Add(alonGirls[alonGirl]);
            finalGirls.Add(lunaGirls[lunaGirl]);
            finalGirls.Add(creyGirls[creyGirl]);
            finalGirls.Add(mingGirls[mingGirl]);

            List<GameObject> finalGirlMeshes = new();
            finalGirlMeshes.Add(alonGirlCharacters[alonGirl]);
            finalGirlMeshes.Add(lunaGirlCharacters[lunaGirl]);
            finalGirlMeshes.Add(creyGirlCharacters[creyGirl]);
            finalGirlMeshes.Add(mingGirlCharacters[mingGirl]);

            // merge both into one
            List<Sprite> finalTribe = finalBoys.Union(finalGirls).ToList();
            List<GameObject> finalTribeCharacters = finalBoyMeshes.Union(finalGirlMeshes).ToList();

            List<Sprite> tribe = Shuffle(finalTribe, finalTribeCharacters);

            // now set the sprites
            randomTribeImages[0].sprite = tribe.ElementAt(0);
            randomTribeImages[1].sprite = tribe.ElementAt(1);
            randomTribeImages[2].sprite = tribe.ElementAt(2);
            randomTribeImages[3].sprite = tribe.ElementAt(3);
            randomTribeImages[4].sprite = tribe.ElementAt(4);
            randomTribeImages[5].sprite = tribe.ElementAt(5);
            randomTribeImages[6].sprite = tribe.ElementAt(6);
            randomTribeImages[7].sprite = tribe.ElementAt(7);

        }

        public List<Sprite> Shuffle(List<Sprite> gender, List<GameObject> characters)
        {
            System.Random random = new System.Random();
            List<Sprite> shuffledList = gender;
            List<GameObject> shuffledCharacters = characters;
            int n = shuffledList.Count;
            while (n > 1)
            {
                int k = (random.Next(0, n) % n);
                n--;
                Sprite value = shuffledList[k];
                shuffledList[k] = shuffledList[n];
                shuffledList[n] = value;

                GameObject value2 = shuffledCharacters[k];
                shuffledCharacters[k] = shuffledCharacters[n];
                shuffledCharacters[n] = value2;
                characters = shuffledCharacters;

                chosenTribe = characters.ToArray();
            }

            return shuffledList;
        }


        public void SetFamily(int familyIndex)
        {
            switch (familyIndex)
            {
                case 1: { chosenTribe = lunaBoyCharacters.Union(lunaGirlCharacters).ToArray(); break; }
                case 2: { chosenTribe = creyBoyCharacters.Union(creyGirlCharacters).ToArray(); break; }
                case 3: { chosenTribe = mingBoyCharacters.Union(mingGirlCharacters).ToArray(); break; }
                default: { chosenTribe = alonBoyCharacters.Union(alonGirlCharacters).ToArray(); break; }
            }

        }

        public void SelectTribe()
        {
            // we've chosen our tribe, save the character choices, and their placement for later
            DataManager.instance.currentTribe = chosenTribe;


        }
    }
}
