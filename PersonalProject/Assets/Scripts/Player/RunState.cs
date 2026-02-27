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
        _owner.Speed = 3.0f;
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
