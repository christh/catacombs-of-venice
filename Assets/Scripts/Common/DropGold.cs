using CV.Factories;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CV
{
    public class DropGold : MonoBehaviour, IDropsLoot
    {
        public GameObject goldSprayer;
        public int goldItemCount = 2;
        public float quality = 0.25f;

        public void Drop(object loot)
        {
            PickupFactory.SpawnGold(goldSprayer, transform.position, goldItemCount, quality);
        }

        public void SetLootQuality(float value)
        {
            quality = value;
        }

        public void SetLootValue(int value)
        {
            goldItemCount = value;
        }
    }
}