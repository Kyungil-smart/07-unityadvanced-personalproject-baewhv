using UnityEngine;

public class FallState : IState
{
    private CharacterStatus Status;
    private CharacterMovement _movement;

    public FallState(CharacterMovement movement, CharacterStatus status)
    {
        Status = status;
        _movement = movement;
    }
    public void Enter()
    {
        
    }

    public void Update()
    {
        if (_movement.isGrounded())
        {
            _movement.ChangeJumpState(_movement.Landed);
        }
    }

    public void Exit()
    {
    }
}