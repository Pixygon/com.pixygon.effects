using Pixygon.ID;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Pixygon.Effects
{
    [CreateAssetMenu(fileName = "New EffectData", menuName = "Pixygon/ID/New EffectData")]
    public class EffectData : IdObject {
        [SerializeField] public string _title;
        [SerializeField] public EffectType _effectType;
        [SerializeField] public AssetReference _effectPrefab;
        [SerializeField] public int _maxCapacity;
        [SerializeField] public int _startCapacity;
    }

    public enum EffectType {
        Impact = 0,
        Continuous = 1
    }
}