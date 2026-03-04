using UnityEngine;

[System.Serializable]
public class CharacterStatus
{
    [field: SerializeField] public float RunSpeed { get; private set; } = 3.0f;
    [field: SerializeField] public float WalkSpeed { get; private set; } = 1.0f;
    [field: SerializeField] public float JumpHeight { get; private set; } = 2.0f;

    public float TargetSpeed { get; set; }
    public Vector3 TargetDirection;
    public ObserveValue<float> CurrentSpeed = new();
    public ObserveValue<float> JumpVelocity = new();
    public ObserveValue<bool> IsWalk = new();
    public ObserveValue<bool> IsJump = new();
    public ObserveValue<bool> IsCrouch = new();
    public ObserveValue<bool> IsAction = new();
}