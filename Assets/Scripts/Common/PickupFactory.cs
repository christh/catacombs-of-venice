using UnityEngine;
using System.Collections;

namespace IR.Factories
{
    public class PickupFactory : MonoBehaviour
    {
        // TODO: add to global settings
        private static Transform group = GameObject.Find("/Pickup Container").transform;

        public static void SpawnGold(GameObject goldSprayer, Vector2 position, int count, float quality)
        {
            if (goldSprayer == null)
            {
                print("GoldSprayer is null...");
                return;
            }

            var msObj = Instantiate(goldSprayer, position, Quaternion.identity, group);
            var ms = msObj.GetComponent<GoldSprayer>();
            ms.NumberOfCoins = count;
            ms.Quality = quality;
            ms.transform.SetParent(group);
        }

        public static void PlaySpriteAnimation(GameObject animation, Vector2 position)
        {
            if (animation != null)
            {
                var obj = Instantiate(animation, position, Quaternion.identity);
                //var ms = obj.GetComponent<Animator>();
                Destroy(obj, 0.5f); // TODO: Should be animation.length...
            }
        }
    }
}