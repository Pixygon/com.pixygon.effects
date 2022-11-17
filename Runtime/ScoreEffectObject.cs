using System;
using TMPro;
using UnityEngine;

namespace Pixygon.Effects
{
    public class ScoreEffectObject : MonoBehaviour
    {
        [SerializeField] private float _killTime = 3f;
        [SerializeField] private TextMeshPro _scoreText;
        private Action _onKill;
        private float _timer;
        
        public void Initialize(int score, Vector3 pos, Action onKill) {
            transform.position = pos;
            _onKill = onKill;
            _timer = _killTime;
            _scoreText.text = score.ToString();
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