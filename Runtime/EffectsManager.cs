using System.Threading.Tasks;
using Pixygon.Addressable;
using UnityEngine;
using UnityEngine.Pool;

namespace Pixygon.Effects {
    public class EffectsManager : MonoBehaviour {
        [SerializeField] private EffectData _bloodEffect;
        [SerializeField] private EffectData _landEffect;
        [SerializeField] private EffectData _burnEffect;
        [SerializeField] private EffectData _poisonEffect;
        [SerializeField] private EffectData _concoctionEffect;
        [SerializeField] private EffectData _levelUpEffect;
        [SerializeField] private EffectData _hitEffect;
        [SerializeField] private EffectData _criticalEffect;
        [SerializeField] private EffectData _treeEffect;
        [SerializeField] private EffectData _plantEffect;
        [SerializeField] private EffectData _mineralsEffect;
        [SerializeField] private EffectData _fishEffect;
        [SerializeField] private EffectData _artifactEffect;
        [SerializeField] private EffectData _insectEffect;
        [SerializeField] private EffectData _partbreakEffect;
        [SerializeField] private EffectData _evolutionEffect;
        [SerializeField] private EffectData _questCompleteEffect;
        
        private static ObjectPool<EffectObject> _bloodFXPool;
        private static ObjectPool<EffectObject> _landFXPool;
        private static ObjectPool<EffectObject> _burnFXPool;
        private static ObjectPool<EffectObject> _poisonFXPool;
        private static ObjectPool<EffectObject> _concoctionFXPool;
        private static ObjectPool<EffectObject> _levelUpFXPool;
        private static ObjectPool<EffectObject> _hitFXPool;
        private static ObjectPool<EffectObject> _criticalFXPool;
        private static ObjectPool<EffectObject> _treeFXPool;
        private static ObjectPool<EffectObject> _plantFXPool;
        private static ObjectPool<EffectObject> _mineralFXPool;
        private static ObjectPool<EffectObject> _fishFXPool;
        private static ObjectPool<EffectObject> _artifactFXPool;
        private static ObjectPool<EffectObject> _insectFXPool;
        private static ObjectPool<EffectObject> _partbreakPool;
        private static ObjectPool<EffectObject> _evolutionPool;
        private static ObjectPool<EffectObject> _questCompletePool;

        private void Start() {
            Setup();
        }
        private async void Setup() {
            _bloodFXPool = await SetupPool(_bloodEffect);
            _landFXPool = await SetupPool(_landEffect);
            _burnFXPool = await SetupPool(_burnEffect);
            _poisonFXPool = await SetupPool(_poisonEffect);
            _concoctionFXPool = await SetupPool(_concoctionEffect);
            _levelUpFXPool = await SetupPool(_levelUpEffect);
            _hitFXPool = await SetupPool(_hitEffect);
            _criticalFXPool = await SetupPool(_criticalEffect);
            _treeFXPool = await SetupPool(_treeEffect);
            _plantFXPool = await SetupPool(_plantEffect);
            _mineralFXPool = await SetupPool(_mineralsEffect);
            _fishFXPool = await SetupPool(_fishEffect);
            _artifactFXPool = await SetupPool(_artifactEffect);
            _insectFXPool = await SetupPool(_insectEffect);
            _partbreakPool = await SetupPool(_partbreakEffect);
            _evolutionPool = await SetupPool(_evolutionEffect);
            _questCompletePool = await SetupPool(_questCompleteEffect);
        }
        private async Task<ObjectPool<EffectObject>> SetupPool(EffectData data) {
            var g1 = await AddressableLoader.LoadGameObject(data._effectPrefab, transform);
            return new ObjectPool<EffectObject>(() => 
                Instantiate(g1, transform.parent).GetComponent<EffectObject>(), obj => {
                obj.gameObject.SetActive(true);
            }, obj => {
                obj.gameObject.SetActive(false);
            }, obj => {
                Destroy(obj.gameObject);
            }, false, data._startCapacity, data._maxCapacity);
        }
        public static void SpawnEffect(Vector3 pos, VFXType type) {
            ObjectPool<EffectObject> pool;
            switch (type) {
                case VFXType.Hit:
                    pool = _hitFXPool;
                    break;
                case VFXType.Critical:
                    pool = _criticalFXPool;
                    break;
                case VFXType.Blood:
                    pool = _bloodFXPool;
                    break;
                case VFXType.Burn:
                    pool = _burnFXPool;
                    break;
                case VFXType.Poison:
                    pool = _poisonFXPool;
                    break;
                case VFXType.Land:
                    pool = _landFXPool;
                    break;
                case VFXType.Concoction:
                    pool = _concoctionFXPool;
                    break;
                case VFXType.LevelUp:
                    pool = _levelUpFXPool;
                    break;
                case VFXType.Partbreak:
                    pool = _partbreakPool;
                    break;
                case VFXType.Water:
                case VFXType.Air:
                case VFXType.Thunder:
                case VFXType.Ice:
                case VFXType.Light:
                case VFXType.Dark:
                case VFXType.Meta:
                    case VFXType.Evolution:
                    pool = _evolutionPool;
                    break;
                case VFXType.QuestComplete:
                    pool = _questCompletePool;
                    break;
                default:
                    return;
            }

            if (pool == null) {
                Debug.Log("Missing effect: " + type);
                return;
            }
            var obj = pool.Get();
            obj.Initialize(pos, () => pool.Release(obj));
        }
        public static void SpawnEffect(Vector3 pos, int recievedItem, string type) {
            EffectObject obj = null;
            switch(type) {
                case "Plants":
                obj = _plantFXPool.Get();
                obj.Initialize(pos, recievedItem, () => _plantFXPool.Release(obj));
                break;
                case "Trees":
                obj = _treeFXPool.Get();
                obj.Initialize(pos, recievedItem, () => _treeFXPool.Release(obj));
                break;
                case "Minerals":
                obj = _mineralFXPool.Get();
                obj.Initialize(pos, recievedItem, () => _mineralFXPool.Release(obj));
                break;
                case "Artifacts":
                obj = _artifactFXPool.Get();
                obj.Initialize(pos, recievedItem, () => _artifactFXPool.Release(obj));
                break;
                case "Fish":
                obj = _fishFXPool.Get();
                obj.Initialize(pos, recievedItem, () => _fishFXPool.Release(obj));
                break;
                case "Insects":
                obj = _insectFXPool.Get();
                obj.Initialize(pos, recievedItem, () => _insectFXPool.Release(obj));
                break;
                case "Blueprints":
                break;
            }
        }
    }

    public enum VFXType {
        Hit,
        Blood,
        Critical,
        Burn,
        Poison,
        Land,
        Concoction,
        LevelUp,
        Water,
        Air,
        Thunder,
        Ice,
        Light,
        Dark,
        Meta,
        Partbreak,
        Evolution,
        QuestComplete
    }
}