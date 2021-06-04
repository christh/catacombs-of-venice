using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace IR
{
    public class Player : MonoBehaviour, IDamageable
    {
        [SerializeField] private float moveSpeed = 5f;

        [SerializeField] public bool isActive = true;
        [SerializeField] public float DIAGONAL_SCALE = 1.4f;
        [SerializeField] public float inAccuracy = .1f;
        [SerializeField] private float primaryCoolDown = 0.4f;

        private Vector2 moveDirection;

        public GameObject BulletPrefab;
        public GameObject Turret;

        private NavMeshAgent agent;

        private Rigidbody2D rb;

        private float primaryTimer = 0;

        public Health Health { get; set; }
        public int gold { get; set; }

        public event Action<Health> OnHealthChanged;
        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            rb = GetComponent<Rigidbody2D>();
            Health = GetComponent<Health>();

            GameManager.Instance?.UpdateHealth(Health.GetCurrentHealth());
        }

        void FixedUpdate()
        {
            ProcessInputs();
            PlayerMovement();
        }

        void LaunchToPoint(Vector3 target)
        {
            var angleFudge = UnityEngine.Random.Range(-inAccuracy, inAccuracy);
            target.z = -Camera.main.transform.position.z;
            target = Camera.main.ScreenToWorldPoint(target) - Turret.transform.position;

            var bulletRotation = Quaternion.LookRotation(Vector3.forward * angleFudge, target);

            GameObject projectileObject = Instantiate(BulletPrefab, (Vector2)Turret.transform.position, bulletRotation);

            var projectile = projectileObject.GetComponent<ForceProjectile>();
            projectile.Launch();
        }

        public void Damage(float damageValue, string tag)
        {
            Debug.Log($"Player damaged for {damageValue} from {tag}!");

            Health.TakeDamage(damageValue);
            OnHealthChanged?.Invoke(Health);
            GameManager.Instance?.UpdateHealth(Health.GetCurrentHealth());

            if (Health.IsDead)
            {
                var loot = GetComponent<IDropsLoot>();

                if (loot != null)
                {
                    loot.SetLootQuality(0f);
                    loot.SetLootValue((int)(GameManager.Instance.GetGold() * 0.75));
                    loot.Drop(damageValue);
                    GameManager.Instance.AddGold(-GameManager.Instance.GetGold());
                }

                GetComponent<IDestructible>()?.Die(damageValue);
            }
            else
            {
                // We'd spawn some damage components here or something
            }
        }

        private void ProcessInputs()
        {
            if (!isActive) { return; }

            primaryTimer += Time.deltaTime;

            if ((Input.GetKey(KeyCode.LeftControl) || Input.GetMouseButton(0)) && primaryTimer >= primaryCoolDown)
            {
                primaryTimer = 0;
                Vector3 direction = Input.mousePosition;
                LaunchToPoint(direction);
            }

            float moveX = Input.GetAxisRaw("Horizontal");
            float moveY = Input.GetAxisRaw("Vertical");

            if (Mathf.Abs(moveX) > 0 && Mathf.Abs(moveY) > 0)
            {
                moveX /= DIAGONAL_SCALE;
                moveY /= DIAGONAL_SCALE;
            }

            moveDirection = new Vector2(moveX, moveY);
        }

        private void PlayerMovement()
        {
            if (!isActive) { return; }

            rb.AddForce(new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed));
        }
    }
}