using System;

namespace Gameplay
{
    public interface IDestroyable
    {
        public event Action OnDestroy;
    }
}

