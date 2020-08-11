using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : UnityEngine.Object, IPoolable
{
    private GameObject parent;
    private T original;
    private readonly List<T> pool = new List<T>();

    private int nextTestIndex = 0;
    private int NextTestIndex
    {
        get { return nextTestIndex; }
        set
        {
            nextTestIndex = value;
            if (nextTestIndex >= pool.Count)
                nextTestIndex = 0;
        }
    }

    private bool isPopulated = false;
    private bool isMutable = false;
    private bool toDestroy;
    private int stragglerCount;

    public bool TryActivate(Vector3 position, out T activatedObject)
    {
        activatedObject = default;
        if (!isPopulated)
        {
            Debug.LogError("Object pool " + parent.name + " isPopulated is FALSE");
            return false;
        }
        if (toDestroy)
        {
            Debug.LogError("Cannot activate pooled objects in list " + parent.name + " (toDestroy)");
            return false;
        }
        GameObject go = pool[NextTestIndex].GameObject;
        // see if the next logical item is good...
        if (go && !go.activeSelf)
        {
            activatedObject = pool[NextTestIndex];
            activatedObject.Activate(position);
            ++NextTestIndex;
            return true;
        }
        // otherwise iterate until one found...
        for (int i = 0; i < pool.Count; i++)
        {
            if (this == null)
                break;
            go = pool[i].GameObject;
            if (go && !go.activeSelf)
            {
                activatedObject = pool[i];
                activatedObject.Activate(position);
                NextTestIndex = i + 1;
                return true;
            }
        }
        // otherwise add one if we can...
        if (isMutable)
        {
            activatedObject = AddObject(original);
            activatedObject.Activate(position);
            ++NextTestIndex;
            return true;
        }
        // otherwise out null and return false
        activatedObject = default;
        return false;
    }

    public void Purge()
    {
        if (pool == null)
            return;
        foreach (var poolable in pool)
        {
            if (this != null && poolable != null)
                GameObject.Destroy(poolable.GameObject);
        }
        pool.Clear();
        GameObject.Destroy(parent);
        isPopulated = false;
    }

    public void MarkForDestruction()
    {
        toDestroy = true;

        for (int i = 0; i < pool.Count; i++)
        {
            //if (this != null && list != null && list[i] != null && list[i].GameObject != null && list[i].GameObject.activeSelf)
            if (pool[i].GameObject != null && pool[i].GameObject.activeSelf)
            {
                ++stragglerCount;
                pool[i].OnDeactivate += HandleStragglerDeactivated;
            }
        }
        if (stragglerCount == 0)
        {
            Purge();
        }
    }

    private void HandleStragglerDeactivated()
    {
        --stragglerCount;
        if (stragglerCount <= 0)
        {
            Purge();
        }
    }

    /// <summary> Populate the pool with a prefab, to a size that
    /// is mutable or immutable.</summary>
    public void Populate(T original, int size, bool mutable)
    {
        if (isPopulated)
        {
            Purge();
            pool.Capacity = size;
        }
        parent = new GameObject(string.Format("OBJECT POOL [{0}]", original.GameObject.name));
        this.original = original;
        for (int i = 0; i < size; i++)
        {
            AddObject(original);
        }
        this.isMutable = mutable;
        isPopulated = true;
    }

    private T AddObject(T original)
    {
        T g = UnityEngine.Object.Instantiate(original, parent.transform);
        g.GameObject.SetActive(false);
        pool.Add(g);
        return g;
    }

    public ObjectPool() { }
}