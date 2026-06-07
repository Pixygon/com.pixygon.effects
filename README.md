# Pixygon — Effects

Pooled VFX/effect manager and spawner. Spawns and recycles visual effects by id so
games don't allocate per-hit.

## Key types

| Type | What it is |
|---|---|
| **`EffectsManager`** | Spawns effects by id; owns the pools. |
| **`EffectData`** | A `ScriptableObject` describing an effect (prefab/addressable + params). |
| **`EffectObject`** | A live spawned effect instance. |
| **`PoolObject`** | Pooling base — recycle instead of Instantiate/Destroy. |
| **`FootstepEffects`** | Surface-aware footstep VFX/SFX. |
| **`ScoreEffectObject`** | Floating score/number popups. |
| **`TimeEffects`** | Time-based effect helpers. |

## Dependencies

`com.unity.addressables`, `com.pixygon.addressables`, `com.pixygon.idsystem`,
`com.pixygon.pagedcontent`, `com.pixygon.debugtool`.

## Usage

```csharp
EffectsManager.Instance.Spawn(effectId, position);   // pooled
```

## Status

`0.5.2`. Effects are addressable-loaded and pooled. For the future engine, the
spawn/timing logic can be split from the Unity spawn back-end.
