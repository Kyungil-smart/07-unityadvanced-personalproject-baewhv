using UnityEngine;

public class JumpStandbyState : IState
{
    private PlayerController _owner;

    public JumpStandbyState(PlayerController pc)
    {
        _owner = pc;
    }

    public void Enter()
    {
        _owner.IsJump.Value = false;
        _owner.JumpVelocity.Value = 0f;
    }

    public void Update()
    {
    }

    public void Exit()
    {
    }
}