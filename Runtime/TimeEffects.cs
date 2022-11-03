using System.Threading.Tasks;
using UnityEngine;

namespace Pixygon.Effects {
    public class TimeEffects : MonoBehaviour {
        private static bool _waiting;

        public static async Task Stop(int duration) {
            if (_waiting) return;
            Time.timeScale = 0.0f;
            _waiting = true;
            await Task.Delay(duration);
            Time.timeScale = 1f;
            _waiting = false;
        }
        public static async void Slowdown() {
            var time = .5f;
            Time.timeScale = .01f;
            while (time > 0f) {
                time -= Time.deltaTime;
                Time.timeScale = Mathf.Lerp(1f, .01f, time*2f);
                await Task.Yield();
            }
            Time.timeScale = 1f;
        }
    }
}