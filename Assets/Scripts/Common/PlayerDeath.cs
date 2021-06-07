using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CV.Factories;

namespace CV
{
    public class PlayerDeath : MonoBehaviour, IDestructible
    {
        [SerializeField] int deadAnimationCount;
        public GameObject DeathPrefab;
        public AudioClip DeathAudio;

        private Animator animator;
        private bool triggered;
        private int deadThingsLayer = 17;
        private int playerLayer = 9;
        private string currentDeadState;

        public void Die(float damageValue)
        {
            GetComponent<PointEffector2D>().enabled = false;

            ExplosionFactory.SpawnEffect(DeathPrefab, DeathAudio, transform.position);

            if (triggered == true) return;

            triggered = true;
            animator = GetComponent<Animator>();
            gameObject.layer = deadThingsLayer;
            if (animator)
            {
                // Play a random death anim
                currentDeadState = $"isDead{Random.Range(1, 3 + 1)}";
                animator.SetBool(currentDeadState, true);
            }

            StartCoroutine(Resurrect());
        }

        IEnumerator Resurrect()
        {
            yield return new WaitForSeconds(2f);
            transform.position = FindObjectOfType<LevelEntrance>().transform.position;
            transform.rotation = Quaternion.identity;
            triggered = false;
            gameObject.layer = playerLayer;
            GetComponent<PointEffector2D>().enabled = true;
            GetComponent<Health>().HealToMax();
            animator.SetBool(currentDeadState, false);
        }
    }
}