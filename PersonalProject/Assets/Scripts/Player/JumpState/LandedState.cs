using UnityEngine;

public class LandedState : IState
{
    private CharacterStatus _status;
    private CharacterMovement _movement;

    public LandedState(CharacterMovement movement, CharacterStatus status)
    {
        _status = status;
        _movement = movement;
    }

    public void EndJump()
    {
        _movement.ChangeJumpState(_movement.JumpStandby);
        _status.IsAction.Value = false;
    }

    public void Enter()
    {
        _movement.GetAnim.CrossFade("Jump_Land", 0.1f);
    }

    public void Update()
    {
    }

    public void Exit()
    {
    }
}