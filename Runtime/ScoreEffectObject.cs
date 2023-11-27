using System;
using TMPro;
using UnityEngine;

namespace Pixygon.Effects
{
    public class ScoreEffectObject : MonoBehaviour
    {
        [SerializeField] private float _killTime = 3f;
        [SerializeField] private TextMeshPro _scoreText;
        [SerializeField] private float _speed;
        [SerializeField] private Animator _anim;
        
        private Action _onKill;
        private float _timer;
        
        public void Initialize(int score, Vector3 pos, Action onKill, bool critical = false, int rank = 0) {
            transform.position = pos;
            _onKill = onKill;
            _timer = _killTime;
            _scoreText.color = Color.white;
            _scoreText.text = score.ToString();
            _anim?.SetBool("Critical", critical);
            _anim?.SetInteger("Rank", rank);
        }
        private void Update() {
            transform.Translate(Vector3.up*_speed);
            _scoreText.color = Color.Lerp(Color.white, new Color(1, 1, 1, 0f), (_killTime-_timer)/3f);
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