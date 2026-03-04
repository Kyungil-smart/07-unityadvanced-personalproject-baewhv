using UnityEngine;

public class JumpingState : IState

{
    private PlayerController _owner;

    public JumpingState(PlayerController pc)
    {
        _owner = pc;
    }

    public void Enter()
    {
        _owner.GetAnim.CrossFade("Jump_Up", 0.1f);
        _owner.JumpVelocity.Value = Mathf.Sqrt(_owner.JumpHeight * -2f * Physics.gravity.y);
        _owner.IsCrouch.Value = false;
    }

    public void Update()
    {
        if (_owner.JumpVelocity.Value < 0)
        {
            _owner.ChangeJumpState(_owner.Fall);
        }
    }

    public void Exit()
    {
    }
}