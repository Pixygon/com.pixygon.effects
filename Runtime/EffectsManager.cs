using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pixygon.Addressable;
using Pixygon.DebugTool;
using Pixygon.ID;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Pool;

namespace Pixygon.Effects {
    public class EffectsManager : MonoBehaviour {
        [SerializeField] private IdGroup _effectGroup;
        [SerializeField] private AssetReference _scoreEffect;

        // Dictionary for O(1) id lookup. Was a List<PoolObject> scanned with a
        // LINQ `.Where()` (enumerator + closure alloc) on EVERY effect spawn —
        // hundreds of times/sec on the damage + kill path.
        private static Dictionary<int, ObjectPool<EffectObject>> _pools;
        private static ObjectPool<ScoreEffectObject> _scorePool;

        private async void Start() {
            _pools = new Dictionary<int, ObjectPool<EffectObject>>();
            foreach (var item in _effectGroup._categories.SelectMany(category => category._list)) {
                var pool = await SetupPool(item as EffectData);
                if (pool != null) _pools[item.GetFullID] = pool;
            }
            if(_scoreEffect != null)
                _scorePool = await SetupScorePool();
        }
        private async Task<ObjectPool<EffectObject>> SetupPool(EffectData data) {
            var g1 = await AddressableLoader.LoadGameObject(data._effectPrefab, transform);
            // The actionOnGet closure captures `pool` once (not per spawn) so each
            // pooled object can release itself — no per-spawn delegate allocation.
            ObjectPool<EffectObject> pool = null;
            pool = new ObjectPool<EffectObject>(
                () => Instantiate(g1, transform.parent).GetComponent<EffectObject>(),
                obj => { obj.gameObject.SetActive(true); obj.SetPool(pool); },
                obj => { obj.gameObject.SetActive(false); obj.transform.parent = transform; },
                obj => Destroy(obj.gameObject),
                false, data._startCapacity, data._maxCapacity);
            return pool;
        }
        private async Task<ObjectPool<ScoreEffectObject>> SetupScorePool() {
            var g1 = await AddressableLoader.LoadGameObject(_scoreEffect, transform);
            ObjectPool<ScoreEffectObject> pool = null;
            pool = new ObjectPool<ScoreEffectObject>(
                () => Instantiate(g1, transform.parent).GetComponent<ScoreEffectObject>(),
                obj => { obj.gameObject.SetActive(true); obj.SetPool(pool); },
                obj => { obj.gameObject.SetActive(false); obj.transform.parent = transform; },
                obj => Destroy(obj.gameObject),
                false, 5, 10);
            return pool;
        }
        public static void SpawnEffect(int id, Vector3 pos, Transform parent = null) {
            if (_pools != null && _pools.TryGetValue(id, out var pool)) {
                var obj = pool.Get();
                obj.Initialize(pos);
                if (parent != null) obj.transform.parent = parent;
                return;
            }
            Log.DebugMessage(DebugGroup.Effects, $"Missing effect: {id}");
        }

        public static void SpawnScoreEffect(int score, Vector3 pos, Transform parent = null, bool critical = false, int rank = 0) {
            if (_scorePool == null) {
                Log.DebugMessage(DebugGroup.Effects, "Missing scoreEffect!");
                return;
            }
            var obj = _scorePool.Get();
            obj.Initialize(score, pos, critical, rank);
            if (parent != null) obj.transform.parent = parent;
        }
    }
}
