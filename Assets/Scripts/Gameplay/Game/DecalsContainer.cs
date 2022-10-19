using UnityEngine;

namespace Gameplay
{
    public class DecalsContainer : MonoBehaviour
    {
        public static DecalsContainer Instance { get; private set; }
    
        [Header("Settings")]
        [SerializeField] private float _decalsLifeTime = 2f;
        [SerializeField] private Transform _decalParent;
        [Space]
        [SerializeField] private PoolObject _energyDecalContainerPrefab;
        [SerializeField] private int _energyDecalsCount;

        private QueuePoolMono<PoolObject> _energyDecalsPool;

        private void Awake()
        {
            Instance = this;
            _energyDecalsPool = new QueuePoolMono<PoolObject>(_energyDecalContainerPrefab, _energyDecalsCount, _parent);
        }
        public PoolObject EnergyDecal
        {
            get
            {
                PoolObject temp = _energyDecalsPool.GetObjectFromPool();
                temp.BackToPool(_decalsLifeTime);
                return temp;
            }
        }
        private Transform _parent
        {
            get { return _decalParent == null ? transform : _decalParent; }
        }

        public PoolObject SetDecal(RaycastHit hit)
        {
            PoolObject decal = EnergyDecal;
            decal.transform.position = hit.point + hit.normal * 0.01f;
            decal.transform.rotation = _energyDecalContainerPrefab.transform.rotation;
            decal.transform.rotation = Quaternion.FromToRotation(decal.transform.up, hit.normal);
            return decal;
        }

        public void SetDecal(RaycastHit hit, Transform parentForDecal, IDestroyable destroyable) 
        {
            var decal = SetDecal(hit);
            decal.transform.SetParent(parentForDecal);
            destroyable.OnDestroy += () => BackToPool(destroyable, decal);
        }         

        private void BackToPool(IDestroyable destroyable, PoolObject poolObject)
        { 
            destroyable.OnDestroy -= () => BackToPool(destroyable, poolObject);
            poolObject.transform.SetParent(_parent);
            poolObject.BackToPool();
        }
    }
}