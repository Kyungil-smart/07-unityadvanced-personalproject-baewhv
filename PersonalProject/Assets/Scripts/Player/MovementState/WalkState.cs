using UnityEngine;

public class WalkState : IState
{
    private PlayerController _owner;

    public WalkState(PlayerController pc)
    {
        _owner = pc;
    }

    public void Enter()
    {
        _owner.Speed.Value = 1.0f;
    }

    public void Update()
    {
        if (_owner.InputAxis.Value == Vector3.zero)
        {
            _owner.ChangeMovement(_owner.Standby);
            return;
        }
        if(!_owner.IsWalk.Value)
            _owner.ChangeMovement(_owner.Run);
    }

    public void Exit()
    {
    }
}