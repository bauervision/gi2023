using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Galo.Interactions
{
    public enum CoinType { Coin, Gem };
    public class Coin : MonoBehaviour
    {
        AudioSource _audioSource;
        public AudioClip _audioClip;

        public CoinType myType;
        public bool willSpin = true;
        public float SpinRate = 1.0f;
        public bool SpinX = false;
        public bool SpinY = false;
        public bool SpinZ = true;

        int myXpValue;

        void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            myXpValue = myType == CoinType.Coin ? 1 : 100;
        }


        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                _audioSource.PlayOneShot(_audioClip);
                EnableThisObject(false);
                ExpManager.UpdateXPCoin(myXpValue, myType);
                //StartCoroutine(ReplaceItem());// instead of destroying, begin timer so we can make it reappear
            }
        }



        IEnumerator ReplaceItem()
        {
            yield return new WaitForSeconds(myType == CoinType.Gem ? 340f : 120f);
            EnableThisObject(true);
        }


        private void EnableThisObject(bool state)
        {
            // disable the collider on this object right away
            gameObject.GetComponent<SphereCollider>().enabled = state;

            if (transform.childCount > 0)
                transform.GetChild(0).GetComponent<MeshRenderer>().enabled = state;
            else
                gameObject.GetComponent<MeshRenderer>().enabled = state;


        }

        private void Update()
        {
            if (willSpin)
            {
                var spinAmount = (SpinRate * 50) * Time.deltaTime;
                transform.Rotate(SpinX ? spinAmount : 0, SpinY ? spinAmount : 0, SpinZ ? spinAmount : 0);
            }
        }

        public void RegenerateMe()
        {
            EnableThisObject(true);
        }
    }
}