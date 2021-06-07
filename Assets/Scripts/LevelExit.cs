using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CV
{

    public class LevelExit : MonoBehaviour
    {
        [SerializeField] bool isGrandExit = false;
        [SerializeField] string destination;
        // Start is called before the first frame update
        void Start()
        {

        }

        public void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                EndLevel();
            }
        }

        public void EndLevel()
        {
            if (isGrandExit)
            {
                UIManager.Instance.WinState();
            }

            var gold = FindObjectsOfType<Pickup>();
            foreach (var item in gold)
            {
                Destroy(item.gameObject);
            }

            GameManager.Instance?.GoToFloor(destination);
        }
    }
}