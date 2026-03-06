using UnityEngine;

public class JumpStandbyState : IState
{
    private CharacterStatus Status;
    private CharacterMovement _movement;

    public JumpStandbyState(CharacterMovement movement, CharacterStatus status)
    {
        Status = status;
        _movement = movement;
    }

    public void Enter()
    {
        Status.IsJump.Value = false;
        Status.IsAction.Value = false;
        Status.JumpVelocity.Value = 0f;
    }

    public void Update()
    {
    }

    public void Exit()
    {
    }
}