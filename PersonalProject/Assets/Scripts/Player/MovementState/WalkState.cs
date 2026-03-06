using UnityEngine;

public class WalkState : IState
{
    private CharacterStatus Status;
    private CharacterMovement _movement;

    public WalkState(CharacterMovement movement, CharacterStatus status)
    {
        Status = status;
        _movement = movement;
    }

    public void Enter()
    {
        Status.TargetSpeed = Status.WalkSpeed;
    }

    public void Update()
    {
        if (Status.TargetDirection == Vector3.zero)
        {
            _movement.ChangeMovement(_movement.Standby);
            return;
        }
        if(!Status.IsWalk.Value && !Status.IsCrouch.Value)
            _movement.ChangeMovement(_movement.Run);
    }

    public void Exit()
    {
    }
}