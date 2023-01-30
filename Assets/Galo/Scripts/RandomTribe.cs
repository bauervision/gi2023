using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class RandomTribe : MonoBehaviour
{
    public TextMeshProUGUI randomTribeData;
    string[] alonTribeBoys = new string[] { "Jerome (Alon)", "Willis (Alon)", "Dion (Alon)" };
    string[] alonTribeGirls = new string[] { "Felicia (Alon)", "Monique (Alon)", "Charice (Alon)" };
    string[] lunaTribeBoys = new string[] { "Roberto (Luna)", "Manny (Luna)", "Juan (Luna)" };
    string[] lunaTribeGirls = new string[] { "Teresa (Luna)", "Maria (Luna)", "Zoe (Luna)" };
    string[] creyTribeBoys = new string[] { "Caden (Crey)", "Miles (Crey)", "Luke (Crey)" };
    string[] creyTribeGirls = new string[] { "Jona (Crey)", "Everleigh (Crey)", "Bella (Crey)" };
    string[] mingTribeBoys = new string[] { "Ren (Ming)", "Hekima (Ming)", "Kage (Ming)" };
    string[] mingTribeGirls = new string[] { "Priya (Ming)", "Kimiko (Ming)", "Taya (Ming)" };

    // Start is called before the first frame update
    void Start()
    {

    }

    public void RandomizeTribe()
    {
        //generate 1 random boy and girl from each tribe
        string alonBoy = alonTribeBoys[UnityEngine.Random.Range(0, 3)];
        string alonGirl = alonTribeGirls[UnityEngine.Random.Range(0, 3)];
        string lunaBoy = lunaTribeBoys[UnityEngine.Random.Range(0, 3)];
        string lunaGirl = lunaTribeGirls[UnityEngine.Random.Range(0, 3)];
        string creyBoy = creyTribeBoys[UnityEngine.Random.Range(0, 3)];
        string creyGirl = creyTribeGirls[UnityEngine.Random.Range(0, 3)];
        string mingBoy = mingTribeBoys[UnityEngine.Random.Range(0, 3)];
        string mingGirl = mingTribeGirls[UnityEngine.Random.Range(0, 3)];

        // now put these into new array
        string[] finalBoys = new string[] { alonBoy, lunaBoy, creyBoy, mingBoy };
        string[] finalGirls = new string[] { alonGirl, lunaGirl, creyGirl, mingGirl };

        // shuffle them
        string[] shuffledBoys = ShuffleGenderArray(finalBoys);
        string[] shuffledGirls = ShuffleGenderArray(finalGirls);

        // now set the string
        randomTribeData.text =
       shuffledBoys[0] + Environment.NewLine +
        shuffledGirls[0] + Environment.NewLine +
        shuffledBoys[1] + Environment.NewLine +
        shuffledGirls[1] + Environment.NewLine +
        shuffledBoys[2] + Environment.NewLine +
         shuffledGirls[2] + Environment.NewLine +
       shuffledBoys[3] + Environment.NewLine +
        shuffledGirls[3];

    }

    public string[] ShuffleGenderArray(string[] genderArray)
    {
        for (int i = 0; i < genderArray.Length - 1; i++)
        {
            int rnd = UnityEngine.Random.Range(i, genderArray.Length);
            string currentmember = genderArray[rnd];
            genderArray[rnd] = genderArray[i];
            genderArray[i] = currentmember;
        }
        return genderArray;
    }
}

