using System;
using UnityEngine;

namespace Pixygon.Effects {
    public class EffectObject : MonoBehaviour {
        [SerializeField] private ParticleSystem _particles;
        [SerializeField] private AudioSource _sfx;
        [SerializeField] private float _killTime = 3f;
        [SerializeField] private bool _sfxByValue;
        [SerializeField] private AudioClip[] _sfxClips;

        private Action _onKill;
        private float _timer;

        public virtual void Initialize(Vector3 pos, Action onKill) {
            transform.position = pos;
            _onKill = onKill;
            _timer = _killTime;
            _particles.Play();
            if(_sfx != null)
                _sfx.Play();
        }
        public virtual void Initialize(Vector3 pos, int value, Action onKill) {
            transform.position = pos;
            _onKill = onKill;
            _timer = _killTime;
            _particles.Play();
            if (_sfx == null) return;
            if(_sfxByValue)
                _sfx.clip = _sfxClips[value];
            _sfx.pitch = UnityEngine.Random.Range(.95f, 1.05f);
            _sfx.Play();
        }

        private void Update() {
            if(_timer > 0f)
                _timer -= Time.deltaTime;
            else
                OnKill();
        }

        public void OnKill() {
            _onKill?.Invoke();
        }
    }
}