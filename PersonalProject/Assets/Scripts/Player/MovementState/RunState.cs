using UnityEngine;

public class RunState : IState
{
    private CharacterStatus Status;
    private CharacterMovement _movement;

    public RunState(CharacterMovement movement, CharacterStatus status)
    {
        Status = status;
        _movement = movement;
    }
    
    public void Enter()
    {
        Status.TargetSpeed = Status.RunSpeed;
    }
    
    public void Update()
    {
        if (Status.TargetDirection == Vector3.zero)
        {
            _movement.ChangeMovement(_movement.Standby);
            return;
        }
        if(Status.IsWalk.Value || Status.IsCrouch.Value)
            _movement.ChangeMovement(_movement.Walk);
    }
    public void Exit()
    {
        
    }
}
