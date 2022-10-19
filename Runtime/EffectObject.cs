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
        private float timer;

        public virtual void Initialize(Vector3 pos, Action onKill) {
            transform.position = pos;
            _onKill = onKill;
            timer = _killTime;
            _particles.Play();
            if(_sfx != null)
                _sfx.Play();
        }
        public virtual void Initialize(Vector3 pos, int value, Action onKill) {
            transform.position = pos;
            _onKill = onKill;
            timer = _killTime;
            _particles.Play();
            if(_sfx != null) {
                if(_sfxByValue)
                    _sfx.clip = _sfxClips[value];
                _sfx.pitch = UnityEngine.Random.Range(.95f, 1.05f);
                _sfx.Play();
            }
        }

        private void Update() {
            if(timer > 0f)
                timer -= Time.deltaTime;
            else
                OnKill();
        }

        public void OnKill() {
            _onKill?.Invoke();
        }
    }
}