using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// ? 2017 TheFlyingKeyboard and released under MIT License
// theflyingkeyboard.net

namespace Galo
{
    public class Launcher : MonoBehaviour
    {
        public GameObject[] seedsToLaunch;
        public static int seedIndex = 0;

        public Transform projectileSpawnPoint;
        public float projectileVelocity;
        public float timeBetweenShots;
        private Rigidbody projectile;

        private float timeBetweenShotsCounter;
        //private bool canShoot;
        // Use this for initialization
        void Start()
        {
            //canShoot = true;
            timeBetweenShotsCounter = timeBetweenShots;
        }

        // // Update is called once per frame
        // void Update()
        // {
        //     if (ControlFreak2.CF2Input.GetMouseButtonDown(0) && canShoot)
        //     {
        //         projectile = seedsToLaunch[seedIndex].GetComponent<Rigidbody>();
        //         Rigidbody bulletInstance = Instantiate(projectile, projectileSpawnPoint.position, Quaternion.Euler(new Vector3(0, 0, transform.localEulerAngles.z))) as Rigidbody;
        //         bulletInstance.GetComponent<Rigidbody>().AddForce(projectileSpawnPoint.right * projectileVelocity);
        //         InteractionManager.instance.currentSeedCount--;

        //         ExpManager.UpdateXP(25);//points for each seed thrown, regardless of type

        //         canShoot = false;
        //     }
        //     if (!canShoot)
        //     {
        //         timeBetweenShotsCounter -= Time.deltaTime;
        //         if (timeBetweenShotsCounter <= 0)
        //         {
        //             canShoot = true;
        //             timeBetweenShotsCounter = timeBetweenShots;
        //         }
        //     }
        // }
    }
}