using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pixygon.Addressable;
using Pixygon.DebugTool;
using Pixygon.ID;
using UnityEngine;
using UnityEngine.Pool;

namespace Pixygon.Effects {
    public class EffectsManager : MonoBehaviour {
        [SerializeField] private IdGroup _effectGroup;
        
        private static List<PoolObject> _pools;

        private async void Start() {
            _pools = new List<PoolObject>();
            foreach (var item in _effectGroup._categories.SelectMany(category => category._list))
                _pools.Add(new PoolObject(item.GetFullID, await SetupPool(item as EffectData)));
        }
        private async Task<ObjectPool<EffectObject>> SetupPool(EffectData data) {
            var g1 = await AddressableLoader.LoadGameObject(data._effectPrefab, transform);
            return new ObjectPool<EffectObject>(() => 
                Instantiate(g1, transform.parent).GetComponent<EffectObject>(), obj => {
                obj.gameObject.SetActive(true);
            }, obj => {
                obj.gameObject.SetActive(false);
                obj.transform.parent = transform;
            }, obj => {
                Destroy(obj.gameObject);
            }, false, data._startCapacity, data._maxCapacity);
        }
        public static void SpawnEffect(int id, Vector3 pos, Transform parent = null) {
            ObjectPool<EffectObject> pool;
            foreach (var poolObject in _pools.Where(poolObject => poolObject._id == id)) {
                pool = poolObject._effect;
                var obj = pool.Get();
                obj.Initialize(pos, () => pool.Release(obj));
                if (parent != null) obj.transform.parent = parent;
                return;
            }
            Log.DebugMessage(DebugGroup.Effects, $"Missing effect: {id}");
        }
    }
}