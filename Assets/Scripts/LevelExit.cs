using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IR
{


    public class LevelExit : MonoBehaviour
    {
        [SerializeField] string destination;
        // Start is called before the first frame update
        void Start()
        {

        }

        public void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                GameManager.Instance?.GoToFloor(destination);
            }


        }
    }
}