using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IR
{
    public class Health : MonoBehaviour
    {
        [SerializeField] int maxHealth = 10;
        [SerializeField] int currentHealth = 10;
        [SerializeField] int armour = 0;

        internal bool IsDead => currentHealth <= 0; 

        internal void TakeDamage(float amount)
        {
            int damageAmount;

            damageAmount = (int)Math.Floor(amount) - armour;

            currentHealth = (int)(currentHealth - damageAmount);

            HealthPopup.Damage(transform.position, (int)Math.Ceiling(amount));
        }

        internal void Heal(float amount)
        {
            int healed;

            if (currentHealth + amount >= maxHealth)
            {
                healed = maxHealth - currentHealth;
            }
            else
            {
                healed = (int)Math.Ceiling(amount);
            }
            currentHealth += healed;

            HealthPopup.Heal(transform.position, healed);

            
        }

        public int GetCurrentHealth() { return currentHealth; }

        internal void HealToMax()
        {
            currentHealth = maxHealth;
        }
    }
}