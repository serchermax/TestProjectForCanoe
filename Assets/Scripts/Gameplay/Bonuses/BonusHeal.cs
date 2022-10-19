using UnityEngine;

namespace Gameplay
{
    public class BonusHeal : Bonus
    {
        [Header("Heal Settings")]
        [SerializeField] private float _heal = 10f;

        protected override void BonusEffect(Collider player)
        {
            if (player.TryGetComponent(out IHealth health))
            {
                health.Heal(_heal);
            }
        }    
    }
}