using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace IR
{
    public class Torch : MonoBehaviour, IInteractable
    {
        [SerializeField] private bool lit;
        [SerializeField] private SpriteRenderer spriteRenderer;

        private Light2D light2D;
        private Animator anim;

        public delegate void Lit(bool isLighting);
        public static event Lit OnLit;

        public void Start()
        {
            light2D = GetComponent<Light2D>();
            anim = GetComponent<Animator>();

            if (lit)
            {
                Ignite();
            }
            else
            {
                Douse();
            }
        }
        public void Interact(GameObject interactor)
        {
            if (lit && interactor.CompareTag("Projectile"))
            {
                Douse();
            }
            else if(!lit && interactor.CompareTag("Player"))
            {
                Ignite();
            }
        }

        private void Ignite()
        {
            lit = true;
            light2D.enabled = true;
            anim.SetBool("isLit", true);
        }

        private void Douse()
        {
            lit = false;
            light2D.enabled = false;
            anim.SetBool("isLit", false);
        }

        // Start is called before the first frame update
        public void OnTriggerEnter2D(Collider2D collision)
        {
            Interact(collision.gameObject);
        }
    }
}