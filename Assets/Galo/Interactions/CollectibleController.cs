using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Galo.Interactions
{
    public class CollectibleController : MonoBehaviour
    {
        GameObject[] collectionList;
        // Start is called before the first frame update
        void Start()
        {
            // make sure we havent already found this collection
            if (!HasPlayerFoundMe())
            {
                // player hasnt found this collectible type yet so let's randomly hide each children
                for (int i = 0; i < this.transform.childCount; i++)
                    this.transform.GetChild(i).gameObject.SetActive(false);
                // unhide the random one
                int randomIndex = (int)Random.Range(0, this.transform.childCount);
                this.transform.GetChild(randomIndex).gameObject.SetActive(true);
            }
        }

        bool HasPlayerFoundMe() { return this.transform.GetChild(0).GetComponent<Collectible>().hiddenFromStart; }


    }
}
