using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldSprayer : MonoBehaviour
{
    public int NumberOfCoins = 4;
    public float Quality = 0.5f;
    public GameObject GoldPrefabBase;
    public GameObject GoldPrefabRare;
    public float SpawnOffSet = .1f;
    private Rigidbody2D rb;
    private PointEffector2D pickupEffectorExplosion;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        pickupEffectorExplosion = GetComponent<PointEffector2D>();

        SpawnMinerals();

    }

    void SpawnMinerals()
    {
        for (int i = 0; i < NumberOfCoins; i++)
        {
            Vector2 spawnOffset = new Vector2(Random.Range(-SpawnOffSet, SpawnOffSet), Random.Range(-SpawnOffSet, SpawnOffSet));

            if (Random.value > Quality)
                Instantiate(GoldPrefabBase, (Vector2)transform.position + spawnOffset, Quaternion.identity, transform.parent);
            else
                Instantiate(GoldPrefabRare, (Vector2)transform.position + spawnOffset, Quaternion.identity, transform.parent);
        }

        pickupEffectorExplosion.enabled = true;

        Destroy(gameObject, .5f);

    }
    // Update is called once per frame
    void Update()
    {

    }
}
