using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualEffectManager : Singleton<VisualEffectManager>
{
    [SerializeField]
    private VisualEffect burstPrefab = default;
    private ObjectPool<VisualEffect> burstPool;

    protected override void Awake()
    {
        base.Awake();

        burstPool = new ObjectPool<VisualEffect>();
        burstPool.Populate(burstPrefab, 10, false);
    }

    public void ActivateBurst(Vector2 position)
    {
        burstPool.TryActivate(position, out _);
    }
}
