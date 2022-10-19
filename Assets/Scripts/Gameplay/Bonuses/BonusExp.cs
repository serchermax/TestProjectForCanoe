using UnityEngine;

namespace Gameplay
{
    public class BonusExp : Bonus
    {
        [Header("Exp Settings")]
        [SerializeField] private int _exp = 1;

        protected override void BonusEffect(Collider player)
        {
            if (player.TryGetComponent(out PlayerProgress playerProgress))
            {
                playerProgress.SetExp(_exp);
            }
        }
    }
}