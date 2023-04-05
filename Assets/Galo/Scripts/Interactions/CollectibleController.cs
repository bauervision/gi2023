using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Galo
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

        /// <summary>
        /// Check the children to see if they are Collectibles, and if they are not, then we just want to randomly hide the kids
        /// </summary>
        /// <returns></returns>
        bool HasPlayerFoundMe()
        {
            // if the first kid is a collectible, than they all are 
            if (this.transform.GetChild(0).GetComponent<Collectible>())
                return this.transform.GetChild(0).GetComponent<Collectible>().hiddenFromStart;
            // ot
            return false;
        }


    }
}
