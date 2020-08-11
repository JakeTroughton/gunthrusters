using UnityEngine;
public interface IPoolable
{
    GameObject GameObject { get; }
    System.Action OnDeactivate { get; set; }
    void Activate(Vector2 position);
    void Deactivate();
}
