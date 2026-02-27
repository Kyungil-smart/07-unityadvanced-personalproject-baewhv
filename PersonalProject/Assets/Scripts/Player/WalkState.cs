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
        _owner.Speed = 1.0f;
    }

    public void Update()
    {
        if (_owner.InputAxis == Vector3.zero)
        {
            _owner.ChangeMovement(_owner.Standby);
        }
    }

    public void Exit()
    {
    }
}