using UnityEngine;
using UnityEngine.Events;

public class AnimationReceiver : MonoBehaviour
{
    public UnityEvent OnEndLand = new();
    public void EndLand()
    {
        OnEndLand?.Invoke();
        Debug.Log("EndLand");
    }
}
