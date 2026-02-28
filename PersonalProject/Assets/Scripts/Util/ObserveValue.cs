using UnityEngine;
using UnityEngine.Events;

public class ObserveValue<T> where T : struct
{
    private T _data = default;

    public T Value
    {
        get => _data;
        set
        {
            _data = value;
            OnValueChange?.Invoke(_data);
        }
    }

    public UnityEvent<T> OnValueChange = new UnityEvent<T>();

    public void AddListener(UnityAction<T> action)
    {
        OnValueChange.AddListener(action);
    }

    public void RemoveListener(UnityAction<T> action)
    {
        OnValueChange.RemoveListener(action);
    }
}