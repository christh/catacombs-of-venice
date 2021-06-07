using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CV {
    public class LevelEntrance : MonoBehaviour
    {
        void Start()
        {
            var player = FindObjectOfType<Player>();

            player.transform.position = transform.position;
        }

    }
}