public class UnarmedMode : IState
{
    private Equipment _equipment;
    private CharacterMovement _movement;

    public UnarmedMode(Equipment equipment, CharacterMovement movement)
    {
        _equipment = equipment;
        _movement = movement;
    }

    public void Enter()
    {
        _equipment.CurrentMode = WeaponMode.Unarmed;
        _movement.ChangeAnimController(_equipment.Modes[(int)WeaponMode.Unarmed]);
    }

    public void Update()
    {
    }

    public void Exit()
    {
    }
}