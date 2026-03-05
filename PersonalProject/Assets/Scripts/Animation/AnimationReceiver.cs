using UnityEngine;
using UnityEngine.Events;

public class AnimationReceiver : MonoBehaviour
{
    public UnityEvent OnEndLand = new();
    public UnityEvent OnStartCollision = new();
    public UnityEvent OnEndCollision = new();
    public UnityEvent OnShoot = new();
    public UnityEvent OnStartEquip = new();

    private void EndLand() => OnEndLand?.Invoke();
    public void StartCollision() => OnStartCollision?.Invoke();
    public void EndCollision() => OnEndCollision?.Invoke();
    public void Shoot() => OnShoot?.Invoke();
    public void StartEquip() => OnStartEquip?.Invoke();
}   