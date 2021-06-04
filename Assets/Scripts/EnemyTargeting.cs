using UnityEngine;

namespace IR {
    public class EnemyTargeting : MonoBehaviour
    {
        [SerializeField] private float primaryCoolDown = 1.5f;
        [SerializeField] public bool isActive = true;
        [SerializeField] public bool inRangeOfPlayer = false;
        [SerializeField] public float inAccuracy = .15f;
        [SerializeField] float spinRate = -500f;
        [SerializeField] GameObject BulletPrefab;
        [SerializeField] GameObject Muzzle;

        private float primaryTimer = 0;
        private Player player;
        // Start is called before the first frame update
        void Start()
        {
            player = FindObjectOfType<Player>();
        }

        public void Update()
        {
            if (!isActive) { return; }

            primaryTimer += Time.deltaTime;

            if (inRangeOfPlayer && primaryTimer >= primaryCoolDown * Random.Range(1, 1.5f))
            {
                primaryTimer = 0;
                ShootAtPlayer();
            }
        }

        void ShootAtPlayer()
        {
            if (!player) return;

            var angleFudge = Random.Range(-inAccuracy, inAccuracy);
            var target = player.transform.position - transform.position;
            var bulletRotation = Quaternion.LookRotation(Vector3.forward * angleFudge, target);

            GameObject projectileObject = Instantiate(BulletPrefab, (Vector2)Muzzle.transform.position, bulletRotation);

            var projectile = projectileObject.GetComponent<ForceProjectile>();
            projectile.AddSpin(spinRate);
            projectile.Launch();
        }

        // Update is called once per frame
        public void OnTriggerEnter2D(Collider2D collision)
        {
            inRangeOfPlayer = true;
        }

        public void OnTriggerExit2D(Collider2D collision)
        {
            inRangeOfPlayer = false;
        }
    }
}