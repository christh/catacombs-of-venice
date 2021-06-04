using System;

namespace IR
{

    public interface IDamageable
    {
        public Health Health { get; set; }
        public event Action<Health> OnHealthChanged;

        void Damage(float damageValue, string tag);

    }
}
