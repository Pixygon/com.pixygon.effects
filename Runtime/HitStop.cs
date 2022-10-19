using System.Threading.Tasks;
using UnityEngine;

namespace Pixygon.Effects {
    public class HitStop : MonoBehaviour {
        private static bool _waiting;

        public static async Task Stop(int duration) {
            if (_waiting) return;
            Time.timeScale = 0.0f;
            _waiting = true;
            await Task.Delay(duration);
            Time.timeScale = 1f;
            _waiting = false;
        }
    }
}