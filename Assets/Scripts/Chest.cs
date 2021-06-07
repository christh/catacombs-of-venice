using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace CV
{
    public class Chest : MonoBehaviour, IInteractable
    {
        [SerializeField] private bool isOpen;
        [SerializeField] private Transform closedSprite;
        [SerializeField] private Transform openSprite;

        public delegate void Lit(bool isLighting);
        public static event Lit OnLit;

        public void Start()
        {

        }
        public void Interact(GameObject interactor)
        {
            if (!isOpen && interactor.CompareTag("Player"))
            {
                Open();
            }
        }

        private void Open()
        {
            isOpen = true;
            openSprite.gameObject.SetActive(isOpen);
            closedSprite.gameObject.SetActive(!isOpen);
            var loot = gameObject.GetComponentsInChildren<IDropsLoot>();
            foreach (var item in loot)
            {
                 item.Drop(0);
            }
        }

        // Start is called before the first frame update
        public void OnTriggerEnter2D(Collider2D collision)
        {
            Interact(collision.gameObject);
        }
    }
}