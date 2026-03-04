using UnityEngine;

public class LandedState : IState
{
    private PlayerController _owner;

    public LandedState(PlayerController pc)
    {
        _owner = pc;
    }

    public void EndJump()
    {
        _owner.ChangeJumpState(_owner.JumpStandby);
    }

    public void Enter()
    {
        _owner.GetAnim.CrossFade("Jump_Land", 0.1f);
    }

    public void Update()
    {
    }

    public void Exit()
    {
    }
}