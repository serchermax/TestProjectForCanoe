using System;

namespace Gameplay
{ 
    public interface IHealth
    {
        public event Action OnHealthChanged;

        public float MaxHealth { get; }
        public float Health { get; }

        public void Damage(float value);
        public void Heal(float value);
    }
}