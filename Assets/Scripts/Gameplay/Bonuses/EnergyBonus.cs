using UnityEngine;

namespace Gameplay
{
    public class EnergyBonus : Bonus
    {
        [Header("Energy Settings")]
        [SerializeField] private float _energyTime = 10f;

        protected override void BonusEffect(Collider player)
        {
            if (player.TryGetComponent(out PlayerShield shield))
            {
                shield.ForceEnergy = _energyTime;
            }
        }
    }
}