using IR.Factories;
using System;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace IR
{
    public class Pickup : MonoBehaviour
    {
        [SerializeField] int value = 1;
        [SerializeField] GameObject Animation;
        [SerializeField] AudioClip CollectSound;

        private Rigidbody2D rb;
        private AudioSource audioSource;
        private float pauseBeforeCollectable = 1f;
        private int activePickupLayer = 12;
        private int inActivePickupLayer = 14;

        private void Start()
        {
            audioSource = GetComponent<AudioSource>();
            rb = GetComponent<Rigidbody2D>();
            gameObject.layer = inActivePickupLayer;
        }

        public void Update()
        {
            if (gameObject.layer != activePickupLayer)
            {
                pauseBeforeCollectable -= Time.deltaTime;
                if (pauseBeforeCollectable < 0)
                {
                    gameObject.layer = activePickupLayer;
                }
            }

        }
        public event Action<Pickup> OnPickup;

        void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                //HandlePickup(other);
                if (GameManager.IsInitialised)
                {
                    GameManager.Instance.AddGold(value);
                }

                if (Animation)
                {
                    PickupFactory.PlaySpriteAnimation(Animation, transform.position);
                }

                if (rb == null) return;

                rb.simulated = false;

                if (CollectSound)
                {
                    audioSource.pitch = UnityEngine.Random.Range(0.9f, 1.1f);
                    audioSource.PlayOneShot(CollectSound);
                    GetComponentInChildren<SpriteRenderer>().enabled = false;
                    Destroy(gameObject, CollectSound.length);
                    GetComponent<Light2D>().enabled = false;
                }
                else
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}