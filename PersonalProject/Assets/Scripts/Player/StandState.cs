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
        _owner.Speed = 0.0f;
    }

    public void Update()
    {
        if (_owner.InputAxis != Vector3.zero)
        {
            _owner.ChangeMovement(_owner.Run);
        }
    }

    public void Exit()
    {
    }
}