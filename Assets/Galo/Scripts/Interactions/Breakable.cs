using UnityEngine;
using System.Collections;
using Invector;

namespace Galo
{

    public class Breakable : MonoBehaviour, vIDamageReceiver
    {
        public Transform brokenObject;
        [Header("Break Object Settings")]

        public float shellStrength = 40f;
        public UnityEngine.Events.UnityEvent OnBroken;
        public UnityEngine.Events.UnityEvent OnWeakHit;

        [SerializeField] protected OnReceiveDamage _onStartReceiveDamage = new OnReceiveDamage();
        [SerializeField] protected OnReceiveDamage _onReceiveDamage = new OnReceiveDamage();
        public OnReceiveDamage onStartReceiveDamage { get { return _onStartReceiveDamage; } protected set { _onStartReceiveDamage = value; } }
        public OnReceiveDamage onReceiveDamage { get { return _onReceiveDamage; } protected set { _onReceiveDamage = value; } }


        private bool isBroken;
        private Collider _collider;
        private Rigidbody _rigidBody;

        void Start()
        {
            _collider = GetComponent<Collider>();
            _rigidBody = GetComponent<Rigidbody>();
        }

        public void TakeDamage(vDamage damage)
        {
            // if they have a power up, double it
            if (PowerUpManager.instance.hasBreakingPower)
                damage.damageValue = damage.damageValue * 2;


            if (damage.damageValue >= shellStrength)
            {
                if (!isBroken)
                {
                    isBroken = true;
                    StartCoroutine(BreakObject());

                    // if they had breaking power, they lost it with this hit
                    if (PowerUpManager.instance.hasBreakingPower)
                        PowerUpManager.instance.LostBreakingPower();
                }
            }
            else
            {
                OnWeakHit.Invoke();
            }
        }

        IEnumerator BreakObject()
        {
            if (_rigidBody) Destroy(_rigidBody);
            if (_collider) Destroy(_collider);
            yield return new WaitForEndOfFrame();
            brokenObject.transform.parent = null;
            brokenObject.gameObject.SetActive(true);
            OnBroken.Invoke();
            Destroy(gameObject);
        }

    }
}