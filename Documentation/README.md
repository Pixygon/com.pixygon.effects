Creating single-use Effects:
- Add EffectsManager-prefab to scene
- Call EffectsManager.SpawnEffect(position, effect-id) to spawn an effect at that position!

Triggering TimeEffects
- The TimeStop-effect is used to stop the time for a short while, usually when getting damaged
- To use, call TimeEffects.Stop(duration in ms)
- The Slowdown-effect is used to slow down the game, and slowly ease back to normal speed
- To use, call TimeEffects.Slowdown()