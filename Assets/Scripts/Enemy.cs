using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace CV
{
    public class Enemy : MonoBehaviour, IDamageable
    {

        [SerializeField] float speed = 3;
        [SerializeField] bool idle = true;
        [SerializeField] string[] TriggerableTags;
        [SerializeField] AudioClip hitSound;
        [SerializeField] float maintainedDistance = 2f;
        private AudioSource audioSource;
        private Rigidbody2D rb;
        private NavMeshAgent agent;
        private GameObject player;


        public Health Health { get; set; }
        public event Action<Health> OnHealthChanged;

        // Start is called before the first frame update
        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            rb = GetComponent<Rigidbody2D>();
            Health = GetComponent<Health>();
            audioSource = GetComponent<AudioSource>();
            player = GameObject.FindGameObjectWithTag("Player");
            agent.updateRotation = false;
            agent.updateUpAxis = false;
            agent.speed = speed;
        }

        public bool IsAggroed()
        {
            return !idle;
        }

        public bool IsMoving()
        {
            return !agent.isStopped;
        }
        public void ResetAggro()
        {
            idle = true;
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
            if (player == null)
            {
                idle = true;
                return;
            }

            agent.destination = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);

            if (agent.remainingDistance > 0 && agent.remainingDistance < UnityEngine.Random.Range(maintainedDistance * 0.8f, maintainedDistance * 1.2f)) // Keep a distance from the player
            {
                agent.isStopped = true;
            }
            else
            {
                agent.isStopped = false;
            }

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