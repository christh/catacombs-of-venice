using System.Collections;
using UnityEngine;

namespace CV
{
    public class GoldSprayer : MonoBehaviour
    {
        public int NumberOfCoins = 4;
        public float Quality = 0.5f;
        public GameObject GoldPrefabBase;
        public GameObject GoldPrefabRare;
        private PointEffector2D pickupEffectorExplosion;
        [SerializeField] float initialGoldForce = 1;

        void Start()
        {
            pickupEffectorExplosion = GetComponent<PointEffector2D>();

            SpawnGold();

        }

        void SpawnGold()
        {
            for (int i = 0; i < NumberOfCoins; i++)
            {
                if (Random.value > Quality)
                {
                    SpawnCoin(GoldPrefabBase);
                }
                else
                {
                    SpawnCoin(GoldPrefabRare);
                }
            }

            StartCoroutine(DelayedExplosion());
        }

        private void SpawnCoin(GameObject goldPrefabBase)
        {
            var go = Instantiate(goldPrefabBase, (Vector2)transform.position, Quaternion.identity, transform.parent);
            var rb = go.GetComponent<Rigidbody2D>();
            rb.AddForce(
                new Vector2(
                    Random.Range(-initialGoldForce, initialGoldForce), 
                    Random.Range(-initialGoldForce, initialGoldForce)
                )
            );
        }

        IEnumerator DelayedExplosion()
        {
            yield return new WaitForSeconds(.1f);
            pickupEffectorExplosion.enabled = true;
            Destroy(gameObject, .5f);
        }
    }
}