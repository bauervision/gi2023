using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Collectible
{
    public bool saved;
    public string name;

    public int collectibleRank;

    // probably a link to the mesh
    public Collectible(string newCollectibleName, int itemRanking)
    {
        this.saved = false;// items is set as not saved when first found, until the player makes it to the save point
        this.name = newCollectibleName;
        this.collectibleRank = itemRanking;
    }

}