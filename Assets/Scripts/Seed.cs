using UnityEngine;
using System.Collections;

public class Seed : MonoBehaviour
{
    AudioSource _audioSource;
    public AudioClip _audioClip;

    public enum SeedType { Flower, Coin, Crystal, Tree, SpiderTrap };
    public SeedType mySeedType;
    public GameObject plant;

    public Transform seedTransform;
    public Terrain t;

    public int posX;
    public int posZ;
    public float[] textureValues;

    void Start()
    {
        // locate required items
        _audioSource = GetComponent<AudioSource>();

        t = Terrain.activeTerrain;
        seedTransform = gameObject.transform;
    }


    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "PW - Nature")
            GetTerrainTexture();
    }

    private void HandleSeedPlanting()
    {
        // plant the flower that will bloom
        Instantiate(plant, transform.position, Quaternion.identity);
        _audioSource.PlayOneShot(_audioClip);
        //disable the mesh
        gameObject.GetComponentInChildren<MeshRenderer>().enabled = false;
        gameObject.GetComponent<SphereCollider>().enabled = false;


        // update our XP
        // switch ((int)mySeedType)
        // {
        //     case 0: ExpManager.UpdateXP(100); break;// flower sprouted
        //     case 1: ExpManager.UpdateXP(50); break;// coins sprouted
        //     case 2: ExpManager.UpdateXP(1000); break;// crystals sprouted
        //     case 3: ExpManager.UpdateXP(2500); break;// tree sprouted
        //     case 4: ExpManager.UpdateXP(250); break;// spidertrap sprouted
        // }
        // destroy the seed
        Destroy(gameObject, _audioClip.length);

    }

    public void GetTerrainTexture()
    {
        ConvertPosition(seedTransform.position);
        CheckTexture();

        // check to see what type of seed we are
        switch ((int)mySeedType)
        {
            case 0://Flower seed
                   // hit good soil
                if (textureValues[1] > 0)
                    HandleSeedPlanting();
                else
                    StartCoroutine(WaitToDestroy());

                break;
            case 1://Coin seed
                   // hit gravel path
                if (textureValues[3] > 0)
                    HandleSeedPlanting();
                else
                    StartCoroutine(WaitToDestroy());

                break;
            case 2://Crystal seed
                if (textureValues[0] > 0)// hit Mountain Rock
                    HandleSeedPlanting();
                else
                    StartCoroutine(WaitToDestroy());

                break;
            case 3: // Tree seed
                if (textureValues[1] > 0)
                    HandleSeedPlanting();
                else
                    StartCoroutine(WaitToDestroy());

                break;
            case 4: // SpiderTrap seed
                HandleSeedPlanting();
                break;

        }

    }

    void ConvertPosition(Vector3 seedPosition)
    {
        Vector3 terrainPosition = seedPosition - t.transform.position;

        Vector3 mapPosition = new Vector3
        (terrainPosition.x / t.terrainData.size.x, 0,
        terrainPosition.z / t.terrainData.size.z);

        float xCoord = mapPosition.x * t.terrainData.alphamapWidth;
        float zCoord = mapPosition.z * t.terrainData.alphamapHeight;

        posX = (int)xCoord;
        posZ = (int)zCoord;
    }

    void CheckTexture()
    {
        float[,,] aMap = t.terrainData.GetAlphamaps(posX, posZ, 1, 1);
        textureValues[0] = aMap[0, 0, 0];
        textureValues[1] = aMap[0, 0, 1];
        textureValues[2] = aMap[0, 0, 2];
        textureValues[3] = aMap[0, 0, 3];



    }




    IEnumerator WaitToDestroy()
    {
        yield return new WaitForSeconds(4f);
        Destroy(gameObject);
    }
}