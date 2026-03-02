using UnityEngine;

public class RunState : IState
{
    private PlayerController _owner;

    public RunState(PlayerController pc)
    {
        _owner = pc;
    }
    
    public void Enter()
    {
        _owner.Speed.Value = 3.0f;
    }
    
    public void Update()
    {
        if (_owner.InputAxis.Value == Vector3.zero)
        {
            _owner.ChangeMovement(_owner.Standby);
            return;
        }
        if(_owner.IsWalk.Value)
            _owner.ChangeMovement(_owner.Walk);
    }
    public void Exit()
    {
        
    }
}
