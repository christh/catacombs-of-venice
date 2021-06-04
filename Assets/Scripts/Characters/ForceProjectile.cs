using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IR
{
    public class ForceProjectile : MonoBehaviour
    {
        public float speed = 600f;
        public float lifetime = 5f;
        public AudioClip LaunchSound;
        public int power = 10;

        private Rigidbody2D rb;
        private SpriteRenderer Renderer;
        private AudioSource Audio;
        private int inactiveProjectileLayer = 13;
        private bool isActive = false;

        void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            Destroy(gameObject, lifetime);
        }
        public void Update()
        {
            if (isActive && rb.velocity.magnitude < 0.1f)
            {
                gameObject.layer = inactiveProjectileLayer;
                isActive = false;
            }
        }


        public void Launch(Vector2 direction, float force)
        {
            rb.AddForce(rb.transform.up * force);
        }

        public void AddSpin(float spin)
        {
            rb.AddTorque(spin);
        }

        public void Launch()
        {
            if (LaunchSound != null)
            {
                AudioSource.PlayClipAtPoint(LaunchSound, (Vector2)transform.position);
            }

            rb.AddForce(rb.transform.up * speed);
        }

        void OnCollisionEnter2D(Collision2D other)
        {
            other.collider.GetComponent<IDamageable>()?.Damage(power, tag);

            if (other.transform.CompareTag("Enemy"))
            {
                gameObject.transform.SetParent(other.transform);
                rb.simulated = false;
            }

            gameObject.layer = inactiveProjectileLayer;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;

            //rb.simulated = false;
            GetComponent<IDestructible>()?.Die(0);
        }

        internal void SetInitialVelocity(Vector2 velocity)
        {
            rb.velocity = velocity;
        }

        internal void LaunchAt(Vector3 target)
        {
            transform.rotation = Quaternion.LookRotation(Vector3.forward, target);
            //transform.LookAt(target);
            Launch();
        }

        internal void LaunchAt(Transform target)
        {
            transform.LookAt(target);
            Launch();
        }
    }
}