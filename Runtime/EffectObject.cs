using UnityEngine;
using UnityEngine.Pool;

namespace Pixygon.Effects {
    public class EffectObject : MonoBehaviour {
        [SerializeField] private ParticleSystem _particles;
        [SerializeField] private AudioSource _sfx;
        [SerializeField] private float _killTime = 3f;
        [SerializeField] private bool _sfxByValue;
        [SerializeField] private AudioClip[] _sfxClips;
        [SerializeField] private bool _usePitching;

        // The object releases ITSELF back to its pool — set once when the pool
        // hands it out (EffectsManager.SetupPool). Removes the per-spawn release
        // closure that used to allocate a delegate on every effect spawn
        // (hundreds/sec on the hit + kill path).
        private IObjectPool<EffectObject> _pool;
        private float _timer;

        public void SetPool(IObjectPool<EffectObject> pool) => _pool = pool;

        public virtual void Initialize(Vector3 pos) {
            transform.position = pos;
            _timer = _killTime;
            _particles.Play();
            if(_sfx != null) {
                if(_usePitching) _sfx.pitch = UnityEngine.Random.Range(.95f, 1.05f);
                _sfx.Play();
            }
        }
        public virtual void Initialize(Vector3 pos, int value) {
            transform.position = pos;
            _timer = _killTime;
            _particles.Play();
            if (_sfx == null) return;
            if(_sfxByValue)
                _sfx.clip = _sfxClips[value];
            if(_usePitching) _sfx.pitch = UnityEngine.Random.Range(.95f, 1.05f);
            _sfx.Play();
        }

        private void Update() {
            if(_timer > 0f)
                _timer -= Time.deltaTime;
            else
                OnKill();
        }

        public void OnKill() {
            _pool?.Release(this);
        }
    }
}
