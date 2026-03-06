using UnityEngine;

public class StandState : IState
{
    private CharacterStatus Status;
    private CharacterMovement _movement;

    public StandState(CharacterMovement movement, CharacterStatus status)
    {
        Status = status;
        _movement = movement;
    }

    public void Enter()
    {
        Status.TargetSpeed = 0.0f;
    }

    public void Update()
    {
        if (Status.TargetDirection != Vector3.zero)
        {
            if (Status.IsWalk.Value || Status.IsCrouch.Value)
                _movement.ChangeMovement(_movement.Walk);
            else
                _movement.ChangeMovement(_movement.Run);
        }
    }

    public void Exit()
    {
    }
}