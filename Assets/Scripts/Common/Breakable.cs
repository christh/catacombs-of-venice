using UnityEngine;
using IR.Factories;

namespace IR
{
    public class Breakable : MonoBehaviour, IDestructible
    {
        [SerializeField] int deadAnimationCount;
        public GameObject ExplosionPreFab;
        public AudioClip ExplosionAudio;

        private bool triggered;

        public void Die(float damageValue) {

            if (triggered) return;

            triggered = true;

            ExplosionFactory.SpawnEffect(ExplosionPreFab, ExplosionAudio, (Vector2)transform.position);

            Destroy(gameObject, 2);
        }
    }
}
