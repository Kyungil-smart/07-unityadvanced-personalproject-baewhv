using UnityEngine;

public class StandState : IState
{
    private PlayerController _owner;

    public StandState(PlayerController pc)
    {
        _owner = pc;
    }

    public void Enter()
    {
        _owner.Speed.Value = 0.0f;
    }

    public void Update()
    {
        if (_owner.InputAxis.Value != Vector3.zero)
        {
            if (!_owner.IsWalk.Value)
                _owner.ChangeMovement(_owner.Run);
            else
                _owner.ChangeMovement(_owner.Walk);
        }
    }

    public void Exit()
    {
    }
}