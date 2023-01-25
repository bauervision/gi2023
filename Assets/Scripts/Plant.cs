using UnityEngine;
using System.Collections;

public class Plant : MonoBehaviour
{
    public float initialSize;
    public float maxSize;
    public float initialBloomSize;
    public float maxBloomSize;
    public float growFactor;


    public enum SeedType { Flower, Coin, Crystal, Tree, SpiderTrap };
    public SeedType myPlantType;



    void Start()
    {
        StartCoroutine(ScaleMain());
    }

    IEnumerator ScaleMain()
    {
        // we scale all axis, so they will have the same value, 
        // so we can work with a float instead of comparing vectors
        while (maxSize > transform.localScale.x)
        {
            transform.localScale += new Vector3(initialSize, initialSize, initialSize) * Time.deltaTime * growFactor;
            yield return null;
        }

        // at this point, the main flower has grown, trigger the bloom, IF this is a flower
        // otherwise figure out what we will do after the growth has completed
        switch ((int)myPlantType)
        {
            case 0: StartCoroutine(ScaleBloom()); break;
            case 1: StartCoroutine(SproutCoins()); break;
            case 2: StartCoroutine(SproutCrystals()); break;
            case 3: SproutTree(); break;
            case 4: handleSpiderTrap(); break;
        }

    }

    IEnumerator ScaleBloom()
    {
        // start the bloom
        while (maxBloomSize > transform.GetChild(0).localScale.x)
        {
            // first fire off the scale
            transform.GetChild(0).localScale += new Vector3(initialBloomSize, initialBloomSize, initialBloomSize) * Time.deltaTime * growFactor;

            yield return null;
        }

        // trigger whatever happens after it blooms...if anything
    }

    IEnumerator SproutCoins()
    {
        yield return new WaitForSeconds(2f);
        print("Sprout the coins!");

    }

    IEnumerator SproutCrystals()
    {
        yield return new WaitForSeconds(1f);
        print("Sprout the crystals!");

    }

    private void SproutTree()
    {
        print("Sprout the Tree!");
    }

    private void handleSpiderTrap()
    {
        print("handle the spider");
    }

}