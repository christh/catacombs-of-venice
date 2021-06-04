using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace IR
{
    public class Enemy : MonoBehaviour, IDamageable
    {

        [SerializeField] float speed = 3;
        [SerializeField] bool idle = true;
        [SerializeField] string[] TriggerableTags;
        [SerializeField] AudioClip hitSound;
        private AudioSource audioSource;
        private Rigidbody2D rb;
        private NavMeshAgent agent;

        public Health Health { get; set; }
        public event Action<Health> OnHealthChanged;

        // Start is called before the first frame update
        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            rb = GetComponent<Rigidbody2D>();
            Health = GetComponent<Health>();
            audioSource = GetComponent<AudioSource>();

            agent.updateRotation = false;
            agent.updateUpAxis = false;
            agent.speed = speed;
        }

        // Update is called once per frame
        void Update()
        {
            if (agent.enabled == false) return;

            if (idle)
            {
                RandomMove();
            }
            else
            {
                ChasePlayer();
            }
        }

        private void ChasePlayer()
        {
            var player = GameObject.FindGameObjectWithTag("Player");

            if (player == null)
            {
                idle = true;
                return;
            }
            agent.destination = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);
        }

        private void RandomMove()
        {
            StartCoroutine(RandomImpulse());
        }

        IEnumerator RandomImpulse()
        {
            var randX = UnityEngine.Random.Range(-1, 2);
            var randY = UnityEngine.Random.Range(-1, 2);

            rb.AddForce(new Vector2(randX * speed, randY * speed));

            yield return new WaitForSeconds(1.5f);
        }

        public void Damage(float damageValue, string tag)
        {
            if (!InDamageableTags(tag)) return;

            Health.TakeDamage(damageValue);

            idle = false;

            if (Health.IsDead)
            {
                GameManager.Instance?.AddEnemyKillCount(1);
                GetComponentInChildren<EnemyTargeting>().enabled = false;
                GetComponent<IDropsLoot>()?.Drop(damageValue);
                GetComponent<IDestructible>()?.Die(damageValue);
                agent.enabled = false;
            }
            else
            {
                audioSource.pitch = UnityEngine.Random.Range(0.9f, 1.1f);
                audioSource.PlayOneShot(hitSound);
            }
        }

        private bool InDamageableTags(string tag)
        {
            // If unset, triggered by everything
            if (TriggerableTags.Length == 0)
            {
                return true;
            }
            else
            {
                foreach (var item in TriggerableTags)
                {
                    if (tag == item)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}