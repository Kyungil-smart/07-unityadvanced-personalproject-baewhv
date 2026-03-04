using UnityEngine;

public class FallState : IState
{
    private PlayerController _owner;

    public FallState(PlayerController pc)
    {
        _owner = pc;
    }
    public void Enter()
    {
        _owner.GetAnim.CrossFade("Jump_Falling", 0.1f);
    }

    public void Update()
    {
        if (_owner.isGrounded())
        {
            _owner.ChangeJumpState(_owner.Landed);
        }
    }

    public void Exit()
    {
    }
}