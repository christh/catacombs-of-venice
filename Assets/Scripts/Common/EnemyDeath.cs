using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IR.Factories;

namespace IR
{
    public class EnemyDeath : MonoBehaviour, IDestructible
    {
        [SerializeField] int deadAnimationCount;
        public GameObject DeathPrefab;
        public AudioClip DeathAudio;

        private Animator animator;
        private bool triggered;
        private int deadThingsLayer = 17;

        public void Die(float damageValue)
        {
            ExplosionFactory.SpawnEffect(DeathPrefab, DeathAudio, transform.position);

            if (triggered == true) return;

            triggered = true;
            animator = GetComponent<Animator>();
            gameObject.layer = deadThingsLayer;
            if (animator)
            {
                // Play a random death anim
                animator.SetBool($"isDead{Random.Range(1, 3 + 1)}", true);
            }
            
            Destroy(gameObject, 2);
        }
    }
}