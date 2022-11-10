using UnityEngine.Pool;

namespace Pixygon.Effects {
    public class PoolObject {
        public readonly int _id;
        public readonly ObjectPool<EffectObject> _effect;

        public PoolObject(int id, ObjectPool<EffectObject> effect) {
            _id = id;
            _effect = effect;
        }
    }
}