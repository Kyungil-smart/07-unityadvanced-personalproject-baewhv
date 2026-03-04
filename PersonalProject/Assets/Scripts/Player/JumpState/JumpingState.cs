using UnityEngine;

public class JumpingState : IState

{
    private CharacterStatus Status;
    private CharacterMovement _movement;

    public JumpingState(CharacterMovement movement, CharacterStatus status)
    {
        Status = status;
        _movement = movement;
    }

    public void Enter()
    {
        _movement.GetAnim.CrossFade("Jump_Up", 0.1f);
        Status.JumpVelocity.Value = Mathf.Sqrt(Status.JumpHeight * -2f * Physics.gravity.y);
        Status.IsCrouch.Value = false;
        Status.IsAction.Value = true;
    }

    public void Update()
    {
        if (Status.JumpVelocity.Value < 0)
        {
            _movement.ChangeJumpState(_movement.Fall);
        }
    }

    public void Exit()
    {
    }
}